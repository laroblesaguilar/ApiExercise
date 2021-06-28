using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDb.Libs
{
    public interface IDynamoDbExample
    {
        void CreateDynamoDbTableAsync();
        Task Insert();
        Task<MiniLibrary> GetMiniLibraryById(int id);
        Task<MiniLibrary> AddBook(int miniLibId, Book book);
    }
}
