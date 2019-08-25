using System.Collections.Generic;

namespace Trackmat.Lib.Models
{
  public struct PaginatedResult<T>
  {
    public int Count { get; set; }
    public IEnumerable<T> List { get; set; }
  }
}
