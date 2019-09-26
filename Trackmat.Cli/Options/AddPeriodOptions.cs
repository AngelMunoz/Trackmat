using System;
using System.Globalization;
using CommandLine;
using Trackmat.Lib.Enums;
using Trackmat.Lib.Models;
using Trackmat.Runners;

namespace Trackmat.Cli.Options
{
  [Verb("add-period", HelpText = "Add a new period to the tracker.")]
  public class AddPeriodOptions
  {
    [Option('s', "startdate", Required = true, HelpText = "Start date of the period. Ex. 2020-05-16 (yyyy-mm-dd)")]
    public string StartDate { get; set; }

    [Option('e', "enddate", Required = true, HelpText = "End date of the period. Ex. 2020-05-20 (yyyy-mm-dd)")]
    public string EndDate { get; set; }

    [Option('n', "name", Required = true, HelpText = "Full Name of this period. Ex. \"May's third week\"")]
    public string Name { get; set; }

    [Option('z', "ezname", Required = true, HelpText = "Short and easy name to remember, query and asociate items with. Ex. \"MW3\" or \"mw3\"")]
    public string EzName { get; set; }

    public Period ToPeriod()
    {
      ParseDates(out DateTime startDate, out DateTime endDate);

      return new Period
      {
        StartDate = startDate,
        EndDate = endDate,
        Name = Name,
        EzName = EzName
      };
    }

    private void ParseDates(out DateTime startDate, out DateTime endDate)
    {
      try
      {
        startDate = DateTime.ParseExact(StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
      }
      catch (Exception e)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"The provided Start Date is not valid: {e.Message}");
        Console.ResetColor();
        throw new ArgumentException("Invalid StartDate Format", e);
      }

      try
      {
        endDate = DateTime.ParseExact(EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
      }
      catch (Exception e)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"The provided End Date is not valid: {e.Message}");
        Console.ResetColor();
        throw new ArgumentException("Invalid EndDate Format", e);
      }
    }

    public static int Run(AddPeriodOptions opts)
    {
      var runner = new PeriodRunner();
      int result = (int)ExitCodes.FailedToConvertArgs;
      try
      {
        result = runner.Create(opts.ToPeriod());
      }
      catch (Exception) { }
      return result;
    }
  }
}
