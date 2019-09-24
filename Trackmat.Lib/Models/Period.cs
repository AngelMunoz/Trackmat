using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;

namespace Trackmat.Lib.Models
{
  public class Period
  {
    [BsonId]
    public ObjectId Id { get; set; }
    public IEnumerable<TrackItem> Items { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Name { get; set; }
    public string EzName { get; set; }

    public override string ToString()
    {
      return $"[{Name} ({EzName})]: From {StartDate.ToShortDateString()} To {EndDate.ToShortDateString()} With {Items?.Count()} {(Items.Count() == 1 ? "Item" : "Items")}";
    }
  }
}
