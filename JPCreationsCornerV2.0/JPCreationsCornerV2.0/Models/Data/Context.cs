﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace JPCreationsCornerV2._0.Models.Data
{
    public class Context: DbContext
    {
        public DbSet<PageDTO> Pages { get; set; }
    }
}