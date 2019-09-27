using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Trackmat.Cli.Extensions
{
  public static class DatabasePath
  {
    public static string GetDatabasePath(this string dbname)
    {
      return Path.Combine(Environment.GetEnvironmentVariable("TRACKMAT_HOME", EnvironmentVariableTarget.User) ?? Environment.GetEnvironmentVariable("TRACKMAT_HOME"), dbname);
    }
  }
}
