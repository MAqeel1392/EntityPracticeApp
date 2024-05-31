using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;

namespace EntityPracticeApp.Models
{
    public class Student
    {
        [Key]
        public int Sid { get; set; }
        [Column("Student Name")]
        public string Name { get; set; }
        [Column("Student Email")]
        public string Email{ get; set;}
        public string Phone { get; set; }
        public int Gender { get; set; }
        public DateTime DOB { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        //public bool isAssigned { get; set; }
    }
}

