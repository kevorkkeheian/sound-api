using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models
{
    public class Sound
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        public string? HouseId { get; set; }
        public House? House { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public decimal Value { get; set; }

    }
}