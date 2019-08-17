using LiteDB;

namespace Trackmat.Lib.Interfaces
{
  public interface IRunnable
  {
    int Find(ObjectId id, string key = null);
    int Create<T>(T target);
    int Update<K, T>(K identifier, T target);
    int Delete<T>(T target);
  }
}
