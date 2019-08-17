using CommandLine;

namespace Trackmat.Cli.Options
{
  [Verb("add-period", HelpText = "Add a new period to the tracker.")]
  public class AddPeriodOptions
  {
    [Option('s', "startdate", Required = true, HelpText = "Start date of the period. Ex. 2020-05-16")]
    public string StartDate { get; set; }

    [Option('e', "enddate", Required = true, HelpText = "End date of the period. Ex. 2020-05-20")]
    public string EndDate { get; set; }

    [Option('n', "name", Required = true, HelpText = "Full Name of this period. Ex. \"May's third week\"")]
    public string name { get; set; }

    [Option('z', "ezname", Required = true, HelpText = "Short and easy name to remember, query and asociate items with. Ex. \"MW3\"")]
    public string EzName { get; set; }
  }
}
