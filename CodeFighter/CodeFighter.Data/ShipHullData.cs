using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighter.Data
{
    public class ShipHullData
    {
        [Key]
        public int Id { get; set; }
        public string ClassName { get; set; }
        public int MaxHP { get; set; }
        public string HullSize { get; set; }

        ICollection<ShipData> ShipData { get; set; }
    }
}
