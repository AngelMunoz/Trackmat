using System;
using LiteDB;

namespace Trackmat.Lib.Models
{
  public class TrackItem
  {
    [BsonId]
    public ObjectId Id { get; set; }
    public string Item { get; set; }
    public float Time { get; set; }
    public DateTime Date { get; set; }
    public string Url { get; set; }
    [BsonRef("periods")]
    public Period Period { get; set; }

    public override string ToString()
    {
      return $"[{Item}]: {Time}h - {Date.ToString("dddd dd MMMM yyyy")}{(Url != null ? $" - {Url}" : "")}";
    }
  }
}
