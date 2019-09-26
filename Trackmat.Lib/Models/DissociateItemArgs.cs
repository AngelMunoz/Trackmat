using System.Collections.Generic;

namespace Trackmat.Lib.Models
{
  public class DissociateItemArgs
  {
    public string EzName { get; set; }
    public IEnumerable<string> Items { get; set; }
  }
}