using CodeFighter.Data;
using CodeFighter.Logic.Ships;
using System;
using System.Collections.Generic;

namespace CodeFighter.Logic.Scenarios
{
    public class Scenario
    {
        public int ID { get; set; }
        public Guid ScenarioGUID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Ship> Ships { get; set; }
        public List<Feature> Features { get; set; }
        public int RoundLimit { get; set; }

        public Scenario() {
            Ships = new List<Ship>();
            Features = new List<Feature>();
        }

        public Scenario(ScenarioData data)
        {
            this.ID = data.Id;
            this.ScenarioGUID = data.ScenarioGUID;
            this.Name = data.Name;
            this.Description = data.Description;
            this.RoundLimit = data.RoundLimit;
            Ships = new List<Ship>();
            Features = new List<Feature>();
        }
    }

    

    
}
