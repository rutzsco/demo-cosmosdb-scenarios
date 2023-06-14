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


        public async Task<IEnumerable<Record>> Readiness()
        {
            var db = _cosmosClient.GetDatabase("TVD");
            var container = db.GetContainer("Records");

            QueryDefinition query = new QueryDefinition("SELECT TOP 1 * FROM r");
            List<Record> results = new();
            using (FeedIterator<Record> resultSetIterator = container.GetItemQueryIterator<Record>(query, requestOptions: new QueryRequestOptions() { }))
            {
                while (resultSetIterator.HasMoreResults)
                {
                    FeedResponse<Record> response = await resultSetIterator.ReadNextAsync();
                    results.AddRange(response);
                }
            }

            return results;
        }
    }
}
