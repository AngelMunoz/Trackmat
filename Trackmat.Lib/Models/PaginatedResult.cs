using System;
using System.Collections.Generic;
using System.Text;

namespace Trackmat.Lib.Models
{
  public struct PaginatedResult<T>
  {
    public int Count { get; set; }
    public IEnumerable<T> List { get; set; }
  }
}
