using LiteDB;

namespace Trackmat.Lib.Interfaces
{
  public interface IRunnable<T>
  {
    int Find(ObjectId id, string key = null);
    int Create(T target);
    int Update(string name, T target);
    int Update(ObjectId id, T target);
    int Delete(T target);
  }
}
