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
    public class MeasurementAddEndpoint
    {
        private CosmosClient _cosmosClient;

        public MeasurementAddEndpoint(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
        }

        [FunctionName("MeasurementAddEndpoint")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "Measurement")] HttpRequest req, [CosmosDB(databaseName: "ConnectedFactory", containerName: "Measurements", Connection = "CosmosDBConnection", CreateIfNotExists = true, PartitionKey="/id")] IAsyncCollector<Measurement> documents, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var record = JsonConvert.DeserializeObject<Measurement>(requestBody);
            record.Id = Guid.NewGuid().ToString();
            
            await documents.AddAsync(record);

            return new OkObjectResult(record.Id);
        }
    }
}
