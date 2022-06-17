using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models
{
    public class Sound
    {
        public Int64 TimeStamp { get; set; }
        
        public string Level { get; set; }

        public int Loudness { get; set; }

        public string MacAddress { get; set; }
        public string CreatedOn { get; set; }

    }
}