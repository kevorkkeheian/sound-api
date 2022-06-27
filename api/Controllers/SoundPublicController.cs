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
        private readonly IDynamoDBContext _dynamoContext;
        private readonly ApplicationDbContext _context;

        public SoundPublicController(IDynamoDBContext dynamoContext, ApplicationDbContext context)
        {
            _dynamoContext = dynamoContext;
            _context = context;
        }

        // GET: api/Sound
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sound>>> GetSound()
        {
            var conditions = new List<ScanCondition>();

            return await _dynamoContext.ScanAsync<Sound>(conditions).GetRemainingAsync();
        }
    }
}
