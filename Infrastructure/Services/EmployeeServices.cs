using Infrastructure.Interface;
using Npgsql;
using Infrastructure.Models;
namespace Infrastructure.Services;

public class EmployeeServices :IEmployeeServices
{
     private readonly string connectionString = @"Server=127.0.0.1;Port=5432;Database=employeedb;User Id=postgres;Password=832111;";

    public int AddEmployee(Employee employee)
    {
        const string query = @"
            INSERT INTO Employee (FirstName, LastName, DateOfBirth, Position, Department, HireDate, Salary, IsActive)
            VALUES (@FirstName, @LastName, @DateOfBirth, @Position, @Department, @HireDate, @Salary, @IsActive)
            RETURNING EmployeeID;";

        using var connection = new NpgsqlConnection(connectionString);
        connection.Open();

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@FirstName", employee.FirstName);
        command.Parameters.AddWithValue("@LastName", employee.LastName);
        command.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
        command.Parameters.AddWithValue("@Position", employee.Position);
        command.Parameters.AddWithValue("@Department", employee.Deoartment);
        command.Parameters.AddWithValue("@HireDate", employee.HireDate);
        command.Parameters.AddWithValue("@Salary", employee.Salary);
        command.Parameters.AddWithValue("@IsActive", employee.IsActive);

        return (int)command.ExecuteScalar(); // Возвращает ID нового сотрудника
    }

    public Employee GetEmployeeById(int id)
    {
        const string query = "SELECT * FROM Employee WHERE EmployeeID = @EmployeeID";

        using var connection = new NpgsqlConnection(connectionString);
        connection.Open();

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@EmployeeID", id);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new Employee
            {
                EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                Position = reader.GetString(reader.GetOrdinal("Position")),
                Deoartment = reader.GetString(reader.GetOrdinal("Department")),
                HireDate = reader.GetDateTime(reader.GetOrdinal("HireDate")),
                Salary = reader.GetDecimal(reader.GetOrdinal("Salary")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
            };
        }

        return null; 
    }

    public List<Employee> GetAllEmployees()
    {
        const string query = "SELECT * FROM Employee";
        var employees = new List<Employee>();

        using var connection = new NpgsqlConnection(connectionString);
        connection.Open();

        using var command = new NpgsqlCommand(query, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            employees.Add(new Employee
            {
                EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                Position = reader.GetString(reader.GetOrdinal("Position")),
                Deoartment = reader.GetString(reader.GetOrdinal("Department")),
                HireDate = reader.GetDateTime(reader.GetOrdinal("HireDate")),
                Salary = reader.GetDecimal(reader.GetOrdinal("Salary")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
            });
        }

        return employees;
    }

    // UPDATE
    public bool UpdateEmployee(Employee employee)
    {
        const string query = @"
            UPDATE Employee 
            SET FirstName = @FirstName, 
                LastName = @LastName, 
                DateOfBirth = @DateOfBirth, 
                Position = @Position, 
                Department = @Department, 
                HireDate = @HireDate, 
                Salary = @Salary, 
                IsActive = @IsActive
            WHERE EmployeeID = @EmployeeID";

        using var connection = new NpgsqlConnection(connectionString);
        connection.Open();

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@EmployeeID", employee.EmployeeID);
        command.Parameters.AddWithValue("@FirstName", employee.FirstName);
        command.Parameters.AddWithValue("@LastName", employee.LastName);
        command.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
        command.Parameters.AddWithValue("@Position", employee.Position);
        command.Parameters.AddWithValue("@Department", employee.Deoartment);
        command.Parameters.AddWithValue("@HireDate", employee.HireDate);
        command.Parameters.AddWithValue("@Salary", employee.Salary);
        command.Parameters.AddWithValue("@IsActive", employee.IsActive);

        return command.ExecuteNonQuery() > 0; 
    }


    public bool DeleteEmployee(int id)
    {
        const string query = "DELETE FROM Employee WHERE EmployeeID = @EmployeeID";

        using var connection = new NpgsqlConnection(connectionString);
        connection.Open();

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@EmployeeID", id);

        return command.ExecuteNonQuery() > 0; 
    }
}

