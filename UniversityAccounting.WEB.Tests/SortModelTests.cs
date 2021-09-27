using System;
using Xunit;
using FluentAssertions;
using UniversityAccounting.DAL.Interfaces;
using UniversityAccounting.WEB.Controllers.HelperClasses;

namespace UniversityAccounting.WEB.Tests
{
    public class SortModelTests
    {
        private readonly SortModel _sortModel = new();

        [Fact]
        public void AddColumn_SeveralColumnAddition_FirstColumnSetsAsDefault()
        {
            _sortModel.AddColumn("name1");
            _sortModel.AddColumn("name2");
            _sortModel.AddColumn("name3");

            _sortModel.SortProperty.Should().Be("name1");
            _sortModel.SortOrder.Should().Be(SortOrder.Ascending);
        }


        [Fact]
        public void AddColumn_SetDefaultColumn_RequiredColumnSetsAsDefault()
        {
            const string firstColumnName = "name1";
            const string secondColumnName = "name2";

            _sortModel.AddColumn(firstColumnName);
            _sortModel.AddColumn(secondColumnName, true);

            _sortModel.SortProperty.Should().Be(secondColumnName);
            _sortModel.SortOrder.Should().Be(SortOrder.Ascending);
        }

        [InlineData(null)]
        [InlineData("")]
        [InlineData("name5")]
        [Theory]
        public void GetColumn_TryToGetNonexistentColumn_ArgumentException(string colName)
        {
            AddColumnsForTest();

            Assert.ThrowsAny<ArgumentException>(() => _sortModel.GetColumn(colName));
        }

        [Fact]
        public void GetColumn_GetExistingColumn_GetRequiredColumn()
        {
            AddColumnsForTest();

            var column = _sortModel.GetColumn("name2");

            column.ColumnName.Should().Be("name2");
            column.Order.Should().Be(SortOrder.Ascending);
        }

        [Fact]
        public void ApplySort_CorrectSortPropertyName_CustomSortPropertyAndOrderIsSet()
        {
            AddColumnsForTest();

            _sortModel.ApplySort("name3", SortOrder.Descending);

            _sortModel.SortProperty.Should().Be("name3");
            _sortModel.SortOrder.Should().Be(SortOrder.Descending);
        }

        [Fact]
        public void ApplySort_IncorrectSortPropertyName_SortPropertyAndOrderIsSetByDefault()
        {
            AddColumnsForTest();

            _sortModel.ApplySort("name4", SortOrder.Descending);

            _sortModel.SortProperty.Should().Be("name2");
            _sortModel.SortOrder.Should().Be(SortOrder.Ascending);
        }

        [Fact]
        public void ApplySort_ChangeColumnSortOrder_SwitchSortOrderAndIcon()
        {
            AddColumnsForTest();

            _sortModel.ApplySort("name3", SortOrder.Descending);

            _sortModel.GetColumn("name3").ColumnName.Should().Be("name3");
            _sortModel.GetColumn("name3").Order.Should().Be(SortOrder.Ascending);
            _sortModel.GetColumn("name3").SortIcon.Should().Be("fas fa-arrow-up");
        }

        [Fact]
        public void ApplySort_ChangeColumnForSorting_NewSortColumnWithAscendingSortOrder()
        {
            AddColumnsForTest();

            _sortModel.ApplySort("name3", SortOrder.Ascending);

            _sortModel.GetColumn("name1").ColumnName.Should().Be("name1");
            _sortModel.GetColumn("name1").Order.Should().Be(SortOrder.Ascending);
        }

        private void AddColumnsForTest()
        {
            _sortModel.AddColumn("name1");
            _sortModel.AddColumn("name2", true);
            _sortModel.AddColumn("name3");
        }
    }
}
