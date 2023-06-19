using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Services.ConnectedFactory.Data
{
    public class CosmosDataAccessLogic
    {
        private readonly CosmosClient _cosmosClient;
        private readonly ILogger _logger;

        public CosmosDataAccessLogic(ILogger logger, CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
            _logger = logger;
        }

        public async Task<IEnumerable<Measurement>> Measurements()
        {
            var db = _cosmosClient.GetDatabase("ConnectedFactory");
            var container = db.GetContainer("Measurement");

            QueryDefinition query = new QueryDefinition("SELECT * FROM m");
            List<Measurement> results = new();
            using (FeedIterator<Measurement> resultSetIterator = container.GetItemQueryIterator<Measurement>(query, requestOptions: new QueryRequestOptions() { }))
            {
                while (resultSetIterator.HasMoreResults)
                {
                    FeedResponse<Measurement> response = await resultSetIterator.ReadNextAsync();
                    results.AddRange(response);
                }
            }

            return results;
        }

        public async Task<IEnumerable<Context>> Context()
        {
            var db = _cosmosClient.GetDatabase("ConnectedFactory");
            var container = db.GetContainer("Context");

            QueryDefinition query = new QueryDefinition("SELECT * FROM c");
            List<Context> results = new();
            using (FeedIterator<Context> resultSetIterator = container.GetItemQueryIterator<Context>(query, requestOptions: new QueryRequestOptions() { }))
            {
                while (resultSetIterator.HasMoreResults)
                {
                    FeedResponse<Context> response = await resultSetIterator.ReadNextAsync();
                    results.AddRange(response);
                }
            }

            return results;
        }

        public async Task<IEnumerable<Measurement>> Readiness()
        {
            var db = _cosmosClient.GetDatabase("ConnectedFactory");
            var container = db.GetContainer("Measurement");

            QueryDefinition query = new QueryDefinition("SELECT TOP 1 * FROM m");
            List<Measurement> results = new();
            using (FeedIterator<Measurement> resultSetIterator = container.GetItemQueryIterator<Measurement>(query, requestOptions: new QueryRequestOptions() { }))
            {
                while (resultSetIterator.HasMoreResults)
                {
                    FeedResponse<Measurement> response = await resultSetIterator.ReadNextAsync();
                    results.AddRange(response);
                }
            }

            return results;
        }
    }
}
