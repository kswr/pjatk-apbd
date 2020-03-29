using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using WebApplication.Models;

namespace WebApplication.DAL
{
    public class MockDbService : IDbService
    {
        private static List<Student> Students;

        static MockDbService()
        {
            Students = new List<Student>
            {
                new Student{IdStudent=1, FirstName="Jan", LastName="Kowalski"},
                new Student{IdStudent=2, FirstName="Anna", LastName="Malewski"},
                new Student{IdStudent=3, FirstName="Andrzej", LastName="Andrzejewicz"}
            };
        }
        public IEnumerable<Student> GetStudents()
        {
            return Students;
        }

        public void PutStudent(Student student)
        {
            Students.Add(student);
        }

        public Student GetStudent(int id)
        {
            return Students.Find(s => s.IdStudent == id);
        }

        public void DeleteStudent(int id)
        {
            Student toDelete = Students.Find(s => s.IdStudent == id);
            Students.Remove(toDelete);
        }
    }
}