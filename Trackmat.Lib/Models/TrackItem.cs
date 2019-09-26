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

    public override string ToString()
    {
      return $"[{Id} - {Item}]: {Time}h - {Date.ToShortDateString()}{(Url != null ? $" - {Url}" : "")}";
    }
  }
}
