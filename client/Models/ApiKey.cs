using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models
{
    public class ApiKey
    {
        public string? Id { get; set; }
        public string Key { get; set; }
    }
}