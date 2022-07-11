using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Application.Converter;
using Newtonsoft.Json;

namespace Application.Models
{
    public class Sound
    {
        [JsonProperty("_id")]
        public string Id { get; set; }
        
        [JsonProperty("ts")]
        public string TimeStamp { get; set; }
        
        [JsonProperty("level")]
        public string Level { get; set; }

        [DynamoDBProperty("loudness")]
        public decimal Loudness { get; set; }

        [DynamoDBProperty("MacAddress")]
        public string MacAddress { get; set; }

        [DynamoDBProperty("_event_time")]
        public string CreatedOn { get; set; }

    }
}