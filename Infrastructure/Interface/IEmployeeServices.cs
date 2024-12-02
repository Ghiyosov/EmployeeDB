using Infrastructure.Models;

namespace Infrastructure.Interface;

public interface IEmployeeServices
{
    List<Employee> GetAllEmployees();
    Employee GetEmployeeById(int id);
    int AddEmployee(Employee employee);
    bool UpdateEmployee(Employee employee);
    bool DeleteEmployee(int id);
}