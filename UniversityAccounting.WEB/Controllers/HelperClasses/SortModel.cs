using System;
using System.Collections.Generic;
using System.Linq;
using UniversityAccounting.DAL.Interfaces;

namespace UniversityAccounting.WEB.Controllers.HelperClasses
{
    public class SortableColumn
    {
        public string ColumnName { get; set; }
        public SortOrder Order { get; set; }
        public string SortIcon { get; set; }
    }

    public class SortModel : ISortModel
    {
        private const string UpIcon = "fas fa-arrow-up";
        private const string DownIcon = "fas fa-arrow-down";
        private readonly List<SortableColumn> _sortableColumns = new();

        public string SortProperty { get; set; }
        public SortOrder SortOrder { get; set; }

        public void AddColumn(string colName, bool isDefaultColumn = false)
        {
            if (string.IsNullOrEmpty(colName)) throw new ArgumentNullException(nameof(colName));

            SortableColumn column = _sortableColumns.SingleOrDefault(c =>
                c.ColumnName.ToLower() == colName.ToLower());

            if (column == null) _sortableColumns.Add(new SortableColumn { ColumnName = colName });

            if (isDefaultColumn || _sortableColumns.Count == 1)
            {
                SortProperty = colName;
                SortOrder = SortOrder.Ascending;
            }
        }

        public SortableColumn GetColumn(string colName)
        {
            if (string.IsNullOrEmpty(colName)) throw new ArgumentNullException(nameof(colName));

            SortableColumn column = _sortableColumns.SingleOrDefault(c =>
                c.ColumnName.ToLower() == colName.ToLower());

            if (column == null)
                throw new ArgumentException(@"Unable to sort by column with this name.", nameof(colName));

            return column;
        }

        public void ApplySort(string sortProperty, SortOrder sortOrder)
        {
            if (string.IsNullOrEmpty(sortProperty)) sortProperty = SortProperty;

            sortProperty = sortProperty.ToLower();
            foreach (var sortableColumn in _sortableColumns)
            {
                sortableColumn.SortIcon = string.Empty;
                sortableColumn.Order = SortOrder.Ascending;

                if (sortProperty != sortableColumn.ColumnName.ToLower()) continue;

                SortProperty = sortableColumn.ColumnName;
                SortOrder = sortOrder;
                if (sortOrder == SortOrder.Ascending)
                {
                    sortableColumn.SortIcon = DownIcon;
                    sortableColumn.Order = SortOrder.Descending;
                }
                else
                {
                    sortableColumn.SortIcon = UpIcon;
                    sortableColumn.Order = SortOrder.Ascending;
                }
            }
        }
    }
}
