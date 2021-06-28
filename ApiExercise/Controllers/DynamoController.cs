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

        [HttpPost]
        [Route("insert")]
        public IActionResult Insert()
        {
            dynamoDbExample.Insert();
            return Ok();
        }
        [HttpPost]


        [Route("opm/insert")]
        public IActionResult InsertOPM()
        {
            dynamoDbExample.InsertObjectPersistenceModel();
            return Ok();
        }
    }
}
