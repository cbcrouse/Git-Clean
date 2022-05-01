#nullable enable
using Git.Clean.Interfaces;
using Git.Clean.Validators;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Git.Clean.Commands
{
    /// <summary>
    /// Remove branches from the repository that are considered stale.
    /// </summary>
    [Command]
    public class RemoveStaleBranches
    {
        /// <summary>
        /// The directory containing the .git folder.
        /// </summary>
        [Option(Description = "The directory containing the .git folder.")]
        [Required]
        public string GitDirectory { get; set; } = string.Empty;

        /// <summary>
        /// The git remote target. Defaults to 'origin'.
        /// </summary>
        [Option(ShortName = "t", Description = "The git remote target. Defaults to 'origin'.")]
        public string RemoteTarget { get; set; } = "origin";

        /// <summary>
        /// The number of months without commits before a branch is considered stale.
        /// </summary>
        [Option(ShortName = "m", Description = "The number of months without commits before a branch is considered stale. Defaults to 3.")]
        public int StaleMonths { get; set; } = 3;

        /// <summary>
        /// The git remote branches to ignore.
        /// </summary>
        [Option(Description = "The git remote branches to ignore. Defaults to [ \"dev\", \"develop\", \"main\", \"master\", \"release\", \"HEAD\" ] ")]
        public List<string> IgnoreBranches { get; set; } = new() { "dev", "develop", "main", "master", "release", "HEAD" };


        // ReSharper disable once UnusedMember.Local
        private void OnExecuteAsync(CommandLineApplication app)
        {
            new RemoveStaleBranchesValidator().ValidateAndThrow(this);
            var gitClean = app.GetRequiredService<IGitCleanService>();
            gitClean.RemoveStaleBranches(this);
        }
    }
}
