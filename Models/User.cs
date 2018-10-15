using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WeddingPlanner.Models;

namespace WeddingPlanner.Models
{
    public class User
    {
        [Key]
        public int user_id { get; set; }  

        [Required]
        [Display(Name="First Name")]
        [MinLength(2, ErrorMessage="Name fields must be 2 characters or more")]
        [MaxLength(20, ErrorMessage="Name fields must be 20 characters or less")]
        [RegularExpression(@"^[A-Za-z''-'\s]{2,10}$", ErrorMessage="Name fields must only contain letters, apostrophies, and whitespace")]
        public string first_name {get;set;}

        [Required]
        [Display(Name="Last Name")]
        [MinLength(2, ErrorMessage="Name fields must be 2 characters or more")]
        [MaxLength(20, ErrorMessage="Name fields must be 20 characters or less")]
        [RegularExpression(@"^[A-Za-z''-'\s]{2,10}$", ErrorMessage="Name fields must only contain letters, apostrophies, and whitespace")]
        public string last_name {get;set;}

        [Required]
        [EmailAddress]
        [Display(Name="Email Address")]
        public string email {get;set;}

        [Required]
        [DataType(DataType.Password)]
        [Display(Name="Password")]
        public string password {get;set;}

        [DataType(DataType.Password)]
        [Required]
        [Compare("password")]
        [NotMapped]
        [Display(Name="Confirm Password")]
        public string confirm {get;set;}

        [DataType(DataType.Date)]
        public DateTime created_at { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime updated_at { get; set; }

        // public List<BankAccount> BankAccounts { get; set; }
 
        // public User()
        // {
        //     BankAccounts = new List<BankAccount>();
        // }

        // public int Balance {get; set;}


        
        public List<Wedding> Weddings {get;set;}
        public List<Guest> WGuest {get;set;}
        public User()
        {
            Weddings = new List<Wedding>();
            WGuest = new List<Guest>();
            created_at = DateTime.Now;
            updated_at = DateTime.Now;
        }


    }
    public class LoginUser
    {
        [Key]
        public int user_id { get; set; }  
        [Required]
        [Display(Name="Email Address")]
        [EmailAddress]
        public string email {get;set;}
        [Required]
        [Display(Name="Password")]
        [DataType(DataType.Password)]
        public string password {get;set;}

        public int Balance { get; set; }
    }
}