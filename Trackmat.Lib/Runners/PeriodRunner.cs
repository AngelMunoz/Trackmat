using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
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
  }
}
