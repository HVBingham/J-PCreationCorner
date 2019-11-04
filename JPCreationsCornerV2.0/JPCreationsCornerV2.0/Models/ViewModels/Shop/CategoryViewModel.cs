using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JPCreationsCornerV2._0.Models.Data;

namespace JPCreationsCornerV2._0.Models.ViewModels.Shop
{
    public class CategoryViewModel
    {
        public CategoryViewModel()
        {
           
        }
        public CategoryViewModel( CategoryDTO row)
        {
            Id = row.Id;
            Name = row.Name;
            Slug = row.Slug;
            Sorting = row.Sorting;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int Sorting { get; set; }
    }

}