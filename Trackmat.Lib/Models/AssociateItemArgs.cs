using System;
using System.Collections.Generic;
using System.Text;

namespace Trackmat.Lib.Models
{
  public class AssociateItemArgs
  {
    public string EzName { get; set; }
    public IEnumerable<string> Items { get; set; }
  }
}
