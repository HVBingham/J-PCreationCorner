using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JPCreationsCornerV2._0.Models.Data;

namespace JPCreationsCornerV2._0.Models.ViewModels.Pages
{
    public class SideBarViewModel
    {
        
        public int Id  {get;set;}
        [AllowHtml]
        public string Body { get; set; }
        public SideBarViewModel()
        {

        }
        public SideBarViewModel(SidebarDTO row)
        {
            Id = row.Id;
            Body = row.Body;
        }
    }
}