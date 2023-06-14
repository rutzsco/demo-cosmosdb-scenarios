param location string

param cosmosDbAccountName string
param functionAppStorageAccountName string
param functionAppName string
param logAnalyticsWorkspaceName string

// Log Analytics
module logAnalytics 'log-analytics.bicep' = {
  name: 'logAnalytics' 
  params: {
    workspaceName: logAnalyticsWorkspaceName
    location: location
  }
}

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
    logAnalyticsWorkspaceId: logAnalytics.outputs.id
    cosmosDBConnectionString: db.outputs.connectionString
  }
}
