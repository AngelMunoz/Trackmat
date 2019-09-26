using System;
using CommandLine;
using LiteDB;
using Trackmat.Lib.Models;
using Trackmat.Runners;

namespace Trackmat.Cli.Options
{
  [Verb("show-item", HelpText = "Shows an existing item either by id or by name")]
  public class ShowItemOptions
  {
    [Option('i', "item", Required = false, HelpText = "Name of the Item to look for. Ex. \"ABC-002\"")]
    public string Item { get; set; }

    [Option('d', "details", Default = false, Required = false, HelpText = "Show Full details of the items found in match")]
    public bool Details { get; set; }

    [Option('p', "page", Default = 1, HelpText = "Number of the page to list on screen")]
    public int Page { get; set; }

    [Option('l', "limit", Default = 10, HelpText = "Amount of items to show at once")]
    public int Limit { get; set; }

    [Option('I', "id", HelpText = "If you know the Id, show that specific item")]
    public string ItemId { get; set; }

    public ShowTrackItemArgs ToShowTrackItemArgs()
    {
      ObjectId id = null;
      try
      {
        if (ItemId != null)
          id = new ObjectId(ItemId);
      }
      catch (Exception)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"The \"[id: {ItemId}]\" is not a valid id. Ignoring...");
        Console.ResetColor();
      }
      return new ShowTrackItemArgs
      {
        Name = Item,
        Pagination = new PaginationValues { Page = Page, Limit = Limit },
        ItemId = id,
        Detailed = Details
      };
    }
    public static int Run(ShowItemOptions opts)
    {
      var runner = new ItemRunner();
      return runner.Show(opts.ToShowTrackItemArgs());
    }
  }
}
