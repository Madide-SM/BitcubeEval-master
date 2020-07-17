using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bitcubeeval.Models
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int QuantityNeeded { get; set; }
        public double Cost { get; set; }
        public double QNCost { get; set; }
    }
}