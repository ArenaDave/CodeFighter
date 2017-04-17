using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighter.Data
{
    public sealed class CodeFighterContext : DbContext
    {
        public CodeFighterContext() : base("name=CodeFighterDB")
        {

        }

        public DbSet<PlayerData> PlayerData { get; set; }
        public DbSet<ScenarioData> ScenarioData { get; set; }
        public DbSet<PlayerScenarioData> PlayerScenarioData { get; set; }
        public DbSet<PartData> PartData { get; set; }
        public DbSet<ShipData> ShipData { get; set; }
        public DbSet<ShipHullData> ShipHullData { get; set; }
        public DbSet<ScenarioShipData> ScenarioShipData { get; set; }
        public DbSet<FeatureData> FeatureData { get; set; }
        public DbSet<ScenarioFeatureData> ScenarioFeatureData { get; set; }
        public DbSet<ActionData> ActionData { get; set; }
        public DbSet<PartCountData> PartCountData { get; set; }
        public DbSet<ShipHullPartCountData> ShipHullPartCountData { get; set; }
    }
}
