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
    public class RecordEndpoints
    {
        private CosmosClient _cosmosClient;

        public RecordEndpoints(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
        }

        [FunctionName("RecordGetEndpoint")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "/records")] HttpRequest req, ILogger log)
        {
            var dataAccess = new CosmosDataAccessLogic(log, _cosmosClient);
            var results = await dataAccess.Readiness();
            return new OkObjectResult(results);
        }

        [FunctionName("RecordAddEndpoint")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "/record")] HttpRequest req, [CosmosDB(databaseName: "Measurements", containerName: "Records", Connection = "CosmosDBConnection", CreateIfNotExists = true)] IAsyncCollector<Record> documents, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var record = JsonConvert.DeserializeObject<Record>(requestBody);
            await documents.AddAsync(record);

            return new OkObjectResult("OK");
        }
    }
}