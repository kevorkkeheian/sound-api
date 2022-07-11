using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Application.Converter;
using Newtonsoft.Json;

namespace Application.Models
{
    public class Sound
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }
        
        // [JsonPropertyName("ts")]
        [JsonProperty("ts")]
        public string TimeStamp { get; set; }
        
        [JsonPropertyName("level")]
        public string Level { get; set; }

        [JsonPropertyName("loudness")]
        public decimal Loudness { get; set; }

        [JsonPropertyName("MacAddress")]
        public string MacAddress { get; set; }

        [JsonPropertyName("_event_time")]
        public string CreatedOn { get; set; }

    }
}