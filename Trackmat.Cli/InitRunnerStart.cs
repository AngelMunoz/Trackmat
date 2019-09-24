using System;
using System.Collections.Generic;
using System.Text;
using Trackmat.Cli.Options;

namespace Trackmat.Cli
{
  public static class InitRunnerStart
  {
    public static Func<InitOptions, int> RunInit()
    {
      return (InitOptions opts) => InitOptions.Run(opts);
    }
  }
}
