using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bitcubeeval.Models
{
    public class Friends
    {
        [Key]
        public int id { get; set; }
        
        public string FriendName { get; set; }
        public string Gender { get; set; }
        public string Cell { get; set; }
        public string description { get; set; }

        [Display(Name = " Picture ")]
        public byte[] pic { get; set; }
    }
}