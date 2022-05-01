using FluentValidation;
using Git.Clean.Commands;
using System.IO;

namespace Git.Clean.Validators
{
    /// <summary>
    /// Provides the business rules for the <see cref="RemoveStaleBranches"/> command.
    /// </summary>
    public class RemoveStaleBranchesValidator : AbstractValidator<RemoveStaleBranches>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public RemoveStaleBranchesValidator()
        {
            RuleFor(x => x.GitDirectory).Must(Directory.Exists).WithMessage("Must be a valid directory.");
            RuleFor(x => x.GitDirectory).Must(x => Directory.Exists(Path.Join(x, ".git"))).WithMessage("Must be a valid .git directory.");
            RuleFor(x => x.StaleMonths).GreaterThan(0);
            RuleFor(x => x.RemoteTarget).NotNull().NotEmpty();
            RuleFor(x => x.IgnoreBranches).NotNull();
        }
    }
}
