using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace WeddingPlanner.Models
{
    public class Guest
    {
        [Key]
        public int guest_id {get;set;}
        public int wedding_id {get;set;}
        public int user_id {get;set;}
        public User WGuest {get;set;}
        public Wedding Wedding {get;set;}

        [DataType(DataType.Date)]
        public DateTime created_at { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime updated_at { get; set; }

        public Guest()
        {
            created_at = DateTime.Now;
            updated_at = DateTime.Now;
        }
    }
}