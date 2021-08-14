using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudentAccounting.Models;

namespace StudentAccounting.Data
{
    public class SeedData
    {
        public static void InitializeData(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.GetRequiredService<UniversityContext>();
            context.Database.Migrate();
            if (!context.Courses.Any())
            {
                var cources = GetCoursesForInitialization();
                var groups = GetGroupsForInitialization();
                var students = GetStudentsForInitialization();

                using var unitOfWork = new UnitOfWork(context);
                unitOfWork.Courses.AddRange(cources);
                context.Database.OpenConnection();
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Courses ON");
                unitOfWork.Complete();
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Courses OFF");

                unitOfWork.Groups.AddRange(groups);
                unitOfWork.Students.AddRange(students);
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Groups ON");
                unitOfWork.Complete();
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Groups OFF");
            }
        }

        private static List<Course> GetCoursesForInitialization()
        {
            return new List<Course>
            {
                new()
                {
                    Id = 1, Name = "Java Spring",
                    Description = "Java – один из самых популярных языков программирования в мире. На нем можно создавать и простые мобильные приложения, и сложные корпоративные системы программного обеспечения. Именно для таких систем и был создан стек Java Spring."
                },
                new()
                {
                    Id = 2, Name = "C#/.NET",
                    Description = "C# - объектно-ориентированный язык для платформы .Net и основной язык разработки в компаниях, которые используют ПО от Microsoft. На нем разрабатывают мобильные и настольные приложения. C# входит в пятерку самых популярных языков мира."
                },
                new()
                {
                    Id = 3, Name = "Android",
                    Description = "Android – операционная система для мобильных устройств с сенсорным экраном. Число мобильных пользователей интернета в 2017 превысило число пользователей десктопных приложений для web. Разработка на Android начинается с изучения Java и XML, в последнее время к этому списку добавился Kotlin."
                },
                new()
                {
                    Id = 4, Name = "Front-End",
                    Description = "Front End - это внешняя сторона любой программной системы или приложения, то, что видит и с чем взаимодействует пользователь. Разработка front end – одна из самых интересных и творческих отраслей программирования."
                },
                new()
                {
                    Id = 5, Name = "Python",
                    Description = "Python – это язык общего назначения, его можно применять для любых задач. Благодаря короткому синтаксису и четкой структуре он подходит для малых сценариев. Это позволяет создавать на Python несложные приложения практически для любой платформы."
                }
            };
        }

        private static List<Group> GetGroupsForInitialization()
        {
            return new List<Group>
            {
                new() {Id = 1, CourseId = 1, Name = "PJ-05", FormationDate = new DateTime(2020, 9, 1)},
                new() {Id = 2, CourseId = 1, Name = "QR-04", FormationDate = new DateTime(2020, 9, 1)},
                new() {Id = 3, CourseId = 2, Name = "BD-07", FormationDate = new DateTime(2020, 9, 1)},
                new() {Id = 4, CourseId = 3, Name = "UR-03", FormationDate = new DateTime(2020, 9, 1)},
                new() {Id = 5, CourseId = 4, Name = "FZ-08", FormationDate = new DateTime(2020, 9, 1)},
                new() {Id = 6, CourseId = 4, Name = "TD-13", FormationDate = new DateTime(2020, 9, 1)},
                new() {Id = 7, CourseId = 4, Name = "KZ-05", FormationDate = new DateTime(2020, 9, 1)},
                new() {Id = 8, CourseId = 5, Name = "BQ-02", FormationDate = new DateTime(2020, 9, 1)},
                new() {Id = 9, CourseId = 5, Name = "TP-08", FormationDate = new DateTime(2020, 9, 1)},
                new() {Id = 10, CourseId = 3, Name = "SR-01", FormationDate = new DateTime(2020, 9, 1)}
            };
        }

