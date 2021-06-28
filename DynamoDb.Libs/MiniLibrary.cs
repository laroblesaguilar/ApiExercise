using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace DynamoDb.Libs
{
    [DynamoDBTable("MiniLibraries")]
    public class MiniLibrary
    {
        [DynamoDBHashKey]
        public int Id { get; set; }
        public string Description { get; set; }
        public List<string> Books { get; set; }
        public string Address { get; set; }
    }
}
