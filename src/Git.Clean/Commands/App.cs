using McMaster.Extensions.CommandLineUtils;

namespace Git.Clean.Commands
{
    /// <summary>
    /// The git clean commandline application.
    /// </summary>
    [Command("git-clean")]
    [Subcommand(typeof(RemoveStaleBranches))]
    public class App
    {
        // ReSharper disable once UnusedMember.Local
        private int OnExecuteAsync(CommandLineApplication app)
        {
            app.ShowHelp();
            return 0;
        }
    }
}