using Git.Clean.Interfaces;
using Git.Clean.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Git.Clean
{
    /// <summary>
    /// Provides a startup orchestrator for the console application.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configure service registrations for the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IGitCleanService, LibGitCleanService>();
        }
    }
}
