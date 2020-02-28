using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Somfic.Logging;

namespace Somfic.Version
{
    public abstract class VersionController
    {
        public abstract System.Version GetLatestVersion();
    }

    public static class VersionControl
    {
        public static bool NewerVersionAvailable(VersionController versionController)
        {
            System.Version latestVersion = versionController.GetLatestVersion();
            System.Version thisVersion = Assembly.GetCallingAssembly().GetName().Version;
            return  latestVersion > thisVersion;
        }
    }
}
