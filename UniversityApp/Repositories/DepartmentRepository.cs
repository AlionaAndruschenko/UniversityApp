using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using UniversityApp.Models;

namespace UniversityApp.Repositories
{
    public class DepartmentRepository
    {
        private readonly SqlConnection _connection;
        private SqlTransaction _transaction;

        public DepartmentRepository(SqlConnection connection, SqlTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        
        public void SetTransaction(SqlTransaction transaction)
        {
            _transaction = transaction;
        }

      
        public IEnumerable<Department> GetAll()
        {
            var list = new List<Department>();

            using var cmd = new SqlCommand("GetAllDepartments", _connection, _transaction);
            cmd.CommandType = CommandType.StoredProcedure;

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Department
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString() ?? string.Empty,
                    FacultyId = reader["Faculty_Id"] == DBNull.Value ? null : Convert.ToInt32(reader["Faculty_Id"]),
                    Head = reader["Head"] == DBNull.Value ? null : reader["Head"].ToString()
                });
            }

            return list;
        }

       
        public void Add(Department department)
        {
            using var cmd = new SqlCommand("AddDepartment", _connection, _transaction);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Name", department.Name);
            cmd.Parameters.AddWithValue("@Faculty_Id", (object?)department.FacultyId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Head", (object?)department.Head ?? DBNull.Value);

            cmd.ExecuteNonQuery();
        }

        
        public void Update(int id, string name, int? facultyId, string head)
        {
            using var cmd = new SqlCommand("UpdateDepartment", _connection, _transaction);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Name", (object?)name ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Faculty_Id", (object?)facultyId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Head", (object?)head ?? DBNull.Value);

            cmd.ExecuteNonQuery();
        }

        
        public void Delete(int id)
        {
            using var cmd = new SqlCommand("DeleteDepartment", _connection, _transaction);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
