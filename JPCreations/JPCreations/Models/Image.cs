using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JPCreations.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        [Display(Name ="Upload Image")]
        public string ImagePath { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

    }
}