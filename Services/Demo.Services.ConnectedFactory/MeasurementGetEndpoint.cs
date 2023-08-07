using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Configuration;
using Demo.Services.ConnectedFactory.Data;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos;

namespace Demo.Services.ConnectedFactory
{
    public class MeasurementGetEndpoint
    {
        private CosmosClient _cosmosClient;

        public MeasurementGetEndpoint(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
        }

        [FunctionName("MeasurementGetEndpoint")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "Measurements")] HttpRequest req, ILogger log)
        {
            var dataAccess = new CosmosDataAccessLogic(log, _cosmosClient);
            var results = await dataAccess.Measurements();
            return new OkObjectResult(results);
        }

        [FunctionName("MeasurementGetByIdEndpoint")]
        public async Task<IActionResult> RunGetById([HttpTrigger(AuthorizationLevel.Function, "get", Route = "Measurements/{id}")] HttpRequest req,string id, ILogger log)
        {
            var dataAccess = new CosmosDataAccessLogic(log, _cosmosClient);
            var results = await dataAccess.MeasurementByIdQuery(id);
            return new OkObjectResult(results);
        }
    }
}
