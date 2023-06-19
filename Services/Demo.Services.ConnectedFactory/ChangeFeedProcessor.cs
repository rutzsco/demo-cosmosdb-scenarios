using System;
using System.Collections.Generic;
using Demo.Services.ConnectedFactory.Data;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Demo.Services.ConnectedFactory
{
    public static class ChangeFeedProcessor
    {
        [FunctionName("ChangeFeedProcessor")]
        public static void Run([CosmosDBTrigger(databaseName: "ConnectedFactory", containerName: "Measurement", 
            Connection = "CosmosDBConnection",
            LeaseContainerName = "leases",
            CreateLeaseContainerIfNotExists = true)]IReadOnlyList<Record> input,
            ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);
                log.LogInformation("First document Id " + input[0].Id);
            }
        }
    }
}
