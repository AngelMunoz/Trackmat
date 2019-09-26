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
      var homedir = Environment.GetEnvironmentVariable("TRACKMAT_HOME", EnvironmentVariableTarget.User) ?? Environment.GetEnvironmentVariable("TRACKMAT_HOME");
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

    public PaginatedResult<Period> FindAll(PaginationValues pagination)
    {
      var skip = (pagination.Page - 1) * pagination.Limit;
      var periods = Periods.Find(Query.All(), skip, pagination.Limit);
      var count = Periods.Count(Query.All());
      return new PaginatedResult<Period> { List = periods, Count = count };
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

    public bool Delete(ObjectId id)
    {
      return Periods.Delete(id);
    }

    #region IDisposable Support
    private bool disposedValue = false; // To detect redundant calls

    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
        {
          Db.Dispose();
        }
        disposedValue = true;
      }
    }

    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
      Dispose(true);
    }
    #endregion
  }
}
