using System;
using Trackmat.Cli.Options;
using Trackmat.Lib.Enums;
using Trackmat.Lib.Runners;

namespace Trackmat.Cli
{
  public static class PeriodRunnerStart
  {
    public static Func<AddPeriodOptions, int> RunAddPeriod()
    {
      return (AddPeriodOptions opts) =>
      {
        if (!InitRunner.IsConfigured())
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine("Trackmat is not configured yet");
          Console.WriteLine("Please run \"trackmat init\" to configure Trackmat");
          Console.ResetColor();
          return (int)ExitCodes.NotConfigured;
        }
        return AddPeriodOptions.Run(opts);
      };
    }

    public static Func<RunShowPeriodOptions, int> RunShowPeriod()
    {
      return (RunShowPeriodOptions opts) =>
      {
        if (!InitRunner.IsConfigured())
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine("Trackmat is not configured yet");
          Console.WriteLine("Please run \"trackmat init\" to configure Trackmat");
          Console.ResetColor();
          return (int)ExitCodes.NotConfigured;
        }
        return RunShowPeriodOptions.Run(opts);
      };
    }
  }
}
