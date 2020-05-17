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
            using var connection = ConnectionProducer();
            using var command = new SqlCommand
            {
                Connection = connection,
                CommandText =
                    "select * from Student left join Enrollment E on Student.IdEnrollment = E.IdEnrollment left join Studies S on E.IdStudy = S.IdStudy;"
            };
            connection.Open();
            var dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                var student = new Student
                {
                    IndexNumber = dataReader["IndexNumber"].ToString(),
                    FirstName = dataReader["FirstName"].ToString(),
                    LastName = dataReader["LastName"].ToString(),
                    BirthDate = Convert.ToDateTime(dataReader["BirthDate"].ToString()),
                    StudyName = dataReader["Name"].ToString(),
                    Semester = Convert.ToInt32(dataReader["Semester"].ToString())
                };
                studentsResult.Add(student);
            }

            dataReader.Close();
            return studentsResult;
        }

        public void PutStudent(Student student)
        {
            // todo implement
        }

        public Student GetStudent(string index)
        {
            Student student = null;
            using var connection = ConnectionProducer();
            using var command = new SqlCommand();
            var indexParam = new SqlParameter {ParameterName = "@Index", Value = index};
            command.Connection = connection;
            command.CommandText =
                "select * from Student left join Enrollment E on Student.IdEnrollment = E.IdEnrollment left join Studies S on E.IdStudy = S.IdStudy where IndexNumber = @Index;";
            command.Parameters.Add(indexParam);
            connection.Open();
            var dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                student = new Student
                {
                    IndexNumber = dataReader["IndexNumber"].ToString(),
                    FirstName = dataReader["FirstName"].ToString(),
                    LastName = dataReader["LastName"].ToString(),
                    BirthDate = Convert.ToDateTime(dataReader["BirthDate"].ToString()),
                    StudyName = dataReader["Name"].ToString(),
                    Semester = Convert.ToInt32(dataReader["Semester"].ToString())
                };
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
                var command = new SqlCommand(
                    "select * from Student left join Enrollment E on Student.IdEnrollment = E.IdEnrollment left join Studies S on E.IdStudy = S.IdStudy where Semester = (select Semester from Student left join Enrollment E on Student.IdEnrollment = E.IdEnrollment left join Studies S on E.IdStudy = S.IdStudy where IndexNumber = @Index);",
                    connection);
                connection.Open();
                var indexParam = new SqlParameter {ParameterName = "@Index", Value = index};
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
                dataReader?.Close();
                connection?.Close();
            }

            return studentsResult;
        }

        public int EnrollStudent(EnrollmentRequest request)
        {
            var idEnrollment = -1;
            if (!request.CorrectRequest())
            {
                return idEnrollment;
            }

            using var connection = ConnectionProducer();
            using var command = new SqlCommand {Connection = connection};

            connection.Open();
            var transaction = connection.BeginTransaction();

            try
            {
                command.CommandText = "select IdStudy from Studies where Name=@studiesName";
                command.Parameters.AddWithValue("name", request.Studies);

                var sqlDataReader = command.ExecuteReader();
                if (!sqlDataReader.Read())
                {
                    transaction.Rollback();
                    return -1;
                }
            
            var idStudies = (int) sqlDataReader["IdStudies"];
            sqlDataReader.Close();
            
            command.CommandText = "select * from Enrollment where IdStudy=@idStudies and Semester=1";
            command.Parameters.AddWithValue("idStudies", idStudies);
            
            sqlDataReader = command.ExecuteReader();
            
            
            if (sqlDataReader.Read())
            {
                idEnrollment = (int) sqlDataReader["IdEnrollment"];
            }
            
            sqlDataReader.Close();
            
            if (idEnrollment.Equals(-1))
            {
                command.CommandText =
                    "INSERT INTO Enrollment(Semester, IdStudy, StartDate) VALUES(@semester, idStudy, startDate)";
                command.Parameters.AddWithValue("semester", 1);
                command.Parameters.AddWithValue("idStudy", idStudies);
                command.Parameters.AddWithValue("startDate", DateTime.Now);
                command.ExecuteNonQuery();
            
                command.CommandText = "select * from Enrollment where IdStudy=@idStudies and Semester=1";
                command.Parameters.AddWithValue("idStudies", idStudies);
                sqlDataReader = command.ExecuteReader();
                idEnrollment = (int) sqlDataReader["IdEnrollment"];
            }
            
            
            command.CommandText =
                "INSERT INTO Student(IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) VALUES(@indexNumber, @firstName, @lastName, @birthDate, @idEnrollment)";
            command.Parameters.AddWithValue("indexNumber", request.IndexNumber);
            command.Parameters.AddWithValue("firstName", request.FirstName);
            command.Parameters.AddWithValue("lastName", request.LastName);
            command.Parameters.AddWithValue("birthDate", request.ExtractBirthDate());
            command.Parameters.AddWithValue("idEnrollment", idEnrollment);
            command.ExecuteNonQuery();
            
            transaction.Commit();
            return idEnrollment;
            }
            catch (SqlException exc)
            {
                transaction.Rollback();
            }
            return idEnrollment;
        }
    }
}