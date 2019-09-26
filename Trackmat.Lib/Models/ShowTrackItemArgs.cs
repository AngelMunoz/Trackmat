using LiteDB;

namespace Trackmat.Lib.Models
{
  public class ShowTrackItemArgs
  {
    public string Name { get; set; }
    public PaginationValues Pagination { get; set; }
    public ObjectId ItemId { get; set; }
    public bool Detailed { get; set; }
  }
}
