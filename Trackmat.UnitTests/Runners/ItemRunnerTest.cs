using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trackmat.Lib.Models;
using Trackmat.Lib.Runners;
using Trackmat.Lib.Services;

namespace Trackmat.UnitTests.Runners
{
  [TestClass]
  public class ItemRunnerTest
  {
    private string homepath;
    private TrackItemService _items;

    [TestInitialize()]
    public void StartUp()
    {
      homepath = Path.Combine(Path.GetTempPath(), "trackmat");
      Environment.SetEnvironmentVariable("TRACKMAT_HOME", homepath, EnvironmentVariableTarget.User);
      if (Directory.Exists(homepath))
      {
        Directory.Delete(homepath, true);
      }
      var initConfig = new InitConfig
      {
        HomeDirectory = homepath
      };
      var runner = new InitRunner();
      runner.Init(initConfig);
      _items = new TrackItemService();
    }

    [TestCleanup()]
    public void CleanUp()
    {
      _items.Dispose();
      Environment.SetEnvironmentVariable("TRACKMAT_HOME", null, EnvironmentVariableTarget.User);
      if (Directory.Exists(homepath))
      {
        Directory.Delete(homepath, true);
      }
    }

    [TestMethod]
    public void Should_Create_Item()
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
    public void Should_Find_Item_By_Id()
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
  }
}
