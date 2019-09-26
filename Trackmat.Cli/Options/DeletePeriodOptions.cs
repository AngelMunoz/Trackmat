using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;
using Trackmat.Lib.Models;
using Trackmat.Runners;

namespace Trackmat.Cli.Options
{
  [Verb("delete-period", HelpText = "Deletes a period, if a period has items it will ask for dissociation")]
  public class DeletePeriodOptions
  {
    [Option('z', "easyname", Required = true, HelpText = "Easy name of the period. Ex. \"S5\" or \"sprint5\"")]
    public string EzName { get; set; }

    [Option('d', "dissociate", Required = false, HelpText = "Dissociate items without prompt")]
    public bool Dissociate { get; set; }

    public DeletePeriodArgs ToDeletePeriodArgs()
    {
      return new DeletePeriodArgs { EzName = EzName, Dissociate = Dissociate };
    }

    public static int Run(DeletePeriodOptions opts)
    {
      var runner = new PeriodRunner();
      return runner.DeletePeriod(opts.ToDeletePeriodArgs());
    }
  }
}
