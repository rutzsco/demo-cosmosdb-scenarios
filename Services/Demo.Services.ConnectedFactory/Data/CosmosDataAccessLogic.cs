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
            var container = db.GetContainer("Measurements");

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

        public async Task<Measurement> MeasurementByIdQuery(string id)
        {
            var db = _cosmosClient.GetDatabase("ConnectedFactory");
            var container = db.GetContainer("Measurements");

            QueryDefinition query = new QueryDefinition("SELECT * FROM c where c.id = @id")
                .WithParameter("@id", id);

            List<Measurement> results = new();
            using (FeedIterator<Measurement> resultSetIterator = container.GetItemQueryIterator<Measurement>(query, requestOptions: new QueryRequestOptions() { }))
            {
                double totalRU = 0;
                while (resultSetIterator.HasMoreResults)
                {
                    FeedResponse<Measurement> response = await resultSetIterator.ReadNextAsync();
                    totalRU += response.RequestCharge;
                    results.AddRange(response);
                }
                _logger.LogMetric("RequestChargeMeasurementByIdQuery", totalRU);
            }

            return results.Single();
        }

        public async Task<Measurement> MeasurementByIdPointRead(string id)
        {
            var db = _cosmosClient.GetDatabase("ConnectedFactory");
            var container = db.GetContainer("Measurements");

            var measurement = await container.ReadItemAsync<Measurement>(id: id, partitionKey: new PartitionKey(id));
            _logger.LogMetric("RequestChargeMeasurementByIdPointRead", measurement.RequestCharge);

            return measurement;
        }

        public async Task<IEnumerable<Measurement>> Readiness()
        {
            var db = _cosmosClient.GetDatabase("ConnectedFactory");
            var container = db.GetContainer("Measurements");

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
