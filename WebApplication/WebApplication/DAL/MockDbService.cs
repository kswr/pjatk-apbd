using System;
using System.Collections.Generic;
using WebApplication.Models;
using System.Data.SqlClient;

namespace WebApplication.DAL
{
    public class MockDbService : IDbService
    {
        private static List<Student> Students;

        public IEnumerable<Student> GetStudents()
        {
            var studentsResult = new List<Student>();
            using (var connection = new SqlConnection("Server=localhost,1433;User Id=sa; Password=password123!"))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "select * from Student left join Enrollment E on Student.IdEnrollment = E.IdEnrollment left join Studies S on E.IdStudy = S.IdStudy;";
                connection.Open();
                var dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    var student = new Student();
                    student.IndexNumber = dataReader["IndexNumber"].ToString();
                    student.FirstName = dataReader["FirstName"].ToString();
                    student.LastName = dataReader["LastName"].ToString();
                    student.BirthDate = Convert.ToDateTime(dataReader["BirthDate"].ToString());
                    student.StudyName = dataReader["Name"].ToString();
                    student.Semester = Convert.ToInt32(dataReader["Semester"].ToString());
                    studentsResult.Add(student);
                }
            }

            return studentsResult;
        }

        public void PutStudent(Student student)
        {
            Students.Add(student);
        }

        public Student GetStudent(string index)
        {
            Student student = null;
            using (var connection = new SqlConnection("Server=localhost,1433;User Id=sa; Password=password123!"))
            using (var command = new SqlCommand())
            {
                SqlParameter indexParam = new SqlParameter();
                indexParam.ParameterName = "@Index";
                indexParam.Value = index;
                command.Connection = connection;
                command.CommandText = "select * from Student left join Enrollment E on Student.IdEnrollment = E.IdEnrollment left join Studies S on E.IdStudy = S.IdStudy where IndexNumber = @Index;";
                command.Parameters.Add(indexParam);
                connection.Open();
                var dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    student = new Student();
                    student.IndexNumber = dataReader["IndexNumber"].ToString();
                    student.FirstName = dataReader["FirstName"].ToString();
                    student.LastName = dataReader["LastName"].ToString();
                    student.BirthDate = Convert.ToDateTime(dataReader["BirthDate"].ToString());
                    student.StudyName = dataReader["Name"].ToString();
                    student.Semester = Convert.ToInt32(dataReader["Semester"].ToString());
                }
            }

            return student;
        }

        public void DeleteStudent(string id)
        {
            Console.WriteLine("Student deleted");
        }

        public IEnumerable<Student> GetSameSemesterStudents(string index)
        {
            var studentsResult = new List<Student>();
            SqlConnection connection = null;
            SqlDataReader dataReader = null;
            try
            {
                connection = new SqlConnection("Server=localhost,1433;User Id=sa; Password=password123!");
                SqlCommand command = new SqlCommand(
                    "select * from Student left join Enrollment E on Student.IdEnrollment = E.IdEnrollment left join Studies S on E.IdStudy = S.IdStudy where Semester = (select Semester from Student left join Enrollment E on Student.IdEnrollment = E.IdEnrollment left join Studies S on E.IdStudy = S.IdStudy where IndexNumber = @Index);",
                    connection);
                connection.Open();
                SqlParameter indexParam = new SqlParameter();
                indexParam.ParameterName = "@Index";
                indexParam.Value = index;
                command.Connection = connection;
                command.Parameters.Add(indexParam);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    var student = new Student();
                    student.IndexNumber = dataReader["IndexNumber"].ToString();
                    student.FirstName = dataReader["FirstName"].ToString();
                    student.LastName = dataReader["LastName"].ToString();
                    student.BirthDate = Convert.ToDateTime(dataReader["BirthDate"].ToString());
                    student.StudyName = dataReader["Name"].ToString();
                    student.Semester = Convert.ToInt32(dataReader["Semester"].ToString());
                    studentsResult.Add(student);
                }
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }

                if (connection != null)
                {
                    connection.Close();
                }
                
            }

            return studentsResult;
        }
    }
}