using System;
using Trackmat.Cli.Options;
using Trackmat.Lib.Enums;
using Trackmat.Runners;

namespace Trackmat.Cli
{
  public static class ItemRunnerStart
  {
    public static Func<DeleteItemOptions, int> RunDeleteItem()
    {
      return (DeleteItemOptions opts) =>
      {
        if (!InitRunner.IsConfigured())
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine("Trackmat is not configured yet");
          Console.WriteLine("Please run \"trackmat init\" to configure Trackmat");
          Console.ResetColor();
          return (int)ExitCodes.NotConfigured;
        }
        return DeleteItemOptions.Run(opts);
      };
    }

    public static Func<UpdateItemOptions, int> RunUpdateItem()
    {
      return (UpdateItemOptions opts) =>
      {
        if (!InitRunner.IsConfigured())
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine("Trackmat is not configured yet");
          Console.WriteLine("Please run \"trackmat init\" to configure Trackmat");
          Console.ResetColor();
          return (int)ExitCodes.NotConfigured;
        }
        return UpdateItemOptions.Run(opts);
      };
    }

    public static Func<ShowItemOptions, int> RunShowItem()
    {
      return (ShowItemOptions opts) =>
      {
        if (!InitRunner.IsConfigured())
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine("Trackmat is not configured yet");
          Console.WriteLine("Please run \"trackmat init\" to configure Trackmat");
          Console.ResetColor();
          return (int)ExitCodes.NotConfigured;
        }
        return ShowItemOptions.Run(opts);
      };
    }

    public static Func<AddItemOptions, int> RunAddItem()
    {
      return (AddItemOptions opts) =>
      {
        if (!InitRunner.IsConfigured())
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine("Trackmat is not configured yet");
          Console.WriteLine("Please run \"trackmat init\" to configure Trackmat");
          Console.ResetColor();
          return (int)ExitCodes.NotConfigured;
        }
        return AddItemOptions.Run(opts);
      };
    }


  }
}
