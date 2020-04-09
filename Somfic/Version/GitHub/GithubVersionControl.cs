using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Somfic.HTTP;
using Somfic.Logging;
using Somfic.Version;

namespace Somfic.Version.Github
{
    public class GithubVersionControl : VersionController
    {

        private readonly string user;
        private readonly string repo;
        private readonly bool usePre;

        public GithubVersionControl(string userName, string repoName, bool countPreReleases = false)
        {
            user = userName;
            repo = repoName;
            usePre = countPreReleases;
        }

        public override System.Version GetVersion()
        {
            Exception ex = null;

            Logger.Verbose($"Checking GitHub repository '{user}/{repo}' for a newer version.");
            GetResult userRepos = new GetRequest($"https://api.github.com/repos/{user}/{repo}/releases/latest").Execute();

            if (userRepos == null)
            {
                Logger.Verbose("Could not get a response from GitHub.");
                return null;
            }

            if (userRepos.Response != null && userRepos.Response.StatusCode != HttpStatusCode.OK)
            {
                Logger.Verbose("Could not get a response from GitHub.", userRepos.Exception);
                return null;
            }

            if (string.IsNullOrWhiteSpace(userRepos.Content))
            {
                Logger.Verbose("Could not get latest version from GitHub.", new Exception($"The repository '{user}/{repo}' could not be found.", userRepos.Exception));
                return null;
            }

            ReleaseResponse latestRelease = JsonConvert.DeserializeObject<ReleaseResponse>(userRepos.Content);

            string version = Regex.Match(latestRelease.TagName, @"([0-9]+\.*[0-9]*\.*[0-9]*\.*[0-9]*)").Groups[1].Value;

            return System.Version.Parse(version);
        }
    }
}

