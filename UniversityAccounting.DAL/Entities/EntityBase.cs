using System.ComponentModel.DataAnnotations;

namespace UniversityAccounting.DAL.Entities
{
    public class EntityBase
    {
        [Key]
        public int Id { get; set; }
    }
}
