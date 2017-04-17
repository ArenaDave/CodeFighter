using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighter.Data
{
    public class ShipHullPartCountData
    {
        [Key]
        public int Id { get; set; }
        public int ShipHullDataId { get; set; }
        public ShipHullData ShipHullData { get; set; }
        public int PartCountDataId { get; set; }
        public PartCountData PartCountData { get; set; }
    }
}
