using System;

namespace WebApplication.Models
{
    public class Student
    {
        public string IndexNumber  { get; set;  } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string StudyName { get; set; }
        public int Semester { get; set; }
    }
}