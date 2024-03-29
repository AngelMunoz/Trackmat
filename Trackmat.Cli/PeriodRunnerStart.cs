﻿using System;
using Trackmat.Cli.Options;
using Trackmat.Lib.Enums;
using Trackmat.Runners;

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

    public static Func<UpdatePeriodOptions, int> RunUpdatePeriod()
    {
      return (UpdatePeriodOptions opts) =>
      {
        if (!InitRunner.IsConfigured())
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine("Trackmat is not configured yet");
          Console.WriteLine("Please run \"trackmat init\" to configure Trackmat");
          Console.ResetColor();
          return (int)ExitCodes.NotConfigured;
        }
        return UpdatePeriodOptions.Run(opts);
      };
    }

    public static Func<DeletePeriodOptions, int> RunDeletePeriod()
    {
      return (DeletePeriodOptions opts) =>
      {
        if (!InitRunner.IsConfigured())
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine("Trackmat is not configured yet");
          Console.WriteLine("Please run \"trackmat init\" to configure Trackmat");
          Console.ResetColor();
          return (int)ExitCodes.NotConfigured;
        }
        return DeletePeriodOptions.Run(opts);
      };
    }

    public static Func<AssociateItemsOptions, int> RunAssociateItems()
    {
      return (AssociateItemsOptions opts) =>
      {
        if (!InitRunner.IsConfigured())
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine("Trackmat is not configured yet");
          Console.WriteLine("Please run \"trackmat init\" to configure Trackmat");
          Console.ResetColor();
          return (int)ExitCodes.NotConfigured;
        }
        return AssociateItemsOptions.Run(opts);
      };
    }

    public static Func<DissociateItemsOptions, int> RunDissociateItems()
    {
      return (DissociateItemsOptions opts) =>
      {
        if (!InitRunner.IsConfigured())
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine("Trackmat is not configured yet");
          Console.WriteLine("Please run \"trackmat init\" to configure Trackmat");
          Console.ResetColor();
          return (int)ExitCodes.NotConfigured;
        }
        return DissociateItemsOptions.Run(opts);
      };
    }
  }
}
