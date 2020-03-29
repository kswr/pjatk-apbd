using System.Collections;
using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.DAL
{
    public interface IDbService
    {
        public IEnumerable<Student> GetStudents();
        public void PutStudent(Student student);
        public void DeleteStudent(int id);
        public Student GetStudent(int id);
    }
}