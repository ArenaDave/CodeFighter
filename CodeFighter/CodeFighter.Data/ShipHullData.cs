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
        public string HullSize { get; set; }
        public int MaxHitPoints { get; set; }

        //public ICollection<ShipData> ShipData { get; set; }
        public ICollection<ShipHullPartCountData> PartCounts { get; set; }
    }
}
