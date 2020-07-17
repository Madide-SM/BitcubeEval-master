using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bitcubeeval.Models
{
    public class Location
    {
        [Key]
        public int LocationId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public double Cost { get; set; }
        public virtual Status Status { get; set; }
        public virtual PType PType { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
    public enum Status
    {
        Free,
        Occupied,
        Unavailable
    }
    public enum PType
    {
        Indoor,
        Outdoor,
        All
    }
}