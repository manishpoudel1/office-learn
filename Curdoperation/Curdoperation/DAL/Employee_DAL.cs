using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;
using Curdoperation.Models;


namespace Curdoperation.DAL
{
    public class Employee_DAL
    {
        private SqlConnection? _connection;
        private SqlCommand? _command;
        public static IConfiguration? Configuration { get; set; }

        private string GetConnectionString()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
            return Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        public List<Employee> GetAll()
        {
            List<Employee> employeeList = new List<Employee>();

            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = new SqlCommand("[DBO].[USP_GET_EMPLOYEES]", _connection);
                _command.CommandType = CommandType.StoredProcedure;
                _connection.Open();

                using (SqlDataReader dr = _command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Employee employee = new Employee
                        {
                            Id = int.Parse(dr["Id"].ToString() ?? "0"),
                            FirstName = dr["FirstName"].ToString(),
                            LastName = dr["LastName"].ToString(),
                            DateOfBirth = (DateTime)dr["DateOfBirth"],
                            Email = dr["Email"].ToString(),
                            Salary = double.Parse(dr["Salary"].ToString() ?? "0")
                        };

                        employeeList.Add(employee);
                    }
                }

                _connection.Close();
            }

            return employeeList;
        }

        public bool Insert(Employee model)
        {
            int rowsAffected;
            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = new SqlCommand("[DBO].[USP_Insert_Employee]", _connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                _command.Parameters.AddWithValue("@FirstName", model.FirstName);
                _command.Parameters.AddWithValue("@LastName", model.LastName);
                _command.Parameters.AddWithValue("@DateOfBirth", model.DateOfBirth);
                _command.Parameters.AddWithValue("@Email", model.Email);
                _command.Parameters.AddWithValue("@Salary", model.Salary);

                _connection.Open();
                rowsAffected = _command.ExecuteNonQuery();
                _connection.Close();
            }

            return rowsAffected > 0;
        }

        public Employee GetById(int Id)
        {
            Employee employee = new Employee();

            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = new SqlCommand("[DBO].[USP_GET_EmployeebyId]", _connection);
                _command.Parameters.AddWithValue("@Id", Id);
                _command.CommandType = CommandType.StoredProcedure;
                _connection.Open();

                using (SqlDataReader dr = _command.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        employee.Id = int.Parse(dr["Id"].ToString() ?? "0");
                        employee.FirstName = dr["FirstName"].ToString();
                        employee.LastName = dr["LastName"].ToString();
                        employee.DateOfBirth = (DateTime)dr["DateOfBirth"];
                        employee.Email = dr["Email"].ToString();
                        employee.Salary = double.Parse(dr["Salary"].ToString() ?? "0");
                    }
                }

                _connection.Close();
            }

            return employee;
        }

        public bool Update(Employee model)
        {
            int rowsAffected;
            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = new SqlCommand("[DBO].[USP_Update_Employee]", _connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                _command.Parameters.AddWithValue("@Id", model.Id);
                _command.Parameters.AddWithValue("@FirstName", model.FirstName);
                _command.Parameters.AddWithValue("@LastName", model.LastName);
                _command.Parameters.AddWithValue("@DateOfBirth", model.DateOfBirth);
                _command.Parameters.AddWithValue("@Email", model.Email);
                _command.Parameters.AddWithValue("@Salary", model.Salary);

                _connection.Open();
                rowsAffected = _command.ExecuteNonQuery();
                _connection.Close();
            }

            return rowsAffected > 0;
        }

        public bool Delete(int Id)
        {
            int rowsAffected;
            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = new SqlCommand("[DBO].[USP_Delete_Employee]", _connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                _command.Parameters.AddWithValue("@Id", Id);

                _connection.Open();
                rowsAffected = _command.ExecuteNonQuery();
                _connection.Close();
            }

            return rowsAffected > 0;
        }
    }
}

    
