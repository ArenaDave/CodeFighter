using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighter.Data
{
    public class ScenarioFeatureData
    {
        [Key]
        public int Id { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }

        public int ScenarioDataId { get; set; }
        public ScenarioData Scenario { get; set; }

        public int FeatureDataId { get; set; }
        public FeatureData Feature { get; set; }
    }
}
