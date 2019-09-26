using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using LiteDB;
using Trackmat.Lib.Models;
using Trackmat.Runners;

namespace Trackmat.Cli.Options
{
  [Verb("delete-item", HelpText = "Deletes an existing item (or items) by id or name")]
  public class DeleteItemOptions
  {
    [Option('I', "id", Required = false, HelpText = "Ids of the specific item to delete")]
    public IEnumerable<string> Ids { get; set; }

    [Option('n', "name", Required = false, HelpText = "name of the specific items to delete")]
    public IEnumerable<string> Names { get; set; }

    [Option("all-names", Required = false, HelpText = "Confirmation to delete all the items found with that name")]
    public bool AllNames { get; set; }

    [Option("all-ids", Required = false, HelpText = "Confirmation to delete all the items found with those ids")]
    public bool AllIds { get; set; }

    public DeleteTrackItemArgs ToDeleteTrackItemArgs()
    {
      var ids = Ids?.Select(id =>
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
      return new DeleteTrackItemArgs
      {
        Ids = ids,
        Names = Names,
        DeleteIds = AllIds,
        DeleteNames = AllNames
      };
    }

    public static int Run(DeleteItemOptions opts)
    {
      var runner = new ItemRunner();
      return runner.Delete(opts.ToDeleteTrackItemArgs());
    }
  }
}
