using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;
using Trackmat.Lib.Models;
using Trackmat.Lib.Runners;

namespace Trackmat.Cli.Options
{
  [Verb("associate-items", HelpText = "Associates multiple Items to a period by their Easy Name")]
  public class AssociateItemsOptions
  {
    [Option('z', "period", Required = true, HelpText = "Easy Name of the Period. Ex. \"S5\" or \"sprint5\"")]
    public string EzName { get; set; }

    [Option('i', "items", Required = true, HelpText = "Items to associate with a period. Ex. \"CSV-2550 ABC-1234 DFG-7894\"")]
    public IEnumerable<string> Items { get; set; }

    public static int Run(AssociateItemsOptions opts)
    {
      var runner = new PeriodRunner();
      return runner.AssociateItems(new AssociateItemArgs { EzName = opts.EzName, Items = opts.Items });
    }
  }

}
