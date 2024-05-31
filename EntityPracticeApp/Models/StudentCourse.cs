namespace EntityPracticeApp.Models
{
    
    public class StudentCourse
    {
        
        public int CoursesCourseId { get; set; }
        public int StudentsSid { get; set; }
        public DateTime Created {  get; set; }
        public DateTime LastUpdated { get; set; }
        public bool CourseAssigned { get; set; }
    }
}
