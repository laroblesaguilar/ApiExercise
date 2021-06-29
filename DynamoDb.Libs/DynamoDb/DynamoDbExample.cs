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
            catch (Exception ex)
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

        public async Task Insert(MiniLibrary miniLibrary)
        {
            var context = new DynamoDBContext(dynamoClient);
            await context.SaveAsync(miniLibrary);
        }


        public async Task Delete(int miniLibraryId)
        {
            var context = new DynamoDBContext(dynamoClient);
            await context.DeleteAsync<MiniLibrary>(miniLibraryId);
        }

        public async Task<MiniLibrary> GetMiniLibraryById(int id)
        {
            var context = new DynamoDBContext(dynamoClient);
            return await context.LoadAsync<MiniLibrary>(id);
        }

        public async Task<MiniLibrary> AddBooks(int miniLibId, List<Book> books)
        {
            var context = new DynamoDBContext(dynamoClient);
            var miniLib = await context.LoadAsync<MiniLibrary>(miniLibId);
            
            foreach(var b in books)
            {
                miniLib.Books.Add(b);
            }

            await context.SaveAsync(miniLib);
            return miniLib;
        }

        public async Task<List<MiniLibrary>> GetMiniLibrariesByBook(string title)
        {
            var context = new DynamoDBContext(dynamoClient);

            var conditions = new List<ScanCondition> { new ScanCondition("Books", ScanOperator.Contains, title) };

            var miniLibs = await context.ScanAsync<MiniLibrary>(conditions).GetRemainingAsync();

            return miniLibs;
        }
    }
}
