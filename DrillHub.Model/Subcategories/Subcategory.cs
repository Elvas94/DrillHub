﻿using System.Collections.Generic;
using DrillHub.Infrastructure;
using DrillHub.Model.Products;

namespace DrillHub.Model.Subcategories
{
    public class Subcategory: IAggregateRoot<int>
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public IList<Product> Products { get; set; }
    }
}
