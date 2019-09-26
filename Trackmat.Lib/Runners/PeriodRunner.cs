using System;
using System.Collections.Generic;
using System.Linq;
using Trackmat.Lib.Enums;
using Trackmat.Lib.Models;
using Trackmat.Lib.Services;

namespace Trackmat.Lib.Runners
{
  public class PeriodRunner
  {
    public int Create(Period period)
    {
      using (var periods = new PeriodService())
      {
        if (periods.Exists(period.EzName))
        {
          Console.ForegroundColor = ConsoleColor.Yellow;
          Console.WriteLine($"A period with the Easy Name: \"{period.EzName}\" already exists!");
          Console.WriteLine($"Please run \"trackmat show-period -z {period.EzName}\" to see it");
          Console.ResetColor();
          return (int)ExitCodes.PeriodExists;
        }
        try
        {
          period.Items = new List<TrackItem>();
          var created = periods.Create(period);
          Console.ForegroundColor = ConsoleColor.Green;
          Console.WriteLine($"Saved [{created.Id} - {created.Name}] Successfully \n{created}");
          Console.ResetColor();
        }
        catch (Exception e)
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine($"Failed to Create {period.Name}. {e.Message}");
          Console.ResetColor();
          return (int)ExitCodes.FailedToCreateItem;
        }
        return (int)ExitCodes.Success;
      }
    }

    public int Show(ShowPeriodOptions opts)
    {
      using (var periods = new PeriodService())
      {
        try
        {
          var found = periods.FindOne(opts.EzName);
          Console.ForegroundColor = ConsoleColor.Green;
          if (!opts.Detailed)
          {
            Console.WriteLine(found);
          }
          else
          {
            Console.WriteLine(found);
            foreach (var item in found.Items)
            {
              Console.WriteLine($"\t{item}");
            }
          }
          return (int)ExitCodes.Success;
        }
        catch (Exception e)
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine($"Failed to show period. {e.Message}");
          Console.ResetColor();
          return (int)ExitCodes.FailedToShowPeriod;
        }
      }
    }

    public int Update(UpdatePeriodArgs args)
    {
      using (var periods = new PeriodService())
      {
        Period found;
        var toUpdate = new Period();
        try
        {
          found = periods.FindOne(args.EzName);
          if (found == null) throw new ArgumentNullException("Period Not Found");
          Console.ForegroundColor = ConsoleColor.Yellow;
          Console.WriteLine($"Updating Found Period\n{found}");
          Console.ResetColor();
        }
        catch (ArgumentNullException)
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine($"Period with Easy Name [{args.EzName}] could not be found.");
          Console.ResetColor();
          return (int)ExitCodes.FailedToUpdate;
        }
        catch (Exception e)
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine($"Failed to Update Period with Easy Name [{args.EzName}], {e.Message}");
          Console.ResetColor();
          return (int)ExitCodes.FailedToUpdate;
        }

        if (args.StartDate != DateTime.MinValue)
        {
          found.StartDate = args.StartDate;
        }

        if (args.EndDate != DateTime.MinValue)
        {
          found.EndDate = args.EndDate;
        }

        if (!string.IsNullOrEmpty(args.Name) && !string.IsNullOrWhiteSpace(args.Name))
        {
          found.Name = args.Name;
        }

        var updated = periods.UpdateOne(found);
        if (!updated)
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine($"Failed to Update Period with Easy Name [{args.EzName}]");
          Console.ResetColor();
          return (int)ExitCodes.FailedToUpdate;
        }
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Period Updated\n{found}");
        Console.ResetColor();
        return (int)ExitCodes.Success;
      }
    }

    public int AssociateItems(AssociateItemArgs args)
    {
      IEnumerable<TrackItem> items;
      Period found;
      using (var _items = new TrackItemService())
      {
        items = _items.Find(args.Items);
      }
      using (var periods = new PeriodService())
      {
        found = periods.FindOne(args.EzName);
      }
      if (found == null)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Period with Easy Name [{args.EzName}] Not Found");
        Console.ResetColor();
        return (int)ExitCodes.PeriodNotFound;
      }

      if (items == null || items.Count() <= 0)
      {
        var joined = args.Items.Aggregate("", (prev, next) => $"{(string.IsNullOrEmpty(prev) ? $"{prev}" : $"{prev}, ")}{next}");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"We did not find any items for [{joined}]");
        Console.ResetColor();
        return (int)ExitCodes.EmptyAssociation;
      }

      ConsoleKeyInfo key = new ConsoleKeyInfo();
      while (key.Key != ConsoleKey.Y && key.Key != ConsoleKey.Enter)
      {
        Console.WriteLine($"Found Period\n{found}");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Are you sure you want to associate the following items? [Y/n]");
        Console.ResetColor();
        var joined = items.Aggregate("", (prev, next) => $"{(string.IsNullOrEmpty(prev) ? $"{prev}" : $"{prev}\n")}{next}");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"({joined})");
        Console.ResetColor();
        key = Console.ReadKey();
        if (key.Key == ConsoleKey.N) return (int)ExitCodes.Success;
      }
      // if the item is already there don't add it
      var set = new HashSet<string>(from item in found.Items select item.Item);
      var newItems = items.Where(item => !set.Contains(item.Item));
      found.Items = newItems.Concat(found.Items);
      bool updated = false;

      using (var periods = new PeriodService())
      {
        updated = periods.UpdateOne(found);
      }

      if (!updated)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("We could not associate the items");
        return (int)ExitCodes.FailedToUpdate;
      }
      Console.ForegroundColor = ConsoleColor.Green;
      Console.Write($"\nUpdated\n{found}");
      return (int)ExitCodes.Success;
    }

    public int DissociateItems(DissociateItemArgs args)
    {
      Period found;
      using (var periods = new PeriodService())
      {
        found = periods.FindOne(args.EzName);
      }
      if (found == null)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Period with Easy Name [{args.EzName}] Not Found");
        Console.ResetColor();
        return (int)ExitCodes.PeriodNotFound;
      }

      if (args.Items == null || args.Items.Count() <= 0)
      {
        var joined = args.Items.Aggregate("", (prev, next) => $"{(string.IsNullOrEmpty(prev) ? $"{prev}" : $"{prev}, ")}{next}");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"We did not find any items for [{joined}]");
        Console.ResetColor();
        return (int)ExitCodes.EmptyDissociation;
      }

      ConsoleKeyInfo key = new ConsoleKeyInfo();
      while (key.Key != ConsoleKey.Y && key.Key != ConsoleKey.Enter)
      {
        Console.WriteLine($"Found Period\n{found}");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Are you sure you want to dissociate the following items? [Y/n]");
        Console.ResetColor();
        var joined = args.Items.Aggregate("", (prev, next) => $"{(string.IsNullOrEmpty(prev) ? $"{prev}" : $"{prev}, ")}{next}");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"[{joined}]");
        Console.ResetColor();
        key = Console.ReadKey();
        if (key.Key == ConsoleKey.N) return (int)ExitCodes.Success;
      }

      var remaining = found.Items.SkipWhile(item => args.Items.Contains(item.Item));
      found.Items = remaining;
      bool updated = false;

      using (var periods = new PeriodService())
      {
        updated = periods.UpdateOne(found);
      }

      if (!updated)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("We could not dissociate the items");
        return (int)ExitCodes.FailedToUpdate;
      }
      Console.ForegroundColor = ConsoleColor.Green;
      Console.Write($"\nUpdated\n{found}");
      return (int)ExitCodes.Success;
    }
  }
}
