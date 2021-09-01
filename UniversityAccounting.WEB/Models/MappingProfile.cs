using AutoMapper;
using UniversityAccounting.DAL.Entities;

namespace UniversityAccounting.WEB.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Course, CourseViewModel>();
            CreateMap<CourseViewModel, Course>();
            CreateMap<Group, GroupViewModel>()
                .ForMember(x => x.StudentsQuantity, opt => opt.MapFrom(g => g.Students.Count));
            CreateMap<GroupViewModel, Group>();
            CreateMap<StudentViewModel, Student>();
            CreateMap<Student, StudentViewModel>()
                .ForMember(x => x.GroupName, opt => opt.MapFrom(s => s.Group.Name));
        }
    }
}
