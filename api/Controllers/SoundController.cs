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
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public SoundController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [HttpGet("today")]
        public async Task<ActionResult<IEnumerable<Sound>>> GetTodaySound()
        {

             if(!ApiKeyExists(Request)) return Unauthorized();

            HttpClient _client = new HttpClient();

            // get rockset api key from appsettings usin configuration
            var rocksetApiKey = _configuration.GetValue<string>("Rockset:ApiKey");
            var rocksetApiUrl = _configuration.GetValue<string>("Rockset:ApiUrl");
            
            _client.DefaultRequestHeaders.Add("Authorization", rocksetApiKey);
            var response = await _client.PostAsJsonAsync<RocksetRequest>($"{rocksetApiUrl}/v1/orgs/self/ws/commons/lambdas/TodayValues/tags/latest", null);
            
            return Ok(response.Content.ReadFromJsonAsync<RocksetRequest>().Result.Results);

        }



        [Route("search")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sound>>> Search([FromQuery] string name, [FromQuery] string type, [FromQuery] string value)
        {

             if(!ApiKeyExists(Request)) return Unauthorized();

            HttpClient _client = new HttpClient();

            // get rockset api key from appsettings usin configuration
            var rocksetApiKey = _configuration.GetValue<string>("Rockset:ApiKey");
            var rocksetApiUrl = _configuration.GetValue<string>("Rockset:ApiUrl");
            
            _client.DefaultRequestHeaders.Add("Authorization", rocksetApiKey);
            var response = await _client.PostAsJsonAsync<RocksetRequest>($"{rocksetApiUrl}/v1/orgs/self/ws/commons/lambdas/FilterByMacAddress/tags/latest?name={name}&type={type}&value={value}", null);
            
            return Ok(response.Content.ReadFromJsonAsync<RocksetRequest>().Result.Results);
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
