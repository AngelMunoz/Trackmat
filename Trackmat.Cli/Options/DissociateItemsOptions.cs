using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using LiteDB;
using Trackmat.Lib.Models;
using Trackmat.Runners;

namespace Trackmat.Cli.Options
{
  [Verb("dissociate-items", HelpText = "Dissociates multiple Items to a period by their Easy Name")]
  public class DissociateItemsOptions
  {
    [Option('z', "period", Required = true, HelpText = "Easy Name of the Period. Ex. \"S5\" or \"sprint5\"")]
    public string EzName { get; set; }

    [Option('i', "items", Required = false, HelpText = "Items to dissociate with a period. Ex. \"CSV-2550 ABC-1234 DFG-7894\"")]
    public IEnumerable<string> Items { get; set; }

    [Option('I', "ids", Required = false, HelpText = "Ids of the specific items to associate to this period. Ex. [a12b3c4d5e6fa12b3c4d5e6f]")]
    public IEnumerable<string> Ids { get; set; }

    public static int Run(DissociateItemsOptions opts)
    {
      var ids = opts.Ids?.Select(id =>
      {
        try
        {
          return new ObjectId(id);
        }
        catch (Exception)
        {
          Console.ForegroundColor = ConsoleColor.Yellow;
          Console.WriteLine($"[{id}] Is not a valid ObjectId. Ignoring...");
          return null;
        }
      })
        .Where(id => id != null);
      var runner = new PeriodRunner();
      return runner.DissociateItems(new DissociateItemArgs { EzName = opts.EzName, Items = opts.Items, Ids = ids });
    }
  }

}
