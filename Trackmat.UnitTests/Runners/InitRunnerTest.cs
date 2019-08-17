using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trackmat.Lib.Enums;
using Trackmat.Lib.Models;
using Trackmat.Lib.Runners;

namespace Trackmat.UnitTests.Runners
{
  [TestClass]
  public class InitRunnerTest
  {
    private string homepath;

    [TestInitialize()]
    public void StartUp()
    {
      Environment.SetEnvironmentVariable("TRACKMAT_HOME", null, EnvironmentVariableTarget.User);
      homepath = Path.Combine(Path.GetTempPath(), "trackmat");
      if (Directory.Exists(homepath))
      {
        Directory.Delete(homepath, true);
      }
    }

    [TestCleanup()]
    public void CleanUp()
    {
      Environment.SetEnvironmentVariable("TRACKMAT_HOME", null, EnvironmentVariableTarget.User);
      if (Directory.Exists(homepath))
      {
        Directory.Delete(homepath, true);
      }
    }

    [TestMethod]
    public void Should_Be_Not_Configured_By_Env_Var()
    {
      Assert.IsFalse(InitRunner.IsConfigured());
    }

    [TestMethod]
    public void Should_Be_Not_Configured_By_Dir()
    {
      Environment.SetEnvironmentVariable("TRACKMAT_HOME", homepath, EnvironmentVariableTarget.User);
      Assert.IsFalse(InitRunner.IsConfigured());
    }

    [TestMethod]
    public void Should_Be_Configured()
    {
      Environment.SetEnvironmentVariable("TRACKMAT_HOME", homepath, EnvironmentVariableTarget.User);
      Directory.CreateDirectory(homepath);
      Assert.IsTrue(InitRunner.IsConfigured());
    }

    [TestMethod]
    public void Init_Should_Not_Fail()
    {
      var runner = new InitRunner();
      var initConfig = new InitConfig
      {
        HomeDirectory = homepath
      };
      var result = runner.Init(initConfig);
      var configExists = File.Exists(Path.Combine(homepath, "config.db"));
      var trackmatExists = File.Exists(Path.Combine(homepath, "trackmat.db"));
      Assert.AreEqual((int)ExitCodes.Success, result);
      Assert.IsTrue(configExists);
      Assert.IsTrue(trackmatExists);
    }

    [TestMethod]
    public void Init_Should_Fail()
    {
      var runner = new InitRunner();
      var initConfig = new InitConfig
      {
        HomeDirectory = "$$$$%%%%^^^^^&&&&&****(((())))___@#$%"
      };
      var result = runner.Init(initConfig);
      var configExists = File.Exists(Path.Combine(initConfig.HomeDirectory, "config.db"));
      var trackmatExists = File.Exists(Path.Combine(initConfig.HomeDirectory, "trackmat.db"));
      Assert.AreEqual((int)ExitCodes.FailedInitAtDirectory, result);
      Assert.IsFalse(configExists);
      Assert.IsFalse(trackmatExists);
    }
  }
}
