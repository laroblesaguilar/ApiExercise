using System;
using System.Collections.Generic;
using System.Text;

namespace DynamoDb.Libs
{
    public class Book
    {
        public Author Author { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public bool IsFiction { get; set; }
    }
}
