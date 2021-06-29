using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDb.Libs
{
    public interface IDynamoDbExample
    {
        void CreateDynamoDbTableAsync();
        Task Insert(MiniLibrary miniLibrary);
        Task Delete(int miniLibraryId);
        Task<MiniLibrary> GetMiniLibraryById(int id);
        Task<MiniLibrary> AddBooks(int miniLibId, List<Book> books);
        Task<List<MiniLibrary>> GetMiniLibrariesByBook(string title);
    }
}
