using System;
using System.Diagnostics;
using System.IO;
using LiteDB;
using Trackmat.Lib.Models;

namespace Trackmat.Lib.Services
{
  public class PeriodService : IDisposable
  {

    private LiteDatabase Db { get; set; }
    private LiteCollection<Period> Periods { get; set; }

    public PeriodService(LiteDatabase db = null)
    {
      var homedir = Environment.GetEnvironmentVariable("TRACKMAT_HOME", EnvironmentVariableTarget.User);
      Db = db ?? new LiteDatabase(Path.Combine(homedir, "trackmat.db"));
      Periods = Db.GetCollection<Period>();

      Periods.EnsureIndex(period => period.EzName, true);
      Periods.EnsureIndex(period => period.Name);

      Periods.EnsureIndex(period => period.StartDate);
      Periods.EnsureIndex(period => period.EndDate);
    }

    public Period Create(Period period)
    {
      try
      {
        var periodId = Periods.Insert(period);
        return Periods.FindById(periodId);
      }
      catch (Exception e)
      {
        Debug.WriteLine(e.Message, "[Trackmat:Services:PeriodService]");
        throw;
      }
    }

    public Period FindOne(ObjectId id)
    {
      try
      {
        return Periods.FindById(id);
      }
      catch (Exception e)
      {
        Debug.WriteLine(e.Message, "[Trackmat:Services:PeriodService]");
        throw;
      }
    }
    public Period FindOne(string ezName)
    {
      try
      {
        return Periods.FindOne(period => period.EzName.ToLowerInvariant() == ezName.ToLowerInvariant());
      }
      catch (Exception e)
      {
        Debug.WriteLine(e.Message, "[Trackmat:Services:PeriodService]");
        throw;
      }
    }

    public bool UpdateOne(Period toUpdate)
    {
      return Periods.Update(toUpdate);
    }

    public bool Exists(string ezName)
    {
      try
      {
        return Periods.Exists(period => period.EzName.ToLowerInvariant() == ezName.ToLowerInvariant());

      }
      catch (Exception e)
      {
        Debug.WriteLine(e.Message, "[Trackmat:Services:PeriodService]");
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
          Db.Dispose();
        }

        // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
        // TODO: set large fields to null.

        disposedValue = true;
      }
    }

    // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
    // ~PeriodService()
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
