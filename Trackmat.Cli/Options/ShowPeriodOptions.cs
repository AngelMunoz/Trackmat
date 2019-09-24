using CommandLine;
using Trackmat.Lib.Models;
using Trackmat.Lib.Runners;

namespace Trackmat.Cli.Options
{
  [Verb("show-period", HelpText = "Shows an Existing Period found by easy name")]
  public class RunShowPeriodOptions
  {
    [Option('z', "ezname", Required = true, HelpText = "short and easy name provided when created. Ex. \"MW3\"")]
    public string EzName { get; set; }

    [Option('d', "details", Required = false, HelpText = "Provide Complete Details about the found period.")]
    public bool Detailed { get; set; }

    public ShowPeriodOptions ToShowPeriodOptions()
    {
      return new ShowPeriodOptions
      {
        EzName = EzName,
        Detailed = Detailed
      };
    }

    public static int Run(RunShowPeriodOptions opts)
    {
      var runner = new PeriodRunner();
      return runner.Show(opts.ToShowPeriodOptions());
    }
  }
}
