using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(Demo.Services.ConnectedFactory.Startup))]

namespace Demo.Services.ConnectedFactory
{
    public class Startup : FunctionsStartup
    {
        private static readonly IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory)
            .AddEnvironmentVariables()
            .Build();

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton((s) =>
            {
                string connectionString = configuration["CosmosDBConnectionString"];
                CosmosClientBuilder configurationBuilder = new CosmosClientBuilder(connectionString);
                return configurationBuilder
                        .Build();
            });
        }
    }
}