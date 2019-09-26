using System;
using System.Collections.Generic;
using System.Linq;
using Trackmat.Lib.Enums;
using Trackmat.Lib.Models;
using Trackmat.Lib.Services;

namespace Trackmat.Runners
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

    public int Show(ShowTrackItemArgs options)
    {
      using (var items = new TrackItemService())
      {
        try
        {
          PaginatedResult<TrackItem> result;
          if (options.All)
          {
            result = items.FindAll(options.Pagination);
          }
          else if (options.ItemId != null)
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

    public int Update(UpdateTrackItemArgs options)
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

    public int Delete(DeleteTrackItemArgs options)
    {
      IEnumerable<TrackItem> byIds = new TrackItem[0];
      IEnumerable<PaginatedResult<TrackItem>> byNames = new PaginatedResult<TrackItem>[0];
      using (var service = new TrackItemService())
      {
        if (options.Ids?.Count() > 0)
        {
          byIds = service.Find(options.Ids);
        }

        if (options.Names?.Count() > 0)
        {
          byNames = options.Names.Select(name => service.Find(name, new PaginationValues { Page = 1, Limit = 10 }));
        }

        if (byIds?.Count() > 0 && !options.DeleteIds)
        {
          Console.ForegroundColor = ConsoleColor.Yellow;
          Console.WriteLine("Found the Following Items:");
          foreach (var item in byIds)
          {
            Console.WriteLine(item);
          }
          Console.WriteLine("Are you sure to continue? [Y/N]");
          var key = Console.ReadKey();
          if (key.Key == ConsoleKey.Y)
          {
            Console.WriteLine("\nDeleting...");
            var deleted = service.Delete(options.Ids).All(deled => deled);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Deleted All Items By Id...");
            Console.ResetColor();
          }
        }
        else if (byIds?.Count() > 0 && options.DeleteIds)
        {
          Console.WriteLine($"\"--all-ids\" Flag Found. Deleting {byIds.Count()} Items...");
          var deleted = service.Delete(options.Ids).All(deled => deled);
          Console.ForegroundColor = ConsoleColor.Green;
          Console.WriteLine($"Deleted All Items By Id...");
          Console.ResetColor();
        }

        if (byNames?.Count() > 0 && !options.DeleteNames)
        {
          Console.ForegroundColor = ConsoleColor.Yellow;
          Console.WriteLine("Found the Following Items By Name:");
          foreach (var result in byNames)
          {
            if (result.Count > 0)
            {
              Console.WriteLine($"[{result.List?.First()?.Item} - {result.Count}]");
              foreach (var item in result.List)
              {
                Console.WriteLine($"\t{item}");
              }
            }
          }
          Console.WriteLine("Are you sure to continue? [Y/N]");
          var key = Console.ReadKey();
          if (key.Key == ConsoleKey.Y)
          {
            Console.WriteLine("\nDeleting...");

            foreach (var name in options.Names)
            {
              var deletes = service.Delete(name);
              Console.WriteLine($"\tDeleted [{name} - {deletes}]");
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Deleted All Items By Name...");
            Console.ResetColor();
          }
          Console.ResetColor();
        }
        else if (byNames?.Count() > 0 && options.DeleteNames)
        {
          var results = byNames.Select(result =>
          {
            if (result.Count > 0)
              return (result.Count, result.List.First().Item);
            else
              return (0, null);
          });
          var total = results.Aggregate(
            "",
            (prev, next) =>
              $"{(string.IsNullOrEmpty(prev) ? $"{prev}" : $"{prev},")}[{next.Item} With {next.Count} {(next.Count == 1 ? "Item" : "Items")}]"
          );
          Console.WriteLine($"\"--all-names\" Flag Found. Deleting ({total})");
          foreach (var name in options.Names)
          {
            var deletes = service.Delete(name);
            Console.WriteLine($"\tDeleted [{name} - {deletes}]");
          }
          Console.ForegroundColor = ConsoleColor.Green;
          Console.WriteLine($"Deleted All Items By Name...");
          Console.ResetColor();
        }
      }
      return (int)ExitCodes.Success;
    }
  }
}
