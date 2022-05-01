using Git.Clean.Commands;
using Git.Clean.Interfaces;
using LibGit2Sharp;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Git.Clean.Services
{
    /// <summary>
    /// A git clean service that implements LibGit2Sharp.
    /// </summary>
    public class LibGitCleanService : IGitCleanService
    {
        private Repository _repository = new ();

        private UsernamePasswordCredentials? _credentials = new();

        /// <summary>
        /// Removes branches considered to be stale.
        /// </summary>
        /// <param name="command">The command object.</param>
        public void RemoveStaleBranches(RemoveStaleBranches command)
        {
            _repository = new Repository(command.GitDirectory);

            var branches = _repository.Branches;
            Console.WriteLine($"Searching through '{branches.Count()}' branches...");

            var branchesToRemove = branches.Where(x =>
                IsSafeToDelete(x.CanonicalName, command.IgnoreBranches, command.RemoteTarget) &&
                IsStaleBranch(x.CanonicalName, command.StaleMonths)).ToList();

            foreach (Branch repoBranch in branchesToRemove)
            {
                Console.WriteLine(repoBranch.CanonicalName);
            }

            var userWishesToContinue = Prompt.GetYesNo($"Found {branchesToRemove.Count} stale branches to delete. This action is destructive and cannot be reversed, do you wish to continue?", false, ConsoleColor.Red);

            if (!userWishesToContinue)
            {
                return;
            }

            _credentials = GetCredentials();

            foreach (Branch repoBranch in branchesToRemove)
            {
                try
                {
                    DeleteBranch(repoBranch.UpstreamBranchCanonicalName, command.RemoteTarget);
                }
                catch (Exception e)
                {
                    ConsoleColor ogColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ForegroundColor = ogColor;
                }
            }
        }

        private UsernamePasswordCredentials GetCredentials()
        {
            var password = Prompt.GetPassword("Access Token:");
            var signature = _repository.Config.BuildSignature(DateTimeOffset.UtcNow);
            return new UsernamePasswordCredentials { Password = password, Username = signature.Email };
        }

        private void DeleteBranch(string upstreamCanonicalName, string remoteTarget)
        {
            var pushOptions = new PushOptions { CredentialsProvider = (_, _, _) => _credentials };
            Remote remote = _repository.Network.Remotes[remoteTarget];
            Console.WriteLine("Deleting..." + upstreamCanonicalName);
            _repository.Network.Push(remote, $"+:{upstreamCanonicalName}", pushOptions);
        }

        private bool IsSafeToDelete(string refSpec, IEnumerable<string> ignoreBranches, string remoteTarget)
        {
            return !IsProtectedBranch(refSpec, ignoreBranches, remoteTarget) && IsRemoteBranch(refSpec);
        }

        private bool IsProtectedBranch(string refSpec, IEnumerable<string> ignoreBranches, string remoteTarget)
        {
            return ignoreBranches.Any(x =>
            {
                var pattern = @"^refs/remotes/" + $"{remoteTarget}/.*{x}.*$";
                var matched = Regex.Match(refSpec, pattern).Success;
                if (matched)
                {
                    Console.WriteLine($"Excluding Branch: {refSpec}");
                }
                return matched;
            });
        }

        private bool IsRemoteBranch(string refSpec)
        {
            return refSpec.ToLower().StartsWith("refs/remotes");
        }

        private bool IsStaleBranch(string refSpec, int staleMonths)
        {
            var commits = _repository.Branches[refSpec].Commits;
            DateTimeOffset staleThreshold = DateTimeOffset.UtcNow.AddMonths(-staleMonths);

            // If there are no commits between now and the stale month, should return true.
            return !commits.Any(x => x.Author.When > staleThreshold);
        }
    }
}
