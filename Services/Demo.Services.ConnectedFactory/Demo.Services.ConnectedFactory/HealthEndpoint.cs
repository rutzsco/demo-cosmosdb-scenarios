using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Demo.Services.ConnectedFactory
{
    public static class HealthEndpoint
    {
        [FunctionName("HealthEndpoint")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "health")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("OK");
        }
    }
}
