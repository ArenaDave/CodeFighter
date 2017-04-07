using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighter.Data
{
    public class ScenarioData
    {
        [Key]
        public int Id { get; set; }
        public Guid ScenarioGUID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<ScenarioShipData> Ships { get; set; }
        public ICollection<ScenarioFeatureData> Features { get; set; }
        public int RoundLimit { get; set; }
    }
}
