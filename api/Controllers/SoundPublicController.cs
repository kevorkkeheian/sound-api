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
    [Route("api/sound/public")]
    [ApiController]
    
    public class SoundPublicController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public SoundPublicController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sound>>> GetSound()
        {
            HttpClient _client = new HttpClient();

            // get rockset api key from appsettings usin configuration
            var rocksetApiKey = _configuration.GetValue<string>("Rockset:ApiKey");
            var rocksetApiUrl = _configuration.GetValue<string>("Rockset:ApiUrl");


            
            _client.DefaultRequestHeaders.Add("Authorization", rocksetApiKey);
            // _client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            var response = await _client.PostAsJsonAsync<RocksetRequest>($"{rocksetApiUrl}/v1/orgs/self/ws/commons/lambdas/PBISample/tags/latest", null);
            
            return Ok(response.Content.ReadFromJsonAsync<RocksetRequest>().Result.Results);
        }
    }
}
