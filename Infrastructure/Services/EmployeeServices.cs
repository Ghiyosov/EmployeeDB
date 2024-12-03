using Infrastructure.Common;
using Infrastructure.Interface;
using Npgsql;
using Infrastructure.Models;

namespace Infrastructure.Services;

public class EmployeeServices : IEmployeeServices
{
    public int AddEmployee(Employee employee)
    {
        try
        {
            using var connection = NpgsqlHelper.CreateConnection(SqlCommands.connectionString);
            using var command = new NpgsqlCommand(SqlCommands.createEmployee, connection);
            
            AddEmployeeParameters(command, employee);
            
            return (int)command.ExecuteScalar(); // Возвращает ID нового сотрудника
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка при добавлении сотрудника: {e.Message}");
            throw;
        }
    }

    public Employee GetEmployeeById(int employeeID)
    {
        try
        {
            using var connection = NpgsqlHelper.CreateConnection(SqlCommands.connectionString);
            using var command = new NpgsqlCommand(SqlCommands.selectByID, connection);
            
            command.Parameters.AddWithValue("@EmployeeID", employeeID);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapEmployee(reader);
            }

            return null; // Если сотрудник не найден
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка при получении сотрудника по ID: {e.Message}");
            throw;
        }
    }

    public List<Employee> GetAllEmployees()
    {
        try
        {
            var employees = new List<Employee>();
            using var connection = NpgsqlHelper.CreateConnection(SqlCommands.connectionString);
            using var command = new NpgsqlCommand(SqlCommands.selectAll, connection);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                employees.Add(MapEmployee(reader));
            }

            return employees;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка при получении всех сотрудников: {e.Message}");
            throw;
        }
    }

    public bool UpdateEmployee(Employee employee)
    {
        try
        {
            using var connection = NpgsqlHelper.CreateConnection(SqlCommands.connectionString);
            using var command = new NpgsqlCommand(SqlCommands.updateEmployee, connection);
            
            AddEmployeeParameters(command, employee);
            command.Parameters.AddWithValue("@EmployeeID", employee.EmployeeID);

            return command.ExecuteNonQuery() > 0;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка при обновлении сотрудника: {e.Message}");
            throw;
        }
    }

    public bool DeleteEmployee(int employeeID)
    {
        try
        {
            using var connection = NpgsqlHelper.CreateConnection(SqlCommands.connectionString);
            using var command = new NpgsqlCommand(SqlCommands.deleteByID, connection);

            command.Parameters.AddWithValue("@EmployeeID", employeeID);

            return command.ExecuteNonQuery() > 0;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка при удалении сотрудника: {e.Message}");
            throw;
        }
    }

    // Метод для добавления параметров сотрудника к команде
    private void AddEmployeeParameters(NpgsqlCommand command, Employee employee)
    {
        command.Parameters.AddWithValue("@FirstName", employee.FirstName);
        command.Parameters.AddWithValue("@LastName", employee.LastName);
        command.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
        command.Parameters.AddWithValue("@Position", employee.Position);
        command.Parameters.AddWithValue("@Department", employee.Department);
        command.Parameters.AddWithValue("@HireDate", employee.HireDate);
        command.Parameters.AddWithValue("@Salary", employee.Salary);
        command.Parameters.AddWithValue("@IsActive", employee.IsActive);
    }

    // Метод для маппинга данных из reader в объект Employee
    private Employee MapEmployee(NpgsqlDataReader reader)
    {
        return new Employee
        {
            EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
            LastName = reader.GetString(reader.GetOrdinal("LastName")),
            DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
            Position = reader.GetString(reader.GetOrdinal("Position")),
            Department = reader.GetString(reader.GetOrdinal("Department")),
            HireDate = reader.GetDateTime(reader.GetOrdinal("HireDate")),
            Salary = reader.GetDecimal(reader.GetOrdinal("Salary")),
            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
        };
    }
}

public static class SqlCommands
{
    public static readonly string connectionString = @"Server=127.0.0.1;Port=5432;Database=employeedb;User Id=postgres;Password=832111;";
    
    public const string selectAll = "SELECT * FROM Employee";
    public const string selectByID = "SELECT * FROM Employee WHERE EmployeeID = @EmployeeID";
    public const string deleteByID = "DELETE FROM Employee WHERE EmployeeID = @EmployeeID";
    public const string createEmployee = @"INSERT INTO Employee (FirstName, LastName, DateOfBirth, Position, Department, HireDate, Salary, IsActive)
                                            VALUES (@FirstName, @LastName, @DateOfBirth, @Position, @Department, @HireDate, @Salary, @IsActive)          
                                            RETURNING EmployeeID;";
    public const string updateEmployee = @"UPDATE Employee 
                                           SET FirstName = @FirstName, 
                                               LastName = @LastName, 
                                               DateOfBirth = @DateOfBirth, 
                                               Position = @Position, 
                                               Department = @Department, 
                                               HireDate = @HireDate, 
                                               Salary = @Salary, 
                                               IsActive = @IsActive
                                           WHERE EmployeeID = @EmployeeID";
}
