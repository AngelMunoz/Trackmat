using System;
using LiteDB;
using Trackmat.Lib.Enums;
using Trackmat.Lib.Interfaces;
using Trackmat.Lib.Models;

namespace Trackmat.Lib.Runners
{
  public class ItemRunner : IRunnable
  {
    public int Create<T>(T target)
    {
      var item = target as TrackItem;
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine(item);
      Console.ResetColor();
      return (int)ExitCodes.Success;
    }

    public int Delete<T>(T target)
    {
      throw new NotImplementedException();
    }

    public int Find(ObjectId id, string key = null)
    {
      throw new NotImplementedException();
    }

    public int Update<K, T>(K identifier, T target)
    {
      throw new NotImplementedException();
    }
  }
}
