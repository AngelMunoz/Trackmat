using System;
using Trackmat.Lib.Enums;
using Trackmat.Lib.Models;
using Trackmat.Lib.Services;

namespace Trackmat.Lib.Runners
{
  public class ItemRunner
  {

    public int Create(TrackItem target)
    {
      using (var items = new TrackItemService())
      {
        try
        {
          var created = items.Create(target);
          Console.ForegroundColor = ConsoleColor.Green;
          Console.WriteLine($"Saved [{created.Id} - {created.Item}] Successfully \n{created}");
          Console.ResetColor();
        }
        catch (Exception e)
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine($"Failed to Create {target.Item}. {e.Message}");
          Console.ResetColor();
          return (int)ExitCodes.FailedToCreateItem;
        }
        return (int)ExitCodes.Success;
      }
    }

    public int Show(ShowOptions options)
    {
      using (var items = new TrackItemService())
      {
        try
        {
          PaginatedResult<TrackItem> result;
          if (options.ItemId != null)
          {
            var item = items.FindOne(options.ItemId);
            result = new PaginatedResult<TrackItem>
            {
              List = new TrackItem[] { item },
              Count = 1
            };
          }
          else
          {
            result = items.Find(options.Name, options.Pagination);
          }
          Console.ForegroundColor = ConsoleColor.Green;
          Console.WriteLine($"Found [{result.Count}] {(result.Count == 1 ? "Item" : "Items")}");
          foreach (var item in result.List)
          {
            if (options.Detailed)
            {
              Console.WriteLine($"[{item.Id} - ({item.Item})] - {item.Time}h {item.Date.ToShortDateString()} {item.Url}");
            }
            else
            {
              Console.WriteLine(item);
            }
          }
          Console.ResetColor();
          return (int)ExitCodes.Success;
        }
        catch (Exception e)
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine($"Failed to show items. {e.Message}");
          Console.ResetColor();
          return (int)ExitCodes.FailedToShowItems;
        }
      }
    }

    public int Update(UpdateOptions options)
    {
      using (var items = new TrackItemService())
      {
        var toUpdate = items.FindOne(options.UpdateId);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Found: {toUpdate}");
        if (options.IsReplacing)
        {
          options.UpdateDefinition.Id = toUpdate.Id;
          Console.ForegroundColor = ConsoleColor.Yellow;
          Console.WriteLine($"Will Replace With: {options.UpdateDefinition}");
          Console.WriteLine($"Are you Sure to Continue?");
          var key = Console.ReadKey();
          if (key.Key == ConsoleKey.Y)
          {
            return items.UpdateOne(options.UpdateDefinition) ? (int)ExitCodes.Success : (int)ExitCodes.FailedToUpdate;
          }
          Console.ResetColor();
          return (int)ExitCodes.FailedToUpdate;
        }

        if (options.UpdateDefinition.Item != null)
        {
          toUpdate.Item = options.UpdateDefinition.Item;
        }

        if (options.UpdateDefinition.Time != -1f)
        {
          toUpdate.Time = options.UpdateDefinition.Time;
        }

        if (options.UpdateDefinition.Date != DateTime.MinValue)
        {
          toUpdate.Date = options.UpdateDefinition.Date;
        }

        if (options.UpdateDefinition.Url != null)
        {
          toUpdate.Url = options.UpdateDefinition.Url;
        }
        Console.WriteLine($"Updating...");
        var result = items.UpdateOne(toUpdate) ? ExitCodes.Success : ExitCodes.FailedToUpdate;
        if (result != ExitCodes.Success)
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine("Failed to update...");
          Console.ResetColor();
          return (int)result;
        }
        Console.WriteLine($"Update Succesfull [{toUpdate.Id} - ({toUpdate.Item})]");
        Console.ResetColor();
        return (int)result;
      }
    }
  }
}
