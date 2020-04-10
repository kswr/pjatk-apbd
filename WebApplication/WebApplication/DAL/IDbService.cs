using System;
using System.Collections;
using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.DAL
{
    public interface IDbService
    {
        public IEnumerable<Student> GetStudents();
        public void PutStudent(Student student);
        public void DeleteStudent(string id);
        public Student GetStudent(string id);
        public IEnumerable<Student> GetSameSemesterStudents(string index);
    }
}