using LiteDB;

namespace Trackmat.Lib.Models
{
  public class InitConfig
  {
    [BsonId]
    public ObjectId Id { get; set; }
    public string HomeDirectory { get; set; }
  }
}
