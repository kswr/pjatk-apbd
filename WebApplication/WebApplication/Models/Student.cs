using System;

namespace WebApplication.Models
{
    public class Student
    {
        public int IdStudent { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IndexNumber  { get; set; } = $"s{new Random().Next(1, 20000)}";
    }
}