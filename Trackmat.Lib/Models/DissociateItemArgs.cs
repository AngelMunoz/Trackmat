using System.Collections.Generic;
using LiteDB;

namespace Trackmat.Lib.Models
{
  public class DissociateItemArgs
  {
    public string EzName { get; set; }
    public IEnumerable<string> Items { get; set; }
    public IEnumerable<ObjectId> Ids { get; set; }
  }
}