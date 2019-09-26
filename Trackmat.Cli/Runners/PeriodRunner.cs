using System;
using System.Collections.Generic;
using System.Linq;
using Trackmat.Lib.Enums;
using Trackmat.Lib.Models;
using Trackmat.Lib.Services;

namespace Trackmat.Runners
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

    public int Show(ShowPeriodArgs opts)
    {
      if (!opts.All && string.IsNullOrEmpty(opts.EzName))
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("You must provide either \"-a\" or the \"-z\" flags");
        Console.WriteLine("Ex. [show-period -a -p 1 -l 10] or [show-period -d -z s5]");
        Console.ResetColor();
        return (int)ExitCodes.MissingArguments;
      }
      using var periods = new PeriodService();
      if (opts.All)
      {
        return FindAllPeriods(opts.Pagination, opts.Detailed);
      }

      try
      {
        return FindSpecificPeriod((opts.EzName, opts.Detailed), periods);
      }
      catch (Exception e)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Failed to show period. {e.Message}");
        Console.ResetColor();
        return (int)ExitCodes.FailedToShowPeriod;
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

    public int DeletePeriod(DeletePeriodArgs args)
    {
      Period found;
      using (var periods = new PeriodService())
      {
        found = periods.FindOne(args.EzName);
        if (found == null) return (int)ExitCodes.PeriodNotFound;
      }

      ConsoleKeyInfo key = new ConsoleKeyInfo();
      if (args.Dissociate)
      {
        key = new ConsoleKeyInfo('Y', ConsoleKey.Y, false, false, false);
      }
      while (key.Key != ConsoleKey.Y && key.Key != ConsoleKey.Enter)
      {
        Console.WriteLine($"Found Period\n{found}");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Are you sure you want to dissociate the following items? [Y/n]");
        Console.ResetColor();
        var joined = found.Items.Aggregate("", (prev, next) => $"{(string.IsNullOrEmpty(prev) ? $"{prev}" : $"{prev}\n")}{next}");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"({joined})");
        Console.ResetColor();
        key = Console.ReadKey();
        if (key.Key == ConsoleKey.N) return (int)ExitCodes.Success;
      }
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine($"Proceeding to Delete\n{found}");
      var deleted = false;
      using (var periods = new PeriodService())
      {
        deleted = periods.Delete(found.Id);
      }
      if (!deleted)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Failed to Delete\n{found}");
        Console.ResetColor();
        return (int)ExitCodes.FailedToDeletePeriod;
      }
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine($"Deleted ({found}) Successfully");
      Console.ResetColor();
      return (int)ExitCodes.Success;
    }

    public int AssociateItems(AssociateItemArgs args, ConsoleKeyInfo prePrompt = new ConsoleKeyInfo())
    {
      using var periods = new PeriodService();
      IEnumerable<TrackItem> items;
      Period found;
      found = periods.FindOne(args.EzName);
      if (found == null)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Period with Easy Name [{args.EzName}] Not Found");
        Console.ResetColor();
        return (int)ExitCodes.PeriodNotFound;
      }

      var _items = new TrackItemService();
      items = _items.Find(args.Items);
      if (args?.Ids?.Count() > 0)
      {
        var temp = _items.Find(args.Ids);
        items = items.Concat(temp);
      }
      if (items == null || items.Count() <= 0)
      {
        var joined = args.Items.Aggregate("", (prev, next) => $"{(string.IsNullOrEmpty(prev) ? $"{prev}" : $"{prev}, ")}{next}");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"We did not find any items for [{joined}]");
        Console.ResetColor();
        return (int)ExitCodes.EmptyAssociation;
      }

      ConsoleKeyInfo key = prePrompt;
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
      updated = periods.UpdateOne(found);

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

    public int DissociateItems(DissociateItemArgs args, ConsoleKeyInfo prePrompt = new ConsoleKeyInfo())
    {
      using var periods = new PeriodService();
      var found = periods.FindOne(args.EzName);
      if (found == null)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Period with Easy Name [{args.EzName}] Not Found");
        Console.ResetColor();
        return (int)ExitCodes.PeriodNotFound;
      }

      if (args?.Items?.Count() <= 0 && args?.Ids?.Count() <= 0)
      {
        var joined = args?.Items.Aggregate("", (prev, next) => $"{(string.IsNullOrEmpty(prev) ? $"{prev}" : $"{prev}, ")}{next}");
        var idsjoined = args?.Ids.Aggregate("", (prev, next) => $"{(string.IsNullOrEmpty(prev) ? $"{prev}" : $"{prev}, ")}{next}");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"We did not find any items for [Names] or [Ids]");
        Console.ResetColor();
        return (int)ExitCodes.EmptyDissociation;
      }

      ConsoleKeyInfo key = prePrompt;
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

      var remaining = found.Items.SkipWhile(item => args.Items.Contains(item.Item) || args.Ids.Contains(item.Id));
      found.Items = remaining;
      bool updated = false;

      updated = periods.UpdateOne(found);

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

    private int FindAllPeriods(PaginationValues pagination, bool detailed)
    {
      using var periods = new PeriodService();

      var paginated = periods.FindAll(pagination);
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine($"Found [{paginated.Count}] {(paginated.Count == 1 ? "Period" : "Periods")}");
      try
      {
        foreach (var period in paginated.List)
        {
          Console.ForegroundColor = ConsoleColor.Green;
          Console.WriteLine(period);
          if (detailed)
          {
            Console.ForegroundColor = ConsoleColor.Blue;
            foreach (var item in period.Items)
            {
              Console.WriteLine($"\t{item}");
            }
          }
          Console.ResetColor();
        }
      }
      catch (Exception e)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Failed to show periods {e.Message}");
        return (int)ExitCodes.FailedToShowPeriod;
      }
      return (int)ExitCodes.Success;
    }

    private int FindSpecificPeriod((string, bool) opts, PeriodService periods)
    {
      var (ezName, detailed) = opts;
      var found = periods.FindOne(ezName);
      if (found == null)
      {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Period With Easy Name [{ezName}] Not found");
        Console.ResetColor();
        return (int)ExitCodes.PeriodNotFound;
      }
      Console.ForegroundColor = ConsoleColor.Green;
      if (!detailed)
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
  }
}
