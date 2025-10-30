using System;
using System.Data.SqlClient;
using UniversityApp.Repositories;

namespace UniversityApp.Data
{
    public class UnitOfWork : IDisposable
    {
        private readonly SqlConnection _connection;
        private SqlTransaction _transaction;

        public StudentRepository Students { get; private set; }
        public StudentGroupRepository Groups { get; private set; }
        public DepartmentRepository Departments { get; private set; }

        public UnitOfWork(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();

            
            Students = new StudentRepository(_connection, _transaction);
            Groups = new StudentGroupRepository(_connection, _transaction);
            Departments = new DepartmentRepository(_connection, _transaction);
        }

        public void Commit()
        {
            try
            {
                _transaction.Commit();

               
                _transaction = _connection.BeginTransaction();

               
                Students.SetTransaction(_transaction);
                Groups.SetTransaction(_transaction);
                Departments.SetTransaction(_transaction);
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
        }

        public void Rollback()
        {
            _transaction.Rollback();

            
            _transaction = _connection.BeginTransaction();

          
            Students.SetTransaction(_transaction);
            Groups.SetTransaction(_transaction);
            Departments.SetTransaction(_transaction);
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Dispose();
        }
    }
}
