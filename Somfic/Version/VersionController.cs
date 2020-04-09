using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Somfic.Logging;

namespace Somfic.Version
{
    public abstract class VersionController
    {
        public abstract System.Version GetLatest();

        public bool HasNewer()
        {
            System.Version latestVersion = GetLatest();
            System.Version thisVersion = Assembly.GetCallingAssembly().GetName().Version;
            return latestVersion > thisVersion;
        }
    }
}
