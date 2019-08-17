using System;
using CommandLine;
using Trackmat.Cli.Options;
using Trackmat.Lib.Enums;
using Trackmat.Lib.Runners;

namespace Trackmat.Cli
{
  class Program
  {
    static int Main(string[] args)
    {
      var result = Parser.Default.ParseArguments<InitOptions, AddItemOptions>(args)
        .MapResult(
          (InitOptions opts) => InitOptions.Run(opts),
          (AddItemOptions opts) =>
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
          },
          errs => (int)ExitCodes.ArgParseFailed
        );
      return result;
    }
  }
}
