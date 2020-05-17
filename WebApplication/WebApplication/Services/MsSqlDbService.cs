using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WebApplication.DTO.Requests;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class MsSqlDbService : IDbService
    {
        private static SqlConnection ConnectionProducer()
        {
            return new SqlConnection("Server=localhost,1433;User Id=sa; Password=password123!");
        }
        public IEnumerable<Student> GetStudents()
        {
            var studentsResult = new List<Student>();
            using (var connection = ConnectionProducer())
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText =
                    "select * from Student left join Enrollment E on Student.IdEnrollment = E.IdEnrollment left join Studies S on E.IdStudy = S.IdStudy;";
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
            // todo implement
        }

        public Student GetStudent(string index)
        {
            Student student = null;
            using (var connection = ConnectionProducer())
            using (var command = new SqlCommand())
            {
                SqlParameter indexParam = new SqlParameter();
                indexParam.ParameterName = "@Index";
                indexParam.Value = index;
                command.Connection = connection;
                command.CommandText =
                    "select * from Student left join Enrollment E on Student.IdEnrollment = E.IdEnrollment left join Studies S on E.IdStudy = S.IdStudy where IndexNumber = @Index;";
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
                connection = ConnectionProducer();
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

        public string EnrollStudent(EnrollmentRequest request)
        {
            if (!request.CorrectRequest() || !StudiesExist(request.Studies))
            {
                return null;
            }
            return "ok";
        }

        private static bool StudiesExist(string studiesName)
        {
            using var connection = ConnectionProducer();
            var command = new SqlCommand("select * from Studies where Name = @StudiesName", connection);
            var studiesNameParam = new SqlParameter {ParameterName = "@StudiesName", Value = studiesName};
            connection.Open();
            command.Connection = connection;
            command.Parameters.Add(studiesNameParam);
            var dataReader = command.ExecuteReader();
            var correct = dataReader.HasRows;
            Console.WriteLine(correct);
            dataReader.Close();
            return correct;
        }
    }
}