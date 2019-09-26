using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using LiteDB;
using Trackmat.Lib.Models;

namespace Trackmat.Lib.Services
{
  public class TrackItemService : IDisposable
  {
    private LiteDatabase Db { get; set; }
    private LiteCollection<TrackItem> TrackItems { get; set; }

    public TrackItemService(LiteDatabase db = null)
    {
      var homedir = Environment.GetEnvironmentVariable("TRACKMAT_HOME", EnvironmentVariableTarget.User);
      Db = db ?? new LiteDatabase(Path.Combine(homedir, "trackmat.db"));
      TrackItems = Db.GetCollection<TrackItem>();
      TrackItems.EnsureIndex(item => item.Item);
      TrackItems.EnsureIndex(item => item.Date);
    }

    public TrackItem Create(TrackItem item)
    {
      try
      {
        var createdId = TrackItems.Insert(item);
        return TrackItems.FindById(createdId);
      }
      catch (Exception e)
      {
        Debug.WriteLine(e.Message, "[Trackmat:Services:TrackItemService]");
        throw;
      }
    }

    public TrackItem FindOne(ObjectId id)
    {
      try
      {
        return TrackItems.FindById(id);
      }
      catch (Exception e)
      {
        Debug.WriteLine(e.Message, "[Trackmat:Services:TrackItemService]");
        throw;
      }
    }

    public PaginatedResult<TrackItem> Find(string name, PaginationValues pagination)
    {
      var skip = (pagination.Page - 1) * pagination.Limit;
      var count = TrackItems.Count(item => item.Item == name);
      var items = TrackItems.Find(item => item.Item == name, skip, pagination.Limit);
      return new PaginatedResult<TrackItem>
      {
        Count = count,
        List = items
      };
    }

    public IEnumerable<TrackItem> Find(IEnumerable<string> names)
    {
      return TrackItems.Find(item => names.Contains(item.Item));
    }

    public IEnumerable<TrackItem> Find(IEnumerable<ObjectId> ids)
    {
      return ids.Select(id => TrackItems.FindById(id));
    }

    public int Delete(string name)
    {
      return TrackItems.Delete(item => item.Item == name);
    }

    public bool Delete(ObjectId id)
    {
      return TrackItems.Delete(id);
    }

    public IEnumerable<bool> Delete(IEnumerable<ObjectId> ids)
    {
      return ids.Select(id => TrackItems.Delete(id));
    }

    public IEnumerable<int> Delete(IEnumerable<string> names)
    {
      return names.Select(name => TrackItems.Delete(item => item.Item == name));
    }

    public bool UpdateOne(TrackItem toUpdate)
    {
      return TrackItems.Update(toUpdate);
    }

    #region IDisposable Support
    private bool disposedValue = false; // To detect redundant calls

    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
        {
          // TODO: dispose managed state (managed objects).
          Db.Dispose();
        }

        // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
        // TODO: set large fields to null.

        disposedValue = true;
      }
    }

    // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
    // ~TrackItemService()
    // {
    //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
    //   Dispose(false);
    // }

    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
      // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
      Dispose(true);
      // TODO: uncomment the following line if the finalizer is overridden above.
      // GC.SuppressFinalize(this);
    }
    #endregion
  }

}
