﻿using System;
using System.Diagnostics;
using System.IO;
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

    public TrackItem FindOne(string name)
    {
      try
      {
        return trackitems.FindOne(item => item.Item == name);
      }
      catch (Exception e)
      {
        Debug.WriteLine(e.Message, "[Trackmat:Services:TrackItemService]");
        throw;
      }
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