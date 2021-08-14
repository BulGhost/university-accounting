using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccounting.Models.ViewModels
{
    public class GroupsIndexViewModel
    {
        public IEnumerable<Group> Groups { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
