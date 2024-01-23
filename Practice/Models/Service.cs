using Practice.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Practice.Models
{
    public class Service:BaseEntity
    {
        public string FullName { get; set; }
        public string Description { get; set; }
        public string  ImageUrl { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
    }
}
