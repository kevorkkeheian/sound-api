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


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sound>>> GetSound()
        {
            HttpClient _client = new HttpClient();

            
            
            _client.DefaultRequestHeaders.Add("Authorization", "ApiKey HmCjwb1agD7nRvI8OAiYx7SCo6mb8MvB1QDKM9cc10NXGjpUpYp5dvGX8bVE1iQW");
            // _client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            var response = await _client.PostAsJsonAsync<RocksetRequest>("https://api.use1a1.rockset.com/v1/orgs/self/ws/commons/lambdas/PBISample/tags/latest", null);
            
            return Ok(response.Content.ReadFromJsonAsync<RocksetRequest>().Result.Results);
        }
    }
}
