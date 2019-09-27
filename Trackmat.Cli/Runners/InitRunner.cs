using System;
using System.IO;
using Trackmat.Cli.Extensions;
using Trackmat.Lib.Enums;
using Trackmat.Lib.Models;
using Trackmat.Lib.Services;

namespace Trackmat.Runners
{
  public class InitRunner
  {
    private string ConnString
    {
      get => $"Filename={"trackmat.db".GetDatabasePath()};Async=true";
    }

    public static bool IsConfigured()
    {
      var homedir = Environment.GetEnvironmentVariable("TRACKMAT_HOME", EnvironmentVariableTarget.User) ?? Environment.GetEnvironmentVariable("TRACKMAT_HOME");
      return !string.IsNullOrEmpty(homedir) && Directory.Exists(homedir);
    }

    public int Init(InitConfig opts)
    {
      if (!opts.HomeDirectory.EndsWith("trackmat"))
      {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("WARNING: the homedir provided doesn't end in \"trackmat\" appending \"trackmat\" to it");
        opts.HomeDirectory = Path.Combine(opts.HomeDirectory, "trackmat");
        Console.ResetColor();
      }
      var appDataResult = CreateAppData(opts.HomeDirectory);
      if (appDataResult != 0) { return appDataResult; }

      Console.WriteLine($"Succesfully Created Home Directory {opts.HomeDirectory}");
      Console.WriteLine();
      Console.WriteLine();
      try
      {
        Environment.SetEnvironmentVariable("TRACKMAT_HOME", opts.HomeDirectory, EnvironmentVariableTarget.User);
        Console.WriteLine($"Succesfully Created Env Var \"TRACKMAT_HOME\" with Value: \"{opts.HomeDirectory}\"");
        Console.WriteLine();
        Console.WriteLine();
      }
      catch (Exception e)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Failed to create Env Var \"TRACKMAT_HOME\" with Value: \"{opts.HomeDirectory}\". {e.Message}");
        return (int)ExitCodes.FailedInitAtDirectory;
      }
      var createDBResult = CreateDatabase(opts.HomeDirectory);
      if (createDBResult != 0) { return createDBResult; }
      Console.WriteLine();
      Console.WriteLine();
      Console.ResetColor();
      return (int)ExitCodes.Success;
    }

    protected int CreateAppData(string homeDir)
    {
      if (Directory.Exists(homeDir))
      {
        Console.ForegroundColor = ConsoleColor.Yellow;
        while (true)
        {
          Console.WriteLine($"A Configuration directory exists at \"{homeDir}\"\nAre you sure to continue? [Y/N]");
          var key = Console.ReadKey();
          if (key.Key == ConsoleKey.N)
          {
            Console.ResetColor();
            return (int)ExitCodes.FailedInitAtDirectory; ;
          }
          else if (key.Key == ConsoleKey.Y)
          {
            Console.ResetColor();
            break;
          }
        }
      }
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine($"\nCreating Directory at {homeDir}");
      return CreateDir(homeDir, Directory.Exists(homeDir));
    }

    protected int CreateDir(string path, bool exists)
    {
      if (exists)
      {
        try
        {
          Directory.Delete(path, true);
        }
        catch (Exception e)
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine($"Failed to re-create directory at {path}. {e.Message}");
          return (int)ExitCodes.FailedInitAtDirectory;
        }
      }

      try
      {
        Directory.CreateDirectory(path);
      }
      catch (Exception e)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Failed to create directory at {path}. {e.Message}");
        return (int)ExitCodes.FailedInitAtDirectory;
      }
      return (int)ExitCodes.Success;
    }

    protected int CreateDatabase(string path)
    {
      try
      {
        using var items = new TrackItemService(ConnString);
        using var periods = new PeriodService(ConnString);

        var itemCreated = items.Create(new TrackItem
        {
          Item = "SMPL-001",
          Time = 0.5f,
          Date = DateTime.Now
        });
        var periodCreated = periods.Create(new Period
        {
          Name = "Sample Period",
          EzName = "sample",
          StartDate = DateTime.Now,
          EndDate = DateTime.Now.AddDays(1),
          Items = new TrackItem[] { itemCreated }
        });

        Console.WriteLine($"Succesfully Created Item and Period Databases \"{Path.Combine(path, "trackmat.db")}\"");
        Console.WriteLine(itemCreated);
        Console.WriteLine(periodCreated);
        Console.ResetColor();
      }
      catch (Exception e)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Failed to create config database at {path}. {e.Message}");
        return (int)ExitCodes.FailedInitAtDatabase;
      }
      return (int)ExitCodes.Success;
    }
  }
}
