using System;

namespace WebApplication.Models
{
    public class Student
    {
        private static int _counter;
        public int IdStudent { get; } = _counter++; 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IndexNumber  { get; } = $"s{new Random().Next(1, 20000)}";
    }
}