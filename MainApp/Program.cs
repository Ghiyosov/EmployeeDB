

using System;
using System.Collections.Generic;
using Infrastructure.Services;
using Infrastructure.Models;

class Program
{
    static void Main()
    {
        var repository = new EmployeeServices();
        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("=== Меню управления сотрудниками ===");
            Console.WriteLine("1. Добавить сотрудника");
            Console.WriteLine("2. Посмотреть сотрудника");
            Console.WriteLine("3. Посмотреть всех сотрудников");
            Console.WriteLine("4. Обновить сотрудника");
            Console.WriteLine("5. Удалить сотрудника");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите действие: ");

            switch (Console.ReadLine())
            {
                case "1":
                    AddEmployee(repository);
                    break;
                case "2":
                    ViewEmployee(repository);
                    break;
                case "3":
                    ViewAllEmployees(repository);
                    break;
                case "4":
                    UpdateEmployee(repository);
                    break;
                case "5":
                    DeleteEmployee(repository);
                    break;
                case "0":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Неверный выбор. Нажмите любую клавишу, чтобы продолжить...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void AddEmployee(EmployeeServices repository)
    {
        Console.Clear();
        Console.WriteLine("=== Добавление сотрудника ===");

        var employee = new Employee();

        Console.Write("Введите имя: ");
        employee.FirstName = Console.ReadLine();

        Console.Write("Введите фамилию: ");
        employee.LastName = Console.ReadLine();

        Console.Write("Введите дату рождения (гггг-мм-дд): ");
        employee.DateOfBirth = DateTime.Parse(Console.ReadLine());

        Console.Write("Введите должность: ");
        employee.Position = Console.ReadLine();

        Console.Write("Введите отдел: ");
        employee.Deoartment = Console.ReadLine();

        Console.Write("Введите дату приема на работу (гггг-мм-дд): ");
        employee.HireDate = DateTime.Parse(Console.ReadLine());

        Console.Write("Введите зарплату: ");
        employee.Salary = decimal.Parse(Console.ReadLine());

        Console.Write("Активен сотрудник? (да/нет): ");
        employee.IsActive = Console.ReadLine()?.ToLower() == "да";

        int newId = repository.AddEmployee(employee);
        Console.WriteLine($"Сотрудник успешно добавлен с ID: {newId}");
        Console.WriteLine("Нажмите любую клавишу, чтобы вернуться в меню...");
        Console.ReadKey();
    }

    static void ViewEmployee(EmployeeServices repository)
    {
        Console.Clear();
        Console.WriteLine("=== Просмотр сотрудника ===");

        Console.Write("Введите ID сотрудника: ");
        int id = int.Parse(Console.ReadLine());

        var employee = repository.GetEmployeeById(id);

        if (employee != null)
        {
            Console.WriteLine($"\nID: {employee.EmployeeID}");
            Console.WriteLine($"Имя: {employee.FirstName}");
            Console.WriteLine($"Фамилия: {employee.LastName}");
            Console.WriteLine($"Дата рождения: {employee.DateOfBirth:yyyy-MM-dd}");
            Console.WriteLine($"Должность: {employee.Position}");
            Console.WriteLine($"Отдел: {employee.Deoartment}");
            Console.WriteLine($"Дата приема на работу: {employee.HireDate:yyyy-MM-dd}");
            Console.WriteLine($"Зарплата: {employee.Salary:C}");
            Console.WriteLine($"Активен: {(employee.IsActive ? "Да" : "Нет")}");
        }
        else
        {
            Console.WriteLine("Сотрудник не найден.");
        }

        Console.WriteLine("\nНажмите любую клавишу, чтобы вернуться в меню...");
        Console.ReadKey();
    }

    static void ViewAllEmployees(EmployeeServices repository)
    {
        Console.Clear();
        Console.WriteLine("=== Список всех сотрудников ===");

        var employees = repository.GetAllEmployees();
        if (employees.Count > 0)
        {
            foreach (var employee in employees)
            {
                Console.WriteLine($"\nID: {employee.EmployeeID}, Имя: {employee.FirstName}, Фамилия: {employee.LastName}, Зарплата: {employee.Salary:C}, Активен: {(employee.IsActive ? "Да" : "Нет")}");
            }
        }
        else
        {
            Console.WriteLine("Сотрудники отсутствуют.");
        }

        Console.WriteLine("\nНажмите любую клавишу, чтобы вернуться в меню...");
        Console.ReadKey();
    }

    static void UpdateEmployee(EmployeeServices repository)
    {
        Console.Clear();
        Console.WriteLine("=== Обновление сотрудника ===");

        Console.Write("Введите ID сотрудника: ");
        int id = int.Parse(Console.ReadLine());

        var employee = repository.GetEmployeeById(id);
        if (employee != null)
        {
            Console.Write($"Имя ({employee.FirstName}): ");
            employee.FirstName = Console.ReadLine() ?? employee.FirstName;

            Console.Write($"Фамилия ({employee.LastName}): ");
            employee.LastName = Console.ReadLine() ?? employee.LastName;

            Console.Write($"Дата рождения ({employee.DateOfBirth:yyyy-MM-dd}): ");
            var dob = Console.ReadLine();
            employee.DateOfBirth = string.IsNullOrWhiteSpace(dob) ? employee.DateOfBirth : DateTime.Parse(dob);

            Console.Write($"Должность ({employee.Position}): ");
            employee.Position = Console.ReadLine() ?? employee.Position;

            Console.Write($"Отдел ({employee.Deoartment}): ");
            employee.Deoartment = Console.ReadLine() ?? employee.Deoartment;

            Console.Write($"Зарплата ({employee.Salary:C}): ");
            var salary = Console.ReadLine();
            employee.Salary = string.IsNullOrWhiteSpace(salary) ? employee.Salary : decimal.Parse(salary);

            Console.Write($"Активен ({(employee.IsActive ? "Да" : "Нет")}): ");
            var active = Console.ReadLine();
            employee.IsActive = string.IsNullOrWhiteSpace(active) ? employee.IsActive : active.ToLower() == "да";

            if (repository.UpdateEmployee(employee))
            {
                Console.WriteLine("Данные сотрудника успешно обновлены.");
            }
            else
            {
                Console.WriteLine("Ошибка обновления данных.");
            }
        }
        else
        {
            Console.WriteLine("Сотрудник не найден.");
        }

        Console.WriteLine("\nНажмите любую клавишу, чтобы вернуться в меню...");
        Console.ReadKey();
    }

    static void DeleteEmployee(EmployeeServices repository)
    {
        Console.Clear();
        Console.WriteLine("=== Удаление сотрудника ===");

        Console.Write("Введите ID сотрудника: ");
        int id = int.Parse(Console.ReadLine());

        if (repository.DeleteEmployee(id))
        {
            Console.WriteLine("Сотрудник успешно удален.");
        }
        else
        {
            Console.WriteLine("Ошибка удаления. Возможно, сотрудник не существует.");
        }

        Console.WriteLine("\nНажмите любую клавишу, чтобы вернуться в меню...");
        Console.ReadKey();
    }
}
