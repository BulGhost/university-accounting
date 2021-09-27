using UniversityAccounting.DAL.Interfaces;

namespace UniversityAccounting.WEB.Controllers.HelperClasses
{
    public interface ISortModel
    {
        string SortProperty { get; set; }
        SortOrder SortOrder { get; set; }
        void AddColumn(string colName, bool isDefaultColumn = false);
        SortableColumn GetColumn(string colName);
        void ApplySort(string sortProperty, SortOrder sortOrder);
    }
}