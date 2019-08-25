using System;
using System.Globalization;
using CommandLine;
using LiteDB;
using Trackmat.Lib.Enums;
using Trackmat.Lib.Models;
using Trackmat.Lib.Runners;

namespace Trackmat.Cli.Options
{
  [Verb("update-item", HelpText = "Updates an existing item in the tracker.")]
  public class UpdateItemOptions
  {
    [Option('I', "id", Required = true, HelpText = "Id of the Item to update. Ex. \"a1b2c3d4e5f6a1b2c3d4e5f6\"")]
    public string ItemId { get; set; }

    [Option('n', "name", Required = false, HelpText = "New name of the item to be saved. Ex. \"CSD-4550\" or \"Overtime\"")]
    public string NewName { get; set; }

    [Option('t', "time", Required = false, HelpText = "Amount of time it was worked on expresed in hours. Ex. 3.5 equals three hours and a half")]
    public float? Time { get; set; }

    [Option('d', "date", Required = false, HelpText = "Date when the item was worked on. Ex. 2020-05-16")]
    public string Date { get; set; }

    [Option('u', "url", Required = false, HelpText = "Url linking to a reference on this work. Ex. https://work-item-ticket-url.xyz")]
    public string Url { get; set; }

    [Option('r', "replace", Required = false, HelpText = "Replace previous entry with this updated one")]
    public bool Replace { get; set; }

    public UpdateOptions ToUpdateOptions()
    {
      ObjectId id;
      DateTime date;
      try
      {
        id = new ObjectId(ItemId);
      }
      catch (Exception)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"The \"[id: {ItemId}]\" is not a valid id. Ignoring...");
        Console.ResetColor();
        throw;
      }

      try
      {
        if (!string.IsNullOrEmpty(Date))
          date = DateTime.ParseExact(Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        else
          date = DateTime.MinValue;
      }
      catch (Exception)
      {
        date = DateTime.MinValue;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"The provided Date is not valid: \"{Date}\". Ignoring...");
        Console.ResetColor();
      }

      var definition = new TrackItem();
      definition.Time = Time.GetValueOrDefault(-1f);
      if (NewName != null)
      {
        definition.Item = NewName;
      }
      if (Url != null)
      {
        definition.Url = Url;
      }
      definition.Date = date;
      return new UpdateOptions
      {
        UpdateId = id,
        UpdateDefinition = definition,
        IsReplacing = Replace
      };

    }

    public static int Run(UpdateItemOptions opts)
    {
      var runner = new ItemRunner();
      int result = (int)ExitCodes.FailedToConvertArgs;
      try
      {
        var isNameEmpty = string.IsNullOrEmpty(opts.NewName?.Trim());
        var isTimeEmpty = !opts.Time.HasValue;

        if (opts.Replace && (isNameEmpty || isTimeEmpty))
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine("-r --replace switch is true");
          Console.WriteLine("but new name (-n --newname) or time (-t --time) are empty");
          Console.WriteLine("New name and Time are required if this is a replacement update");
          Console.WriteLine("Ex. \"trackmat update -I a1b2c3d4e5f6a1b2c3d4e5f6 -n NEW-002 -t 3.25 -r\"");
          Console.ResetColor();
          throw new Exception("Replacing without a name or time");
        }

        result = runner.Update(opts.ToUpdateOptions());
      }
      catch (Exception) { }
      return result;
    }
  }
}
