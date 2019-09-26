using System.Collections.Generic;
using CommandLine;
using Trackmat.Lib.Models;
using Trackmat.Runners;

namespace Trackmat.Cli.Options
{
  [Verb("dissociate-items", HelpText = "Dissociates multiple Items to a period by their Easy Name")]
  public class DissociateItemsOptions
  {
    [Option('z', "period", Required = true, HelpText = "Easy Name of the Period. Ex. \"S5\" or \"sprint5\"")]
    public string EzName { get; set; }

    [Option('i', "items", Required = true, HelpText = "Items to dissociate with a period. Ex. \"CSV-2550 ABC-1234 DFG-7894\"")]
    public IEnumerable<string> Items { get; set; }

    public static int Run(DissociateItemsOptions opts)
    {
      var runner = new PeriodRunner();
      return runner.DissociateItems(new DissociateItemArgs { EzName = opts.EzName, Items = opts.Items });
    }
  }

}
