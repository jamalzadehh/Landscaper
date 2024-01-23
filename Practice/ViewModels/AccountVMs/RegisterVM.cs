using System.ComponentModel.DataAnnotations;

namespace Practice.ViewModels.AccountVMs
{
    public class RegisterVM
    {
        [Required]
        [MinLength(3)]
        [MaxLength(64)]        
        public string Name { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(64)]
        public string Surname    { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(64)]
        public string UserName { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(64)]
        [DataType(DataType.Password)]
        public string Password  { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Duzgun simvollardan istifade edin.")]
        public string Email { get; set; }
    }
}
