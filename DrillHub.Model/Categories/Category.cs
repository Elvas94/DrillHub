using System.Collections.Generic;
using DrillHub.Infrastructure;
using DrillHub.Model.SubCategories;

namespace DrillHub.Model.Categories
{
    public class Category: IAggregateRoot<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<SubCategory> SubCategories { get; set; }
    }
}
