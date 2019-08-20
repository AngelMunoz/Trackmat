using System;
using System.Globalization;
using CommandLine;
using Trackmat.Lib.Enums;
using Trackmat.Lib.Models;
using Trackmat.Lib.Runners;

namespace Trackmat.Cli.Options
{
  [Verb("add-item", HelpText = "Add a new item to the tracker.")]
  public class AddItemOptions
  {
    [Option('i', "item", Required = true, HelpText = "Item to be saved. Ex. \"CSD-4550\" or \"Overtime\"")]
    public string Item { get; set; }

    [Option('t', "time", Required = true, HelpText = "Amount of time it was worked on expresed in hours. Ex. 3.5 equals three hours and a half")]
    public float Time { get; set; }

    [Option('z', "ezname", Required = false, HelpText = "name used to asociate this item with a specific period. Ex. \"Sprint5\"")]
    public string EzName { get; set; }

    [Option('d', "date", Required = false, HelpText = "Date when the item was worked on. Ex. 2020-05-16")]
    public string Date { get; set; }

    [Option('u', "url", Required = false, HelpText = "Url linking to a reference on this work. Ex. https://work-item-ticket-url.xyz")]
    public string Url { get; set; }

    public TrackItem ToTrackItem()
    {
      DateTime parsedDate;
      try
      {
        parsedDate = DateTime.ParseExact(Date ?? DateTime.UtcNow.ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.InvariantCulture);
      }
      catch (Exception e)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"The provided Date is not valid: {e.Message}");
        Console.ResetColor();
        throw;
      }

      return new TrackItem
      {
        Item = Item,
        Time = Time,
        Date = parsedDate,
        Url = Url
      };
    }

    public static int Run(AddItemOptions opts)
    {
      var runner = new ItemRunner();
      int result = (int)ExitCodes.FailedToConvertArgs;
      try
      {
        result = runner.Create(opts.ToTrackItem());
      }
      catch (Exception) { }
      return result;
    }
  }
}
