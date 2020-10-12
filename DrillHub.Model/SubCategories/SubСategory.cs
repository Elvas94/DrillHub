using System.Collections.Generic;
using DrillHub.Infrastructure;
using DrillHub.Model.Products;

namespace DrillHub.Model.SubCategories
{
    public class SubCategory : IAggregateRoot<int>
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public bool IsCategory { get; set; }
        public virtual IList<Product> Products { get; set; }
    }
}
