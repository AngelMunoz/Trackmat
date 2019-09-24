using System;
using CommandLine;
using Trackmat.Cli.Options;
using Trackmat.Lib.Enums;
using Trackmat.Lib.Runners;

namespace Trackmat.Cli
{
  public class Program
  {
    static int Main(string[] args)
    {
      var result = Parser.Default
        .ParseArguments<InitOptions,
                        AddItemOptions,
                        ShowItemOptions,
                        UpdateItemOptions,
                        DeleteItemOptions,
                        AddPeriodOptions,
                        RunShowPeriodOptions>(args)
        .MapResult(
          InitRunnerStart.RunInit(),
          ItemRunnerStart.RunAddItem(),
          ItemRunnerStart.RunShowItem(),
          ItemRunnerStart.RunUpdateItem(),
          ItemRunnerStart.RunDeleteItem(),
          PeriodRunnerStart.RunAddPeriod(),
          PeriodRunnerStart.RunShowPeriod(),
          errs => (int)ExitCodes.ArgParseFailed
        );
      return result;
    }

    
  }
}
