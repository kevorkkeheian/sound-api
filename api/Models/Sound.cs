using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Application.Converter;

namespace Application.Models
{
    [DynamoDBTable("Esp8266_SM_Table")]
    public class Sound
    {
        [DynamoDBProperty("ts")]
        public Int64 TimeStamp { get; set; }
        
        [DynamoDBProperty("Level")]
        public string Level { get; set; }

        [DynamoDBProperty("Loudness")]
        public int Loudness { get; set; }

        [DynamoDBProperty("macAddress")]
        public string MacAddress { get; set; }

        [DynamoDBProperty("Time")]
        public string CreatedOn { get; set; }

    }
}