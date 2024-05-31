using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EntityPracticeApp.ViewModel
{
    public class StudentViewModel
    {
        [Key]
        public int Sid { get; set; }
        [Required]
        [MaxLength(10,ErrorMessage ="length can not be more than 10 characters")]
        public string Name { get; set; }
        [Required (ErrorMessage ="Email field can not be empty")]
        [EmailAddress(ErrorMessage ="Input valid email address")]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"\+92\d{10}", ErrorMessage = "Phone number must start with +92 and be followed by 10 digits.")]
        public string Phone { get; set; }
        [Required (ErrorMessage ="Please select gender")]
        public string Gender { get; set; }
        [Required]
        public string DateOfBirth { get; set; }

        public string University { get; set; } = "FAST";

        public bool isAssigned { get; set; }

    }
}
