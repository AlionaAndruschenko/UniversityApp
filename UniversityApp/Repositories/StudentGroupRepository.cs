using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using UniversityApp.Models;

namespace UniversityApp.Repositories
{
    public class StudentGroupRepository
    {
        private readonly SqlConnection _connection;
        private SqlTransaction _transaction;

        public StudentGroupRepository(SqlConnection connection, SqlTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

       
        public void SetTransaction(SqlTransaction transaction)
        {
            _transaction = transaction;
        }

      
        public IEnumerable<StudentGroup> GetAll()
        {
            var groups = new List<StudentGroup>();
            using var cmd = new SqlCommand(
                "SELECT Id, Name, Department_Id FROM StudentGroup WHERE Is_Deleted = 0",
                _connection, _transaction);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                groups.Add(new StudentGroup
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    DepartmentId = reader.GetInt32(2)
                });
            }

            return groups;
        }


        
        public void Add(StudentGroup group)
        {
            using var cmd = new SqlCommand(
                "INSERT INTO StudentGroup (Name, Department_Id) VALUES (@Name, @Department_Id)",
                _connection, _transaction);

            cmd.Parameters.AddWithValue("@Name", group.Name);
            cmd.Parameters.AddWithValue("@Department_Id", (object?)group.DepartmentId ?? DBNull.Value);

            cmd.ExecuteNonQuery();
        }

        public void Update(int id, string groupName, int? departmentId)
        {
            using var cmd = new SqlCommand(
                "UPDATE StudentGroup SET Name = @Name, Department_Id = @Department_Id WHERE Id = @Id",
                _connection, _transaction);

            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Name", (object?)groupName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Department_Id", (object?)departmentId ?? DBNull.Value);

            cmd.ExecuteNonQuery();
        }

      
        public void Delete(int id)
        {
            using var cmd = new SqlCommand("SoftDeleteGroup", _connection, _transaction);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@GroupId", id);
            cmd.ExecuteNonQuery();
        }

    }
}
