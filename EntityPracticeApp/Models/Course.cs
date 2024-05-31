using System.ComponentModel.DataAnnotations;

namespace EntityPracticeApp.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public string Title { get; set; }       
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set;}

        //public bool isAssigned {  get; set; } 
    }
}
