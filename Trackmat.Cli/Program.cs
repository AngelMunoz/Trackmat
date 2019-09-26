using CommandLine;
using Trackmat.Cli.Options;
using Trackmat.Lib.Enums;

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
                        RunShowPeriodOptions,
                        UpdatePeriodOptions>(args)
        .MapResult(
          InitRunnerStart.RunInit(),
          ItemRunnerStart.RunAddItem(),
          ItemRunnerStart.RunShowItem(),
          ItemRunnerStart.RunUpdateItem(),
          ItemRunnerStart.RunDeleteItem(),
          PeriodRunnerStart.RunAddPeriod(),
          PeriodRunnerStart.RunShowPeriod(),
          PeriodRunnerStart.RunUpdatePeriod(),
          errs => (int)ExitCodes.ArgParseFailed
        );
      return result;
    }


  }
}
