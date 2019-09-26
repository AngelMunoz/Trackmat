using System;
using System.Globalization;
using CommandLine;
using Trackmat.Lib.Enums;
using Trackmat.Lib.Models;
using Trackmat.Lib.Runners;

namespace Trackmat.Cli.Options
{
  [Verb("update-period", HelpText = "Updates an existing period from given easy name and update properties")]
  public class UpdatePeriodOptions
  {
    [Option('z', "easyname", Required = true, HelpText = "Easy name given for the period. Ex. \"s5\" or \"sprint5\"")]
    public string EzName { get; set; }

    [Option('s', "startdate", Required = false, HelpText = "Start date of the period. Ex. 2020-05-16 (yyyy-mm-dd)")]
    public string StartDate { get; set; }

    [Option('e', "enddate", Required = false, HelpText = "End date of the period. Ex. 2020-05-20 (yyyy-mm-dd)")]
    public string EndDate { get; set; }

    [Option('n', "name", Required = false, HelpText = "Full Name of this period. Ex. \"May's third week\"")]
    public string Name { get; set; }

    public UpdatePeriodArgs ToUpdatePeriodArgs()
    {
      ParseDates(out DateTime startDate, out DateTime endDate);

      return new UpdatePeriodArgs
      {
        StartDate = startDate,
        EndDate = endDate,
        Name = Name,
        EzName = EzName
      };
    }

    public static int Run(UpdatePeriodOptions opts)
    {
      var runner = new PeriodRunner();
      int result = (int)ExitCodes.FailedToConvertArgs;
      try
      {
        result = runner.Update(opts.ToUpdatePeriodArgs());
      }
      catch (Exception) { }
      return result;
    }


    private void ParseDates(out DateTime startDate, out DateTime endDate)
    {
      try
      {
        if (string.IsNullOrEmpty(StartDate) || string.IsNullOrWhiteSpace(StartDate))
          startDate = DateTime.MinValue;
        else
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
        if (string.IsNullOrEmpty(EndDate) || string.IsNullOrWhiteSpace(EndDate))
          endDate = DateTime.MinValue;
        else
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
  }
}
