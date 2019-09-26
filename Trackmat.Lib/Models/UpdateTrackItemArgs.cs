using LiteDB;

namespace Trackmat.Lib.Models
{
  public class UpdateTrackItemArgs
  {
    public ObjectId UpdateId { get; set; }
    public TrackItem UpdateDefinition { get; set; }
    public bool IsReplacing { get; set; }
  }
}
