using Microsoft.AspNetCore.Mvc;
using System.Linq;
using StudentAccounting.Data;
using StudentAccounting.Models;
using StudentAccounting.Models.ViewModels;

namespace StudentAccounting.Controllers
{
    public class StudentsController : Controller
    {
        private const int StudentsPerPage = 10;
        private readonly IUnitOfWork _unitOfWork;

        public StudentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int groupId, int page = 1)
        {
            ViewBag.Group = _unitOfWork.Groups.Get(groupId);
            return View(new StudentsIndexViewModel
            {
                Students = _unitOfWork.Students.GetAll()
                    .Where(s => s.GroupId == groupId)
                    .OrderBy(s => s.Id)
                    .Skip((page - 1) * StudentsPerPage)
                    .Take(StudentsPerPage),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = StudentsPerPage,
                    TotalItems = _unitOfWork.Students.GetAll()
                        .Count(s => s.GroupId == groupId)
                }
            });
        }

        public IActionResult Create(int groupId)
        {
            var student = new Student {GroupId = groupId};
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Students.Add(student);
                _unitOfWork.Complete();
                return RedirectToAction("Index", new {groupId = student.GroupId});
            }

            return View(student);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var student = _unitOfWork.Students.Get((int) id);
            if (student == null) return NotFound();

            ViewBag.Groups = _unitOfWork.Groups.Find(g => g.CourseId == student.Group.CourseId);
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                var updStudent = _unitOfWork.Students.Get(student.Id);
                updStudent.FirstName = student.FirstName;
                updStudent.LastName = student.LastName;
                updStudent.DateOfBirth = student.DateOfBirth;
                updStudent.GroupId = student.GroupId;
                updStudent.Status = student.Status;
                _unitOfWork.Complete();
                return RedirectToAction("Index", new {groupId = student.GroupId});
            }

            return View(student);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var student = _unitOfWork.Students.Get((int) id);
            if (student == null) return NotFound();

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var student = _unitOfWork.Students.Get((int)id);
            if (student == null) return NotFound();

            _unitOfWork.Students.Remove(student);
            _unitOfWork.Complete();
            return RedirectToAction("Index", new {groupId = student.GroupId});
        }
    }
}
