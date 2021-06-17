using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

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
                        new AttributeDefinition
                        {
                            AttributeName = "DateTime",
                            AttributeType = "N"
                        }
                    },
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement
                        {
                            AttributeName = "Id",
                            KeyType = "HASH" // Partition Key
                        },
                        new KeySchemaElement
                        {
                            AttributeName = "DateTime",
                            KeyType = "RANGE" // Sort Key
                        }
                    },
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 5,
                        WriteCapacityUnits = 5,
                    },
                    TableName = "TestTable"
                };

                var response = await dynamoClient.CreateTableAsync(request);

                WaitUntilTableReady("TestTable");
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

        public void Insert()
        {

        }
    }
    ///https://www.youtube.com/watch?v=o3dZCM9i0Ss
}
