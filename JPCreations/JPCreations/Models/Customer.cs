using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JPCreations.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="First Name")]
        public string FirstName {get; set;}
        [Display(Name="Last Name")]
        public string LastName { get; set; }
    
        [Display(Name="Street Address")]
        
        public string StreetAddress { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name ="State")]
        [StringLength(2, ErrorMessage = "The State must be the abrivation.", MinimumLength = 2)]
        public string State { get; set; }
        [Display(Name ="Zip Code")]
        [StringLength(5,ErrorMessage ="Must be a valid Zip Code.", MinimumLength = 5)]
        [DataType(DataType.PostalCode)]
        public string ZipCode { get; set; }
        [ForeignKey("ApplicationUser")]
        public string ApplicationId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public CustomerCart customerCart = new CustomerCart();
        
    }
}