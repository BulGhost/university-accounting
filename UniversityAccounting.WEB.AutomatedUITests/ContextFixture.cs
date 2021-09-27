using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UniversityAccounting.DAL.EF;
using UniversityAccounting.DAL.Entities;

namespace UniversityAccounting.WEB.AutomatedUITests
{
    public class ContextFixture : IDisposable
    {
        private readonly UniversityContext _context;
        private Process _kestrelProcess;

        public int TestGroupId { get; }
        public string TestGroupName { get; }
        public Student TestStudent { get; set; }

        public ContextFixture()
        {
            _context = new UniversityContext();
            var group = _context.Groups.First();
            TestGroupId = group.Id;
            TestGroupName = group.Name;

            StartKestrel();
        }


        public void Dispose()
        {
            DeleteAddedStudent(TestStudent);
            _context.Dispose();

            KillKestrel();
        }


        private void DeleteAddedStudent(Student student)
        {
            if (student == null) return;

            var studentToDelete = _context.Students
                .SingleOrDefault(s => s.FirstName == student.FirstName &&
                                      s.LastName == student.LastName &&
                                      s.DateOfBirth == student.DateOfBirth);

            if (studentToDelete == null) return;

            _context.Students.Remove(studentToDelete);
            _context.SaveChanges();
        }

        private void StartKestrel()
        {
            _kestrelProcess = new Process
            {
                StartInfo =
                {
                    FileName = "dotnet",
                    Arguments = "run",
                    WorkingDirectory = Path.Combine(GetSolutionFolderPath(), "UniversityAccounting.WEB")
                }
            };
            _kestrelProcess.Start();
        }

        private static string GetSolutionFolderPath()
        {
            var directory = new DirectoryInfo(Environment.CurrentDirectory);

            while (directory?.GetFiles("*.sln").Length == 0)
                directory = directory.Parent;

            return directory?.FullName;
        }

        private void KillKestrel()
        {
            if (!_kestrelProcess.HasExited)
            {
                _kestrelProcess.Kill();
                _kestrelProcess.Dispose();
            }
        }
    }
}
