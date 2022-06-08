using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models
{
    public class House
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
    }
}