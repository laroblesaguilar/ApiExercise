using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DynamoDb.Libs.DynamoDb
{
    public class DynamoDbExample : IDynamoDbExample
    {
        private readonly IAmazonDynamoDB dynamoClient;

        public DynamoDbExample(IAmazonDynamoDB dynamoClient)
        {
            this.dynamoClient = dynamoClient;
        }

        public async void CreateDynamoDbTableAsync()
        {
            try
            {
                var request = new CreateTableRequest
                {
                    AttributeDefinitions = new List<AttributeDefinition>
                    {
                        new AttributeDefinition
                        {
                            AttributeName = "Id",
                            AttributeType = "N"
                        },
                    },
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement
                        {
                            AttributeName = "Id",
                            KeyType = "HASH" // Partition Key
                        }
                    },
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 5,
                        WriteCapacityUnits = 5,
                    },
                    TableName = "MiniLibraries"
                };

                var response = await dynamoClient.CreateTableAsync(request);

                WaitUntilTableReady("MiniLibraries");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void WaitUntilTableReady(string tableName)
        {
            string status = null;

            do
            {
                Thread.Sleep(5000);
                try
                {
                    var response = dynamoClient.DescribeTableAsync(new DescribeTableRequest
                    {
                        TableName = tableName
                    });

                    status = response.Result.Table.TableStatus;
                }
                catch (ResourceNotFoundException ex)
                {

                }
            } while (status != "ACTIVE");
            {
                Console.WriteLine("Table Created Successfully");
            }
        }

        public async Task Insert()
        {
            var table = Table.LoadTable(dynamoClient, "MiniLibraries");

            var miniLib = new Document();

            miniLib["Id"] = 1;
            miniLib["Description"] = "Mini Library located at Alamo Ranch";
            miniLib["Books"] = new List<string> { "Book1", "Book2", "Book3" };
            miniLib["Address"] = "123 Alamo Ranch Pkwy";

            await table.PutItemAsync(miniLib);
        }
        public async Task InsertObjectPersistenceModel()
        {
            var context = new DynamoDBContext(dynamoClient);
            var miniLib = new MiniLibrary
            {
                Id = 2,
                Description = "Mini Library added using OPM",
                Books = new List<string> { "Book 1", "Book 2", "Book 3" },
                Address = "123 Street"
            };

            await context.SaveAsync(miniLib);
        }
    }
}
