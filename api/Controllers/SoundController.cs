using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Models;
using Api.Attributes;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class SoundController : ControllerBase
    {
        private readonly IDynamoDBContext _dynamoContext;
        private readonly ApplicationDbContext _context;

        public SoundController(IDynamoDBContext dynamoContext, ApplicationDbContext context)
        {
            _dynamoContext = dynamoContext;
            _context = context;
        }

        // GET: api/Sound
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sound>>> GetSound()
        {
            if(!ApiKeyExists(Request)) return Unauthorized();

            var conditions = new List<ScanCondition>();

            return await _dynamoContext.ScanAsync<Sound>(conditions).GetRemainingAsync();
        }

        [HttpGet("today")]
        public async Task<ActionResult<IEnumerable<Sound>>> GetTodaySound()
        {
            if(!ApiKeyExists(Request)) return Unauthorized();

            var conditions = new List<ScanCondition>();

            // convert today to timestamp
            var now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            var epoch = new DateTime(1970, 1, 1);
            var timestamp = (long)(startDate - epoch).TotalMilliseconds;

            // add condition to scan
            conditions.Add(new ScanCondition("TimeStamp", ScanOperator.GreaterThanOrEqual, timestamp));

            // return result
            return await _dynamoContext.ScanAsync<Sound>(conditions).GetRemainingAsync();

        }



        [Route("search/{mac}")]
        [HttpGet]
        public async Task<IActionResult> Search(string mac, [FromQuery] string? ts)
        {
            // Note: You can only query the tables that have a composite primary key (partition key and sort key).

            // 1. Construct QueryFilter
            var queryFilter = new QueryFilter("macAddress", QueryOperator.Equal, mac);

            if (!string.IsNullOrEmpty(ts))
            {
                queryFilter.AddCondition("ts", QueryOperator.Equal, Convert.ToInt64(ts));
            }

            // 2. Construct QueryOperationConfig
            var queryOperationConfig = new QueryOperationConfig
            {
                Filter = queryFilter
            };

            // 3. Create async search object
            var search = _dynamoContext.FromQueryAsync<Sound>(queryOperationConfig);

            // 4. Finally get all the data in a singleshot
            var searchResponse = await search.GetRemainingAsync();


            // Return it
            return Ok(searchResponse);
        }

        private bool ApiKeyExists(HttpRequest request)
        {
            var key = request.Headers.FirstOrDefault(x => x.Key == "key").Value.FirstOrDefault();

            return (_context.ApiKey?.Any(e => e.Key == key)).GetValueOrDefault();
        }

        private static DateTime UnixTimeStampToDateTime( double unixTimeStamp )
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds( unixTimeStamp ).ToLocalTime();
            return dateTime;
        }
    }
}
