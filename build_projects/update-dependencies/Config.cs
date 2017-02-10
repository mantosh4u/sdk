﻿// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.DotNet.Scripts
{
    /// <summary>
    /// Holds the configuration information for the update-dependencies script.
    /// </summary>
    /// <remarks>
    /// The following Environment Variables are required by this script:
    /// 
    /// GITHUB_USER - The user to commit the changes as.
    /// GITHUB_EMAIL - The user's email to commit the changes as.
    /// GITHUB_PASSWORD - The password/personal access token of the GitHub user.
    ///
    /// The following Environment Variables can optionally be specified:
    /// 
    /// ROSLYN_VERSION_URL - The Url to get the current Roslyn version. (ex. "https://raw.githubusercontent.com/dotnet/versions/master/build-info/dotnet/roslyn/netcore1.0")
    /// CORESETUP_VERSION_URL - The Url to get the current dotnet/core-setup package versions. (ex. "https://raw.githubusercontent.com/dotnet/versions/master/build-info/dotnet/core-setup/master")
    /// GITHUB_ORIGIN_OWNER - The owner of the GitHub fork to push the commit and create the PR from. (ex. "dotnet-bot")
    /// GITHUB_UPSTREAM_OWNER - The owner of the GitHub base repo to create the PR to. (ex. "dotnet")
    /// GITHUB_PROJECT - The repo name under the ORIGIN and UPSTREAM owners. (ex. "cli")
    /// GITHUB_UPSTREAM_BRANCH - The branch in the GitHub base repo to create the PR to. (ex. "master")
    /// GITHUB_PULL_REQUEST_NOTIFICATIONS - A semi-colon ';' separated list of GitHub users to notify on the PR.
    /// </remarks>
    public class Config
    {
        public static Config Instance { get; } = new Config();

        private Lazy<string> _userName = new Lazy<string>(() => GetEnvironmentVariable("GITHUB_USER"));
        private Lazy<string> _email = new Lazy<string>(() => GetEnvironmentVariable("GITHUB_EMAIL"));
        private Lazy<string> _password = new Lazy<string>(() => GetEnvironmentVariable("GITHUB_PASSWORD"));
        
        private Lazy<string> _roslynVersionUrl = new Lazy<string>(() => GetEnvironmentVariable("ROSLYN_VERSION_URL", "https://raw.githubusercontent.com/dotnet/versions/master/build-info/dotnet/roslyn/netcore1.0"));
        private Lazy<string> _coreSetupVersionUrl = new Lazy<string>(() => GetEnvironmentVariable("CORESETUP_VERSION_URL", "https://raw.githubusercontent.com/dotnet/versions/master/build-info/dotnet/core-setup/master"));
        private Lazy<string> _gitHubUpstreamOwner = new Lazy<string>(() => GetEnvironmentVariable("GITHUB_UPSTREAM_OWNER", "dotnet"));
        private Lazy<string> _gitHubProject = new Lazy<string>(() => GetEnvironmentVariable("GITHUB_PROJECT", "cli"));
        private Lazy<string> _gitHubUpstreamBranch = new Lazy<string>(() => GetEnvironmentVariable("GITHUB_UPSTREAM_BRANCH", "master"));
        private Lazy<string[]> _gitHubPullRequestNotifications = new Lazy<string[]>(() => 
                                                GetEnvironmentVariable("GITHUB_PULL_REQUEST_NOTIFICATIONS", "")
                                                    .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));

        private Config()
        {
        }

        public string UserName => _userName.Value;
        public string Email => _email.Value; 
        public string Password => _password.Value;
        public string RoslynVersionUrl => _roslynVersionUrl.Value;
        public string CoreSetupVersionUrl => _coreSetupVersionUrl.Value;
        public string GitHubUpstreamOwner => _gitHubUpstreamOwner.Value;
        public string GitHubProject => _gitHubProject.Value;
        public string GitHubUpstreamBranch => _gitHubUpstreamBranch.Value;
        public string[] GitHubPullRequestNotifications => _gitHubPullRequestNotifications.Value;

        private static string GetEnvironmentVariable(string name, string defaultValue = null)
        {
            string value = Environment.GetEnvironmentVariable(name);
            if (value == null)
            {
                value = defaultValue;
            }

            if (value == null)
            {
                throw new InvalidOperationException($"Can't find environment variable '{name}'.");
            }

            return value;
        }
    }
}
