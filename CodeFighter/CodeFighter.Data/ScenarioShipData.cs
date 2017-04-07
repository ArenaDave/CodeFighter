using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighter.Data
{
    public class ScenarioShipData
    {
        [Key]
        public int Id { get; set; }
        public int StartingPositionX { get; set; }
        public int StartingPositionY { get; set; }
        public bool IsPlayer { get; set; }
        public string ShipName { get; set; }

        public int ScenarioDataId { get; set; }
        public ScenarioData Scenario { get; set; }

        public int ShipDataId { get; set; }
        public ShipData Ship { get; set; }


    }
}
