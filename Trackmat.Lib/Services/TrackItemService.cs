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
    private LiteDatabase db { get; set; }
    private LiteCollection<TrackItem> trackitems { get; set; }

    public TrackItemService(LiteDatabase db = null)
    {
      var homedir = Environment.GetEnvironmentVariable("TRACKMAT_HOME", EnvironmentVariableTarget.User);
      this.db = db ?? new LiteDatabase(Path.Combine(homedir, "trackmat.db"));
      trackitems = this.db.GetCollection<TrackItem>();
    }

    public TrackItem Create(TrackItem item)
    {
      try
      {
        var createdId = trackitems.Insert(item);
        return trackitems.FindById(createdId);
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
        return trackitems.FindById(id);
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
      var count = trackitems.Count(item => item.Item == name);
      var items = trackitems.Find(item => item.Item == name, skip, pagination.Limit);
      return new PaginatedResult<TrackItem>
      {
        Count = count,
        List = items
      };
    }

    public IEnumerable<TrackItem> Find(IEnumerable<ObjectId> ids)
    {
      return ids.Select(id => trackitems.FindById(id));
    }

    public int Delete(string name)
    {
      return trackitems.Delete(item => item.Item == name);
    }

    public bool Delete(ObjectId id)
    {
      return trackitems.Delete(id);
    }

    public IEnumerable<bool> Delete(IEnumerable<ObjectId> ids)
    {
      return ids.Select(id => trackitems.Delete(id));
    }

    public IEnumerable<int> Delete(IEnumerable<string> names)
    {
      return names.Select(name => trackitemss.Delete(item => item.Item == name));
    }

    public bool UpdateOne(TrackItem toUpdate)
    {
      return trackitems.Update(toUpdate);
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
          db.Dispose();
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
