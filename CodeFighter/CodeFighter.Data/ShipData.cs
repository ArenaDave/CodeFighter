using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighter.Data
{
    public class ShipData
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public int ShipHullDataId { get; set; }
        public ShipHullData ShipHull { get; set; }

        public ICollection<ShipPartData> Parts { get; set; }

        public ICollection<ScenarioShipData> ScenarioShipData { get; set; }
    }
}
