using Git.Clean.Commands;

namespace Git.Clean.Interfaces
{
    /// <summary>
    /// Provides a testable abstraction for git clean commands.
    /// </summary>
    public interface IGitCleanService
    {
        /// <summary>
        /// Removes branches considered to be stale.
        /// </summary>
        /// <param name="command">The command object.</param>
        void RemoveStaleBranches(RemoveStaleBranches command);
    }
}
