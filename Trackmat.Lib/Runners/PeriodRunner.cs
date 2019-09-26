using System;
using System.Collections.Generic;
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
  }
}