        private static List<Student> GetStudentsForInitialization()
        {
            return new List<Student>
            {
                new(1, "Иван", "Поляков") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(1, "Олег", "Горбачев") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(1, "Оксана", "Крылова") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(1, "Ярослав", "Громов") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(1, "Полина", "Васильева") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(1, "Евгений", "Сольвьев") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(1, "Александр", "Яковлев") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(1, "Анастасия", "Ульянова") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(1, "Софья", "Матвеева") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(1, "Евгения", "Фомина") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(1, "Владимир", "Давыдов") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(2, "Руслан", "Шилов") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(2, "Екатерина", "Медведева") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(2, "Максим", "Максимов") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(2, "Виктор", "Колесников") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(2, "Александр", "Морозов") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(2, "Даниил", "Булынко") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(2, "Илья", "Сергеев") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(2, "Софья", "Столярова") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(2, "Евгений", "Крылов") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(2, "Артем", "Волков") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(3, "Виктор", "Родин") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(3, "Петр", "Верещагин") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(3, "Ольга", "Егорова") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(3, "Влада", "Фокина") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(3, "Георгий", "Маслов") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(3, "Алина", "Беляева") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(3, "Марк", "Шмелев") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(3, "Влад", "Дьяков") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(3, "Вячеслав", "Иванов") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(3, "Ирина", "Гаврилова") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(3, "Елена", "Макаревич") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(3, "Федор", "Новиков") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(3, "Федор", "Новиков") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(3, "Никита", "Судаков") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(3, "Егор", "Киселев") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(3, "Дмитрий", "Смирнов") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(4, "Мария", "Ермакова") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(4, "Дарья", "Соболева") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(4, "Юлия", "Майорова") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(4, "Антон", "Мартынов") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(4, "Игорь", "Помомарев") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(4, "Константин", "Поляков") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(4, "Олеся", "Кузнецова") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(4, "Владимир", "Панин") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(4, "Карина", "Абрамова") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(5, "Ярослав", "Подлесных") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(5, "Глеб", "Абрамов") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(5, "Степан", "Федосеев") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(5, "Алиса", "Жукова") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(5, "Сергей", "Панов") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(5, "Ксения", "Хохлова") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(5, "Светлана", "Яковлева") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(5, "Диана", "Власова") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(5, "Роман", "Васильев") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(5, "Владислав", "Собовлев") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(5, "Наталья", "Короткова") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(5, "Лидия", "Коновалова") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(6, "Давид", "Гойко") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(6, "Константин", "Брянский") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(6, "Артур", "Елисеев") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(6, "Вадим", "Касьянов") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(6, "Игорь", "Третьяков") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(6, "Глеб", "Порошин") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(6, "Степан", "Разин") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(6, "Николай", "Черных") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(6, "Екатерина", "Филатова") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(6, "Алина", "Ахтямова") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(6, "Марина", "Макеева") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(6, "Юлия", "Мардуева") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(6, "Кира", "Алексеева") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(6, "Тимофей", "Иванов") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(7, "Маргарита", "Самойлова") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(7, "Алексей", "Родионов") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(7, "Максим", "Копылов") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(7, "Иван", "Севастьянов") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(7, "Павел", "Панков") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(7, "Денис", "Климов") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(7, "Денис", "Муратов") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(7, "Лариса", "Петрова") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(7, "Альбина", "Голубева") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(7, "Лев", "Толстой") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(8, "Георгий", "Шубин") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(8, "Арсений", "Сорокин") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(8, "Ева", "Михайлова") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(8, "Кира", "Губина") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(8, "Лев", "Авдеев") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(8, "Нина", "Алексеева") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(8, "Федор", "Никитин") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(8, "Андрей", "Борисов") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(8, "Михаил", "Горлов") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(8, "Ян", "Грачев") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(8, "Демид", "Громов") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(9, "Павел", "Чернишев") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(9, "Арина", "Еремина") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(9, "Диана", "Зуева") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(9, "Григорий", "Ильин") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(9, "Марк", "Князев") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(9, "Виктория", "Крылова") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(9, "Таисия", "Маслякова") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(9, "Елена", "Лаврова") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(10, "Виктор", "Устинов") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(10, "Влада", "Ревягина") {DateOfBirth = GetRandomDate(1990, 2000)},
                new(10, "Олег", "Копытов") {DateOfBirth = GetRandomDate(1990, 2000)}
            };
        }

        private static DateTime GetRandomDate(int minYear, int maxYear)
        {
            var rnd = new Random((int)DateTime.Now.Ticks);
            return new DateTime(minYear, 1, 1).AddDays(rnd.Next(0, 365 * (maxYear - minYear)));
        }
    }
}
