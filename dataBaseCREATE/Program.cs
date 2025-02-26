﻿using System;
using Microsoft.Data.SqlClient;

class Program
{
    static void Main()
    {
        string connectionString = "Server=NPK-102O-11;Integrated Security=true;TrustServerCertificate=True;";

        // Создание базы данных и таблиц
        CreateDatabaseAndTables(connectionString);


        Console.WriteLine("Данные успешно добавлены!");
    }

    static void CreateDatabaseAndTables(string connectionString)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            // Создание базы данных
            string createDatabaseQuery = "IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'University') CREATE DATABASE University;";
            SqlCommand createDatabaseCommand = new SqlCommand(createDatabaseQuery, connection);
            createDatabaseCommand.ExecuteNonQuery();

            // Подключение к созданной базе данных
            connection.ChangeDatabase("University");

            // Создание таблиц
            string createStudentsTable = @"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Students')
                BEGIN
                    CREATE TABLE Students (
                        StudentId INT PRIMARY KEY IDENTITY,
                        FirstName NVARCHAR(50),
                        LastName NVARCHAR(50),
                        DateOfBirth DATE
                    )
                END";

            string createCoursesTable = @"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Courses')
                BEGIN
                    CREATE TABLE Courses (
                        CourseId INT PRIMARY KEY IDENTITY,
                        CourseName NVARCHAR(100),
                        Credits INT
                    )
                END";

            string createEnrollmentsTable = @"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Enrollments')
                BEGIN
                    CREATE TABLE Enrollments (
                        EnrollmentId INT PRIMARY KEY IDENTITY,
                        StudentId INT,
                        CourseId INT,
                        EnrollmentDate DATE,
                        FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
                        FOREIGN KEY (CourseId) REFERENCES Courses(CourseId)
                    )
                END";

            // Выполнение команд на создание таблиц
            new SqlCommand(createStudentsTable, connection).ExecuteNonQuery();
            new SqlCommand(createCoursesTable, connection).ExecuteNonQuery();
            new SqlCommand(createEnrollmentsTable, connection).ExecuteNonQuery();

            Console.WriteLine("База данных и таблицы успешно созданы или уже существуют!");
        }
    }
}
