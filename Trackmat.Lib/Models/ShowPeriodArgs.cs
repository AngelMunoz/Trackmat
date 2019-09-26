namespace Trackmat.Lib.Models
{
  public class ShowPeriodArgs
  {
    public string EzName { get; set; }
    public bool Detailed { get; set; }
    public bool All { get; set; }
    public PaginationValues Pagination { get; set; }
  }
}
