using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bitcubeeval.Models
{
    public class Image
    {
        [Key]
        public int ImageId { get; set; }
        public byte[] ImageByte { get; set; }
    }
}