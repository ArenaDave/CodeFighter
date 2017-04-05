using System;
using System.Collections.Generic;

namespace CodeFighter.Logic.Scenarios
{
    public class Scenario
    {
        public int ID { get; set; }
        public Guid ScenarioID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ScenarioShip> Ships { get; set; }
        public List<Feature> Features { get; set; }
        public int RoundLimit { get; set; }
    }

    

    
}
