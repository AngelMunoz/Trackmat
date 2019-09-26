using System;
using System.Collections.Generic;
using System.Text;

namespace Trackmat.Lib.Models
{
  public class UpdatePeriodArgs
  {
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Name { get; set; }
    public string EzName { get; set; }
  }
}
