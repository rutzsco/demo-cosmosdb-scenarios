param location string

param cosmosDbAccountName string
param functionAppStorageAccountName string
param functionAppName string


// Service Bus
module db 'cosmosdb.bicep' = {
	name: 'cosmosdb'
	params: {
      location: location
      accountName: cosmosDbAccountName
      databaseName: 'ConnectedFactory'
	}
}

// Function
module function 'function.bicep' = {
  name: 'functionDeploy'
  params: {
    location: location
    functionAppName: functionAppName
    storageAccountName: functionAppStorageAccountName
    cosmosDBConnectionString: db.outputs.connectionString
  }
}
