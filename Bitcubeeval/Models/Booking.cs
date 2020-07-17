using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bitcubeeval.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; } 
        public DateTime DateCreated { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "datetime2")]
        public DateTime EventDate { get; set; }
        [DataType(DataType.Time)]
        [Column(TypeName = "datetime2")]
        public DateTime StartTime { get; set; }
        public double BookingDiscount { get; set; }
        public double DepositFee { get; set; }
        public int NumOfPeople { get; set; }
        public double TotalCost { get; set; }
        public string UserId { get; set; }
        [Column(TypeName = "datetime2")]
        [Display(Name ="Duration(hrs:)")]
        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }
        public int LocationId { get; set; }
        [ForeignKey("LocationId")]
        public virtual Location Location { get; set; }
        public IList<string> SelectedFruits { get; set; }
        public IList<FoodItem> FoodItems { get; set; }
        public IList<string> SelectedItems { get; set; }
        public IList<Item> Items { get; set; }
        public Booking()
        {
            SelectedFruits = new List<string>();
            FoodItems = new List<FoodItem>();
            SelectedItems = new List<string>();
            Items = new List<Item>();
            this.DateCreated = DateTime.Now;
        }

    }
    public class FoodItem
    {
        [Key]
        public int Fid { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }


    }
}