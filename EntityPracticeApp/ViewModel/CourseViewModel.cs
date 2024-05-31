using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace EntityPracticeApp.ViewModel
{
    public class CourseViewModel
    {
        [Key]
        public int CourseId { get; set; }
        [Required]
        [MaxLength(10, ErrorMessage = "Title can not more than 10 character!")]
        public string Title { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Description can not more than 20 character!")]
        public string Description { get; set; }
        [Required ]
        public bool isAssigned { get; set; }
    }
}
