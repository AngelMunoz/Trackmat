using CommandLine;
using Trackmat.Lib.Models;
using Trackmat.Runners;

namespace Trackmat.Cli.Options
{
  [Verb("show-period", HelpText = "Shows an Existing Period found by easy name")]
  public class RunShowPeriodOptions
  {
    [Option('z', "ezname", Required = false, HelpText = "short and easy name provided when created. Ex. \"MW3\"")]
    public string EzName { get; set; }

    [Option('a', "all", Required = false, HelpText = "List all of the existing periods in a paginated fashion")]
    public bool All { get; set; }

    [Option('d', "details", Required = false, HelpText = "Provide Complete Details about the found period.")]
    public bool Detailed { get; set; }

    [Option('p', "page", Default = 1, HelpText = "Number of the page to list on screen. Used only with \"-a\" flag")]
    public int Page { get; set; }

    [Option('l', "limit", Default = 10, HelpText = "Amount of items to show at once.  Used only with \"-a\" flag")]
    public int Limit { get; set; }


    public ShowPeriodArgs ToShowPeriodArgs()
    {
      return new ShowPeriodArgs
      {
        EzName = EzName,
        Detailed = Detailed,
        All = All,
        Pagination = new PaginationValues { Limit = Limit, Page = Page }
      };
    }

    public static int Run(RunShowPeriodOptions opts)
    {
      var runner = new PeriodRunner();
      return runner.Show(opts.ToShowPeriodArgs());
    }
  }
}
