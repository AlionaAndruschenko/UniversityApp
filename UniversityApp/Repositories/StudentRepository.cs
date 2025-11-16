using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using UniversityApp.Models;

namespace UniversityApp.Repositories
{
    public class StudentRepository
    {
        private readonly SqlConnection _connection;
        private SqlTransaction _transaction; 

        public StudentRepository(SqlConnection connection, SqlTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        
        public void SetTransaction(SqlTransaction transaction)
        {
            _transaction = transaction;
        }

       
        public IEnumerable<Student> GetAll()
        {
            var students = new List<Student>();

            using var cmd = new SqlCommand(
                "SELECT Id, First_Name, Last_Name, Email, Group_Id FROM Student WHERE Is_Deleted = 0",
                _connection, _transaction);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                students.Add(new Student
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Email = reader.IsDBNull(3) ? null : reader.GetString(3),
                    GroupId = reader.IsDBNull(4) ? null : reader.GetInt32(4)
                });
            }

            reader.Close();
            return students;
        }


       
        public void Add(Student student)
        {
            using var cmd = new SqlCommand(
                "INSERT INTO Student (First_Name, Last_Name, Email, Group_Id,Enrollment_Date, updated_at, Is_Deleted) VALUES (@First_Name, @Last_Name, @Email, @Group_Id, @EnrollmentDate, @UpdatedAt, 0)",
                _connection, _transaction);

            cmd.Parameters.AddWithValue("@First_Name", student.FirstName);
            cmd.Parameters.AddWithValue("@Last_Name", student.LastName);
            cmd.Parameters.AddWithValue("@Email", (object?)student.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Group_Id", (object?)student.GroupId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@EnrollmentDate", DateTime.Now);
            cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);

            cmd.ExecuteNonQuery();
        }

        
        public void Update(int id, string firstName, string lastName, string email, int? groupId)
        {
            using var cmd = new SqlCommand(
                "UPDATE Student SET First_Name = @First_Name, Last_Name = @Last_Name, Email = @Email, Group_Id = @Group_Id WHERE Id = @Id",
                _connection, _transaction);

            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@First_Name", (object?)firstName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Last_Name", (object?)lastName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object?)email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Group_Id", (object?)groupId ?? DBNull.Value);

            cmd.ExecuteNonQuery();
        }

       
        public void Delete(int id)
        {
            using var cmd = new SqlCommand("SoftDeleteStudent", _connection, _transaction);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StudentId", id);
            cmd.ExecuteNonQuery();
        }

    }
}
