﻿using System;
using System.Collections.Generic;

#nullable disable

namespace AdventureWorksLT_DA.Entities
{
    public partial class VProductAndDescription
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string ProductModel { get; set; }
        public string Culture { get; set; }
        public string Description { get; set; }
    }
}
