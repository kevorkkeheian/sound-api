using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models
{
    public class Sound
    {
        public string TimeStamp { get; set; }
        
        public string Level { get; set; }

        public decimal Loudness { get; set; }

        public string MacAddress { get; set; }
        public string CreatedOn { get; set; }

    }
}