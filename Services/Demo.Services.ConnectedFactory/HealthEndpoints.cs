using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;
using Demo.Services.ConnectedFactory.Data;

namespace Demo.Services.ConnectedFactory.TVD
{
    public class HealthEndpoints
    {
        private CosmosClient _cosmosClient;

        public HealthEndpoints(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
        }

        [FunctionName("HealthLivenessEndpoint")]
        public async Task<IActionResult> HealthLivenessEndpoint([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "health")] HttpRequest req, ILogger log)
        {
            log.LogInformation($"{Diagnostics.ActivityCode.HealthCheckLiveness} - Executing HealthCheck");
            return new OkObjectResult("OK");
        }

        [FunctionName("HealthReadinessEndpoint")]
        public async Task<IActionResult> HealthReadinessEndpoint([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "health/readiness")] HttpRequest req, ILogger log)
        {
            log.LogInformation($"{Diagnostics.ActivityCode.HealthCheckReadiness} - Executing HealthCheck");

            var logic = new CosmosDataAccessLogic(log, _cosmosClient);
            await logic.Readiness();

            return new OkObjectResult("OK");
        }
    }
}
