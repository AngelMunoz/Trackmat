﻿using CommandLine;
using Trackmat.Cli.Options;
using Trackmat.Lib.Enums;

namespace Trackmat.Cli
{
  public class Program
  {
    static int Main(string[] args)
    {
      return Parser.Default
        .ParseArguments<InitOptions,
                        AddItemOptions,
                        ShowItemOptions,
                        UpdateItemOptions,
                        DeleteItemOptions,
                        AddPeriodOptions,
                        RunShowPeriodOptions,
                        UpdatePeriodOptions,
                        AssociateItemsOptions,
                        DissociateItemsOptions,
                        DeletePeriodOptions>(args)
        .MapResult(
          InitRunnerStart.RunInit(),
          ItemRunnerStart.RunAddItem(),
          ItemRunnerStart.RunShowItem(),
          ItemRunnerStart.RunUpdateItem(),
          ItemRunnerStart.RunDeleteItem(),
          PeriodRunnerStart.RunAddPeriod(),
          PeriodRunnerStart.RunShowPeriod(),
          PeriodRunnerStart.RunUpdatePeriod(),
          PeriodRunnerStart.RunAssociateItems(),
          PeriodRunnerStart.RunDissociateItems(),
          PeriodRunnerStart.RunDeletePeriod(),
          errs => (int)ExitCodes.ArgParseFailed
        );
    }


  }
}
