using System;
using System.IO;
using System.Linq;
using LiteDB;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trackmat.Lib.Models;

namespace Trackmat.Lib.Services.Tests
{
  [TestClass()]
  public class TrackItemServiceTest
  {
    private string homepath;
    private readonly LiteDatabase _db;
    private TrackItemService _items;

    [TestInitialize()]
    public void StartUp()
    {
      homepath = Path.Combine(Path.GetTempPath(), "trackmat");
      Directory.CreateDirectory(homepath);
      _items = new TrackItemService(Path.Combine(homepath, "trackmat.db"));
    }

    [TestCleanup()]
    public void CleanUp()
    {
      _items.Dispose();
      if (Directory.Exists(homepath))
      {
        Directory.Delete(homepath, true);
      }
    }

    [TestMethod]
    public void CreateTest()
    {
      var item = _items.Create(new TrackItem
      {
        Item = "SMPL-001",
        Time = 0.5f,
        Date = DateTime.Now
      });

      Assert.IsNotNull(item.Id);
      Assert.AreEqual(DateTime.Now.ToString("yyyy-MM-dd"), item.Date.ToString("yyyy-MM-dd"));
      Assert.AreEqual("SMPL-001", item.Item);
      Assert.AreEqual(0.5f, item.Time);
      Assert.IsNull(item.Url);
    }

    [TestMethod]
    public void FindOneTest()
    {
      var item = _items.Create(new TrackItem
      {
        Item = "SMPL-001",
        Time = 0.5f,
        Date = DateTime.Now,
        Url = "https://localhost/browse/SMPL-001"
      });

      var found = _items.FindOne(item.Id);

      Assert.IsNotNull(found.Id);
      Assert.AreEqual(item.Date.ToString("yyyy-MM-dd"), found.Date.ToString("yyyy-MM-dd"));
      Assert.AreEqual(item.Item, found.Item);
      Assert.AreEqual(item.Time, found.Time);
      Assert.AreEqual(item.Url, found.Url);
    }

    [TestMethod]
    public void FindTest()
    {
      var result = _items.Find("", new PaginationValues { Page = 1, Limit = 10 });
      Assert.IsTrue(result.List.Count() == 0);
      Assert.AreEqual(0, result.Count);

      _items.Create(new TrackItem
      {
        Item = "IM-UNIQ",
        Time = 0.5f,
        Date = DateTime.Now,
        Url = "https://localhost/browse/SMPL-001"
      });
      var uniq = _items.Find("IM-UNIQ", new PaginationValues { Page = 1, Limit = 10 });
      Assert.IsTrue(uniq.List.Count() == 1);
      Assert.AreEqual(1, uniq.Count);
    }
  }
}
