using System.ComponentModel.DataAnnotations.Schema;

namespace Practice.ViewModels
{
    public class ServiceCreateVM
    {
        
        public string FullName { get; set; }
        public string Description { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
    }
}
