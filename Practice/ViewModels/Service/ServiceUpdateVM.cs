using System.ComponentModel.DataAnnotations.Schema;

namespace Practice.ViewModels.Service
{
    public class ServiceUpdateVM
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
    }
}
