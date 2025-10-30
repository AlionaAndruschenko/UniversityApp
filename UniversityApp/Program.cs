using System;
using UniversityApp.Data;
using UniversityApp.Models;

namespace UniversityApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=DESKTOP-VFCTIQP\\SQLEXPRESS02;Database=UniversityDB;Trusted_Connection=True;TrustServerCertificate=True;";

            using var uow = new UnitOfWork(connectionString);

            bool running = true;

            while (running)
            {
                Console.WriteLine("\n=== Головне меню ===");
                Console.WriteLine("1. Робота зі студентами");
                Console.WriteLine("2. Робота з групами");
                Console.WriteLine("0. Вихід");
                Console.Write("Ваш вибір: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        StudentMenu(uow);
                        break;
                    case "2":
                        GroupMenu(uow);
                        break;
                    case "0":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("❌ Невірний вибір!");
                        break;
                }
            }

            Console.WriteLine("Програма завершена.");
        }

       
        static void StudentMenu(UnitOfWork uow)
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n--- Меню студентів ---");
                Console.WriteLine("1. Переглянути всіх");
                Console.WriteLine("2. Додати");
                Console.WriteLine("3. Оновити");
                Console.WriteLine("4. Видалити");
                Console.WriteLine("0. Назад");
                Console.Write("Вибір: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ShowAllStudents(uow); break;
                    case "2": AddStudent(uow); break;
                    case "3": UpdateStudent(uow); break;
                    case "4": DeleteStudent(uow); break;
                    case "0": back = true; break;
                    default: Console.WriteLine("❌ Невірний вибір!"); break;
                }
            }
        }

      
        static void GroupMenu(UnitOfWork uow)
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n--- Меню груп ---");
                Console.WriteLine("1. Переглянути всі");
                Console.WriteLine("2. Додати");
                Console.WriteLine("3. Оновити");
                Console.WriteLine("4. Видалити");
                Console.WriteLine("0. Назад");
                Console.Write("Вибір: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ShowAllGroups(uow); break;
                    case "2": AddGroup(uow); break;
                    case "3": UpdateGroup(uow); break;
                    case "4": DeleteGroup(uow); break;
                    case "0": back = true; break;
                    default: Console.WriteLine("❌ Невірний вибір!"); break;
                }
            }
        }

        
        static void ShowAllStudents(UnitOfWork uow)
        {
            var students = uow.Students.GetAll();
            Console.WriteLine("\n=== Список студентів ===");
            foreach (var s in students)
                Console.WriteLine($"{s.Id}: {s.FirstName} {s.LastName}, Email: {s.Email}, GroupId: {s.GroupId}");
        }

        static void AddStudent(UnitOfWork uow)
        {
            Console.Write("Ім'я: ");
            string firstName = Console.ReadLine();

            Console.Write("Прізвище: ");
            string lastName = Console.ReadLine();

            Console.Write("Email (або Enter): ");
            string email = Console.ReadLine();

            Console.Write("ID групи (або Enter): ");
            string groupInput = Console.ReadLine();
            int? groupId = string.IsNullOrWhiteSpace(groupInput) ? null : int.Parse(groupInput);

            var student = new Student
            {
                FirstName = firstName,
                LastName = lastName,
                Email = string.IsNullOrWhiteSpace(email) ? null : email,
                GroupId = groupId
            };

            try
            {
                uow.Students.Add(student);
                uow.Commit();
                Console.WriteLine("✅ Студента успішно додано!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Помилка при додаванні: " + ex.Message);
                uow.Rollback();
            }
        }

        static void UpdateStudent(UnitOfWork uow)
        {
            Console.Write("ID студента: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Нове ім'я (або Enter): ");
            string firstName = Console.ReadLine();

            Console.Write("Нове прізвище (або Enter): ");
            string lastName = Console.ReadLine();

            Console.Write("Новий Email (або Enter): ");
            string email = Console.ReadLine();

            Console.Write("Новий ID групи (або Enter): ");
            string groupInput = Console.ReadLine();
            int? groupId = string.IsNullOrWhiteSpace(groupInput) ? null : int.Parse(groupInput);

            try
            {
                uow.Students.Update(id, firstName, lastName, email, groupId);
                uow.Commit();
                Console.WriteLine("✅ Студента оновлено!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Помилка при оновленні: " + ex.Message);
                uow.Rollback();
            }
        }

        static void DeleteStudent(UnitOfWork uow)
        {
            Console.Write("ID студента для видалення: ");
            int id = int.Parse(Console.ReadLine());

            try
            {
                uow.Students.Delete(id);
                uow.Commit();
                Console.WriteLine("✅ Студента видалено!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Помилка при видаленні: " + ex.Message);
                uow.Rollback();
            }
        }

        
        static void ShowAllGroups(UnitOfWork uow)
        {
            var groups = uow.Groups.GetAll();
            Console.WriteLine("\n=== Список груп ===");
            foreach (var g in groups)
                Console.WriteLine($"{g.Id}: {g.Name}, DepartmentId: {g.DepartmentId}");
        }

        static void AddGroup(UnitOfWork uow)
        {
            Console.Write("Назва групи: ");
            string name = Console.ReadLine();

            Console.Write("ID кафедри (або Enter): ");
            string deptInput = Console.ReadLine();
            int? departmentId = string.IsNullOrWhiteSpace(deptInput) ? null : int.Parse(deptInput);

            var group = new StudentGroup
            {
                Name = name,
                DepartmentId = departmentId
            };

            try
            {
                uow.Groups.Add(group);
                uow.Commit();
                Console.WriteLine("✅ Групу додано!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Помилка при додаванні: " + ex.Message);
                uow.Rollback();
            }
        }

        static void UpdateGroup(UnitOfWork uow)
        {
            Console.Write("ID групи: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Нова назва (або Enter): ");
            string name = Console.ReadLine();

            Console.Write("Новий ID кафедри (або Enter): ");
            string deptInput = Console.ReadLine();
            int? departmentId = string.IsNullOrWhiteSpace(deptInput) ? null : int.Parse(deptInput);

            try
            {
                uow.Groups.Update(id, name, departmentId);
                uow.Commit();
                Console.WriteLine("✅ Групу оновлено!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Помилка при оновленні: " + ex.Message);
                uow.Rollback();
            }
        }

        static void DeleteGroup(UnitOfWork uow)
        {
            Console.Write("ID групи для видалення: ");
            int id = int.Parse(Console.ReadLine());

            try
            {
                uow.Groups.Delete(id);
                uow.Commit();
                Console.WriteLine("✅ Групу видалено!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Помилка при видаленні: " + ex.Message);
                uow.Rollback();
            }
        }
    }
}
