using System;
using System.Collections.Generic;

#nullable disable

namespace AdventureWorksLT_DA.Entities
{
    public partial class VGetAllCategories
    {
        public string ParentProductCategoryName { get; set; }
        public string ProductCategoryName { get; set; }
        public int? ProductCategoryId { get; set; }
    }
}
