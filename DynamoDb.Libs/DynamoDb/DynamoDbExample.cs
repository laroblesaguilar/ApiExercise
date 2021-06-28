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

        public async Task Insert()
        {
            var context = new DynamoDBContext(dynamoClient);

            var miniLib = new MiniLibrary
            {
                Id = 2,
                Description = "Mini Library added using OPM",
                Books = new List<Book>
                {
                    new Book
                    {
                        Author = new Author
                        {
                            FirstName = "James",
                            LastName = "Lampkins"
                        },
                        Title = "James' Book",
                        ISBN = "123456798",
                        IsFiction = true
                    }
                },
                Address = "123 Street"
            };

            await context.SaveAsync(miniLib);
        }

        public async Task<MiniLibrary> GetMiniLibraryById(int id)
        {
            var context = new DynamoDBContext(dynamoClient);
            return await context.LoadAsync<MiniLibrary>(id);
        }

        public async Task<MiniLibrary> AddBook(int miniLibId, Book book)
        {
            var context = new DynamoDBContext(dynamoClient);
            var miniLib = await context.LoadAsync<MiniLibrary>(miniLibId);
            miniLib.Books.Add(book);
            await context.SaveAsync(miniLib);
            return miniLib;
        }
    }
}
