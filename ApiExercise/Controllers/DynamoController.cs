using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamoDb.Libs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiExercise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DynamoController : ControllerBase
    {
        private IDynamoDbExample dynamoDbExample;

        public DynamoController(IDynamoDbExample dynamoDbExample)
        {
            this.dynamoDbExample = dynamoDbExample;
        }

        [Route("create/table")]
        public IActionResult CreateDynamoDbTable()
        {
            dynamoDbExample.CreateDynamoDbTableAsync();
            return Ok();
        }

        //     {
        //    "Id": 4,
        //    "Description":"Mini Lib sent from request body",
        //    "Books": [{
        //        "Author": {"FirstName":"Test", "LastName": "McTesterson"},
        //        "Tile":"RequestBodyTitle",
        //        "ISBN": "12345678910",
        //        "IsFiction": true
        //    }],
        //    "Address": { "Street": "6218 Gilmer Ml", "City": "San Antonio", "State": "Texas", "ZipCode": 78253}
        //    } Example 
        [HttpPost]
        [Route("insert")]
        public IActionResult Insert([FromBody] MiniLibrary miniLibrary)
        {
            dynamoDbExample.Insert(miniLibrary);
            return Ok();
        }

        [HttpPost]
        [Route("minilibrary/{id}/delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await dynamoDbExample.Delete(id);
            return Ok();
        }


        [HttpGet]
        [Route("minilibrary/{id}")]
        public async Task<IActionResult> GetMiniLibraryById(int id)
        {
            return Ok(await dynamoDbExample.GetMiniLibraryById(id));
        }


        //    [{
        //    "Author": {"FirstName":"Leo", "LastName": "Robles"},
        //    "Title":"Narnia",
        //    "ISBN": "12345678910",
        //    "IsFiction": true
        //}] Example
        [HttpPost]
        [Route("minilibrary/{id}/add")]
        public async Task<IActionResult> AddBookToMiniLibrary(int id, [FromBody]List<Book> books)
        {
            return Ok(await dynamoDbExample.AddBooks(id, books));
        }


        [HttpGet]
        [Route("minilibrary/search")]
        public async Task<IActionResult> FindMiniLibraryByCity()
        {
            return Ok(await dynamoDbExample.GetMiniLibrariesByBook("Henderson"));
        }
    }
}
