using System;
using System.IO;
using CommandLine;
using Trackmat.Lib.Models;
using Trackmat.Runners;

namespace Trackmat.Cli.Options
{
  [Verb("init", HelpText = "Helps you initialize the application and set some defaults")]
  public class InitOptions
  {
    [Option('h', "homedir", Required = false, HelpText = "Lets you choose where to store the settings and db. Default $HOME/.trackmat")]
    public string HomeDir { get; set; }

    public InitConfig ToInitConfig()
    {
      var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
      return new InitConfig
      {
        HomeDirectory = HomeDir ?? Path.Combine(appData, "trackmat")
      };
    }

    public static int Run(InitOptions opts)
    {
      var runner = new InitRunner();
      var result = runner.Init(opts.ToInitConfig());
      return result;
    }
  }
}
