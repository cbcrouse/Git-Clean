using FluentValidation.TestHelper;
using FluentValidation.Validators;
using FluentValidation.Validators.UnitTestExtension.Composer;
using FluentValidation.Validators.UnitTestExtension.Core;
using Git.Clean.Validators;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using System.IO;
using System.Reflection;
using Xunit;

namespace Git.Clean.Tests
{
    public class RemoveStaleBranchesValidatorTests
    {
        [Fact]
        public void Validator_Fails_OnInvalidDirectory()
        {
            // Arrange
            const string invalidDirectory = "X:\\Temp";
            var sut = new RemoveStaleBranchesValidator();

            // Act & Assert
            sut.ShouldHaveValidationErrorFor(x => x.GitDirectory, invalidDirectory).WithErrorMessage("Must be a valid directory.");
        }

        [Fact]
        public void Validator_Fails_OnInvalidGitDirectory()
        {
            // Arrange
            string invalidDirectory = Path.GetTempPath();
            var sut = new RemoveStaleBranchesValidator();

            // Act & Assert
            sut.ShouldHaveValidationErrorFor(x => x.GitDirectory, invalidDirectory).WithErrorMessage("Must be a valid .git directory.");
        }

        [Fact]
        public void Validator_Succeeds_OnValidDirectory()
        {
            // Arrange
            string frameworkDir = Directory.GetParent(Assembly.GetExecutingAssembly().GetAssemblyLocation())!.FullName;
            string configDir = Directory.GetParent(frameworkDir)!.FullName;
            string binDir = Directory.GetParent(configDir)!.FullName;
            string projectDir = Directory.GetParent(binDir)!.FullName;
            string tstDir = Directory.GetParent(projectDir)!.FullName;
            string slnDir = Directory.GetParent(tstDir)!.FullName;
            var sut = new RemoveStaleBranchesValidator();

            // Act & Assert
            sut.ShouldNotHaveValidationErrorFor(x => x.GitDirectory, slnDir);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Validator_Fails_OnInvalidStaleMonths(int staleMonths)
        {
            // Arrange
            var sut = new RemoveStaleBranchesValidator();

            // Act & Assert
            sut.ShouldHaveValidationErrorFor(x => x.StaleMonths, staleMonths);
        }

        [Fact]
        public void Validator_HasCorrectValidators()
        {
            // Arrange
            var sut = new RemoveStaleBranchesValidator();

            // Act & Assert
            sut.ShouldHaveRules(x => x.StaleMonths,
                BaseVerifiersSetComposer.Build()
                    .AddPropertyValidatorVerifier<GreaterThanValidator>(0)
                    .Create());

            sut.ShouldHaveRules(x => x.RemoteTarget,
                BaseVerifiersSetComposer.Build()
                    .AddPropertyValidatorVerifier<NotNullValidator>()
                    .AddPropertyValidatorVerifier<NotEmptyValidator>()
                    .Create());

            sut.ShouldHaveRules(x => x.IgnoreBranches,
                BaseVerifiersSetComposer.Build()
                    .AddPropertyValidatorVerifier<NotNullValidator>() 
                    .Create());
        }
    }
}