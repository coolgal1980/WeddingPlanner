using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class Wedding 
    {
        [Key]
        public int wedding_id {get;set;}
        public int user_id {get;set;}
        public User Host {get;set;}
        public string wedder_one {get;set;}
        public string wedder_two {get;set;}

        public DateTime wedding_date {get;set;}

        public string wedding_address {get;set;}

        [DataType(DataType.Date)]
        public DateTime created_at { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime updated_at { get; set; }


        public List<Guest> WGuest {get;set;}

        public Wedding()
        {
            WGuest = new List<Guest>();
            created_at = DateTime.Now;
            updated_at = DateTime.Now;
        }
    }

    public class AddWeddingData 
    {
        public int user_id {get;set;}
        [Required(ErrorMessage="Wedder One is required")]
        [MinLength(3, ErrorMessage="Min length of 3")]
        [MaxLength(40, ErrorMessage="Max length of 40")]
        [Display(Name="Wedder One")]
        public string wedder_one {get;set;}

        [Required(ErrorMessage="Wedder Two is required")]
        [MinLength(3, ErrorMessage="Min length of 3")]
        [MaxLength(40, ErrorMessage="Max length of 40")]
        [Display(Name="Wedder Two")]
        public string wedder_two {get;set;}

        [Required(ErrorMessage="Event Date is required")]
        [Display(Name="Event Date")]
        [DataType(DataType.Date)]
        public DateTime event_date {get;set;}

        [Required(ErrorMessage="Address is required")]
        [MinLength(3, ErrorMessage="Min length of 3")]
        [MaxLength(40, ErrorMessage="Max length of 40")]
        [Display(Name="Address")]
        public string address {get;set;}
        public Wedding TheWedding()
        {
            Wedding newWedding = new Wedding
            {
                user_id = this.user_id,
                wedder_one = this.wedder_one,
                wedder_two = this.wedder_two,
                wedding_date = this.event_date,
                wedding_address = this.address
            };
            return newWedding;
        }
    }
}