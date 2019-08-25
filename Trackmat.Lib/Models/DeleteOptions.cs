using System.Collections.Generic;
using LiteDB;

namespace Trackmat.Lib.Models
{
  public class DeleteOptions
  {
    public IEnumerable<ObjectId> Ids { get; set; }
    public IEnumerable<string> Names { get; set; }
    public bool DeleteIds { get; set; }
    public bool DeleteNames { get; set; }
  }
}
