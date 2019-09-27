using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trackmat.Lib.Models;

namespace Trackmat.Lib.Services.Tests
{
  [TestClass()]
  public class PeriodServiceTest
  {
    private string homepath;
    private TrackItemService _items;
    private PeriodService _periods;

    [TestInitialize()]
    public void StartUp()
    {
      homepath = Path.Combine(Path.GetTempPath(), "trackmat");
      Directory.CreateDirectory(homepath);
      _items = new TrackItemService(Path.Combine(homepath, "trackmat.db"));
      _periods = new PeriodService(Path.Combine(homepath, "trackmat.db"));
    }

    [TestCleanup()]
    public void CleanUp()
    {
      _items.Dispose();
      _periods.Dispose();
      if (Directory.Exists(homepath))
      {
        Directory.Delete(homepath, true);
      }
    }

    [TestMethod]
    public void CreateTest()
    {
      var period = _periods.Create(new Period
      {
        Name = "Sprint 5",
        EzName = "s5",
        StartDate = DateTime.Now,
        EndDate = DateTime.Now.AddDays(1),
        Items = new List<TrackItem>()
      });

      Assert.IsNotNull(period.Id);
      Assert.AreEqual(DateTime.Now.ToString("yyyy-MM-dd"), period.StartDate.ToString("yyyy-MM-dd"));
      Assert.AreEqual(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"), period.EndDate.ToString("yyyy-MM-dd"));
      Assert.AreEqual(0, period.Items.Count());
    }

    [TestMethod]
    public void FindOneTest()
    {
      var expected = _periods.Create(new Period
      {
        Name = "FindOne",
        EzName = "F1",
        StartDate = DateTime.Now,
        EndDate = DateTime.Now.AddDays(1),
        Items = new List<TrackItem>()
      });

      var actual = _periods.FindOne("F1");

      Assert.AreEqual(expected.EzName, actual.EzName);
      Assert.AreEqual(expected.Name, actual.Name);
      Assert.AreEqual(
        expected.StartDate.ToString("yyyy-MM-dd"),
        actual.StartDate.ToString("yyyy-MM-dd")
      );
      Assert.AreEqual(
        expected.EndDate.ToString("yyyy-MM-dd"),
        actual.EndDate.ToString("yyyy-MM-dd")
      );
      Assert.AreEqual(expected.Items.Count(), actual.Items.Count());
    }

    [TestMethod]
    public void ExistsTest()
    {
      _periods.Create(new Period
      {
        Name = "Exists",
        EzName = "E1",
        StartDate = DateTime.Now,
        EndDate = DateTime.Now.AddDays(1),
        Items = new List<TrackItem>()
      });

      var upperCase = _periods.Exists("E1");
      var lowerCase = _periods.Exists("e1");
      Assert.IsTrue(upperCase);
      Assert.IsTrue(lowerCase);
    }

  }
}
