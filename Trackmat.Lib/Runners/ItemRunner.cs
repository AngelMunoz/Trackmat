using System;
using LiteDB;
using Trackmat.Lib.Enums;
using Trackmat.Lib.Interfaces;
using Trackmat.Lib.Models;
using Trackmat.Lib.Services;

namespace Trackmat.Lib.Runners
{
  public class ItemRunner : IRunnable<TrackItem>
  {
    protected TrackItemService _items { get; private set; } = new TrackItemService();


    public int Create(TrackItem target)
    {
      try
      {
        var created = _items.Create(target);
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

    public int Delete(TrackItem target)
    {
      throw new NotImplementedException();
    }

    public int Find(ObjectId id, string key = null)
    {
      throw new NotImplementedException();
    }

    public int Update(string name, TrackItem target)
    {
      throw new NotImplementedException();
    }

    public int Update(ObjectId id, TrackItem target)
    {
      throw new NotImplementedException();
    }
  }
}
