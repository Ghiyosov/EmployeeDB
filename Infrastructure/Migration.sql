-- Создать базу данных
CREATE DATABASE EmployeeDB;

-- Использовать базу данных
USE EmployeeDB;

-- Создать таблицу Employee
CREATE TABLE Employee (
                          EmployeeID INT AUTO_INCREMENT PRIMARY KEY,  -- Уникальный ID сотрудника
                          FirstName VARCHAR(50) NOT NULL,             -- Имя
                          LastName VARCHAR(50) NOT NULL,              -- Фамилия
                          DateOfBirth DATE,                           -- Дата рождения
                          Position VARCHAR(100),                      -- Должность
                          Department VARCHAR(100),                    -- Отдел
                          HireDate DATE NOT NULL,                     -- Дата найма
                          Salary DECIMAL(10, 2),                      -- Зарплата
                          IsActive BOOLEAN DEFAULT TRUE               -- Статус (работает или нет)
);
