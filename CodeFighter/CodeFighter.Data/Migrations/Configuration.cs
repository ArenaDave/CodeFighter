namespace CodeFighter.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CodeFighter.Data.CodeFighterContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(CodeFighter.Data.CodeFighterContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            #region ACTIONS
            var repairPart = new ActionData() { Id = 1, Type = "RepairPart", Name = "Sheild Regeneration", Description = "Shields regenerate 2HP each round", TargetSelfPart = true, ActionValuesJSON = "{\"RepairAmount\":2}" };
            var repairShip = new ActionData() { Id = 2, Type = "RepairShip", Name = "Damage Control Teams", Description = "Damage Control teams repair the ship for 5HP per round", TargetSelfShip = true, ActionValuesJSON = "{\"RepairAmount\":5}" };

            context.ActionData.AddOrUpdate(x => x.Id,
                repairPart,
                repairShip
                );
            #endregion

            #region PARTS
            // weapons
            //** laser beam
            var laserBeam = new PartData() { Id = 1, Type = "Weapon", Name = "Laser Beam", Description = "Freaking Lasers", MaxHP = 1, DamageType = "Energy", FiringType = "Beam" };
            //** plasma cannon
            var plasmaCannon = new PartData() { Id = 2, Type = "Weapon", Name = "Plasma Cannon", Description = "BURNNNNN!", MaxHP = 1, DamageType = "Plasma", FiringType = "Cannon" };
            //** torpedo
            var torpedoLauncher = new PartData() { Id = 3, Type = "Weapon", Name = "Torpedo Launcher", Description = "All Tubes, Fire!", MaxHP = 1, DamageType = "Explosive", FiringType = "Launcher" };

            // defenses
            //** shield generator
            var shieldGenerator = new PartData() { Id = 4, Type = "Defense", Name = "Shield Generator", Description = "Regenerative Shielding", MaxHP = 15, DefenseType= "Shield", ActionData = new List<ActionData>() { repairPart } };
            //** armor plate
            var armorPlate = new PartData() { Id = 5, Type = "Defense", Name = "Armor Plating", Description = "Reactive Armor", MaxHP = 15, DefenseType="Armor" };
            //** point defense
            var pointDefense = new PartData() { Id = 8, Type = "Defense", Name = "Point Defense Pod", Description = "Close In Weapons System", MaxHP = 15, DefenseType = "PointDefense" };

            // actions
            //** damage control
            var damageControl = new PartData() { Id = 6, Type = "Action", Name = "Damage Control", Description = "Repair 5HP per round, 50% chance to repair a destroyed part", MaxHP = 1, ActionData = new List<ActionData>() { repairShip } };

            // engines
            //** thruster
            var thruster = new PartData() { Id = 7, Type = "Engine", Name = "DU-1 Thruster", Description = "Make It Go Faster!", MaxHP = 1 };

            context.PartData.AddOrUpdate(x => x.Id,
                laserBeam,
                plasmaCannon,
                torpedoLauncher,
                shieldGenerator,
                armorPlate,
                pointDefense,
                damageControl,
                thruster
            );
            #endregion

            #region SHIPHULLS
            var hullIowa = new ShipHullData() { Id = 1, ClassName = "Iowa", HullSize = "CO", MaxHitPoints=40 };
            var hullBunker = new ShipHullData() { Id = 2, ClassName = "Bunker", HullSize = "CO", MaxHitPoints=40 };

            context.ShipHullData.AddOrUpdate(x => x.Id,
                hullIowa,
                hullBunker
                );
            #endregion

            #region SHIPS
            var missouri = new ShipData()
            {
                Id = 1,
                Name = "UNSC Missouri",
                ShipHull = hullIowa

            };
            missouri.Parts = new List<ShipPartData>()
                {
                    new ShipPartData() { Id=1, PartData = laserBeam, ShipData=missouri },
                    new ShipPartData() { Id=2, PartData = laserBeam, ShipData=missouri },
                    new ShipPartData() { Id=3, PartData = plasmaCannon, ShipData=missouri },
                    new ShipPartData() { Id=4, PartData = torpedoLauncher, ShipData=missouri },
                    new ShipPartData() { Id=5, PartData = shieldGenerator, ShipData=missouri },
                    new ShipPartData() { Id=6, PartData = shieldGenerator, ShipData=missouri },
                    new ShipPartData() { Id=7, PartData = armorPlate, ShipData=missouri },
                    new ShipPartData() { Id=8, PartData = damageControl, ShipData=missouri },
                    new ShipPartData() { Id=9, PartData = thruster, ShipData=missouri },
                    new ShipPartData() { Id=10, PartData = thruster, ShipData=missouri },
                    new ShipPartData() { Id=11, PartData = thruster, ShipData=missouri },
                    new ShipPartData() { Id=12, PartData = thruster, ShipData=missouri },
                    new ShipPartData() { Id=13, PartData = thruster, ShipData=missouri },
                    new ShipPartData() { Id=14, PartData = thruster, ShipData=missouri }
                };
            var alpha = new ShipData()
            {
                Id = 2,
                Name = "Alpha-01",
                ShipHull = hullBunker
            };
            alpha.Parts = new List<ShipPartData>()
                {
                    new ShipPartData() { Id=15, PartData = laserBeam, ShipData=alpha },
                    new ShipPartData() { Id=16, PartData = laserBeam, ShipData=alpha },
                    new ShipPartData() { Id=17, PartData = plasmaCannon, ShipData=alpha },
                    new ShipPartData() { Id=18, PartData = torpedoLauncher, ShipData=alpha },
                    new ShipPartData() { Id=19, PartData = shieldGenerator, ShipData=alpha },
                    new ShipPartData() { Id=20, PartData = shieldGenerator, ShipData=alpha },
                    new ShipPartData() { Id=21, PartData = armorPlate, ShipData=alpha },
                    new ShipPartData() { Id=22, PartData = damageControl, ShipData=alpha },
                    new ShipPartData() { Id=23, PartData = thruster, ShipData=alpha },
                    new ShipPartData() { Id=24, PartData = thruster, ShipData=alpha },
                    new ShipPartData() { Id=25, PartData = thruster, ShipData=alpha },
                    new ShipPartData() { Id=26, PartData = thruster, ShipData=alpha },
                    new ShipPartData() { Id=27, PartData = thruster, ShipData=alpha },
                    new ShipPartData() { Id=28, PartData = thruster, ShipData=alpha }
                };

            context.ShipData.AddOrUpdate(x => x.Id,
                missouri,
                alpha
            );
            #endregion

            #region FEATURES
            var asteroid = new FeatureData() { Id = 1, Name = "Asteroid", IsBlocking = true, Type = "asteroid" };

            context.FeatureData.AddOrUpdate(x => x.Id,
                asteroid);
            #endregion

            #region SCENARIOS
            var scenario = new ScenarioData() { Id = 1, ScenarioGUID = new Guid("508E62F3-5B0E-4716-8B8D-FBCA000317A2"), Name = "One on One", Description = "1v1 fight to the death in an asteroid field", RoundLimit = 30 };

            context.ScenarioData.AddOrUpdate(x => x.Id,
                scenario
            );
            #endregion

            #region SCENARIOSHIPS
            context.ScenarioShipData.AddOrUpdate(x => x.Id,
                new ScenarioShipData() { Id = 1, StartingPositionX = 5, StartingPositionY = 5, ShipName = "UNSC Missouri", IsPlayer = true, Ship = missouri, Scenario = scenario },
                new ScenarioShipData() { Id = 2, StartingPositionX = 20, StartingPositionY = 20, ShipName = "Alpha-01", IsPlayer = false, Ship = alpha, Scenario = scenario }
                );
            #endregion

            #region SCENARIOFEATURES
            context.ScenarioFeatureData.AddOrUpdate(x => x.Id,
                new ScenarioFeatureData() { Id = 1, PositionX = 0, PositionY = 0, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 2, PositionX = 0, PositionY = 2, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 3, PositionX = 0, PositionY = 4, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 4, PositionX = 0, PositionY = 22, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 5, PositionX = 1, PositionY = 10, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 6, PositionX = 1, PositionY = 11, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 7, PositionX = 2, PositionY = 17, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 8, PositionX = 2, PositionY = 20, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 9, PositionX = 2, PositionY = 22, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 10, PositionX = 3, PositionY = 1, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 11, PositionX = 3, PositionY = 7, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 12, PositionX = 4, PositionY = 1, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 13, PositionX = 4, PositionY = 2, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 14, PositionX = 4, PositionY = 4, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 15, PositionX = 5, PositionY = 0, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 16, PositionX = 5, PositionY = 6, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 17, PositionX = 5, PositionY = 13, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 18, PositionX = 6, PositionY = 0, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 19, PositionX = 7, PositionY = 1, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 20, PositionX = 7, PositionY = 5, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 21, PositionX = 7, PositionY = 11, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 22, PositionX = 7, PositionY = 20, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 23, PositionX = 8, PositionY = 1, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 24, PositionX = 8, PositionY = 2, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 25, PositionX = 8, PositionY = 16, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 26, PositionX = 9, PositionY = 2, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 27, PositionX = 9, PositionY = 7, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 28, PositionX = 9, PositionY = 18, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 29, PositionX = 10, PositionY = 5, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 30, PositionX = 10, PositionY = 8, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 31, PositionX = 10, PositionY = 15, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 32, PositionX = 10, PositionY = 18, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 33, PositionX = 10, PositionY = 19, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 34, PositionX = 11, PositionY = 5, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 35, PositionX = 12, PositionY = 0, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 36, PositionX = 12, PositionY = 5, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 37, PositionX = 12, PositionY = 10, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 38, PositionX = 12, PositionY = 11, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 39, PositionX = 13, PositionY = 3, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 40, PositionX = 14, PositionY = 23, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 41, PositionX = 15, PositionY = 0, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 42, PositionX = 15, PositionY = 2, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 43, PositionX = 15, PositionY = 7, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 44, PositionX = 15, PositionY = 14, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 45, PositionX = 16, PositionY = 7, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 46, PositionX = 17, PositionY = 6, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 47, PositionX = 17, PositionY = 9, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 48, PositionX = 17, PositionY = 15, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 49, PositionX = 18, PositionY = 15, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 50, PositionX = 18, PositionY = 23, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 51, PositionX = 19, PositionY = 5, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 52, PositionX = 19, PositionY = 6, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 53, PositionX = 19, PositionY = 9, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 54, PositionX = 19, PositionY = 10, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 55, PositionX = 19, PositionY = 21, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 56, PositionX = 20, PositionY = 0, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 57, PositionX = 20, PositionY = 10, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 58, PositionX = 20, PositionY = 24, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 59, PositionX = 21, PositionY = 11, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 60, PositionX = 21, PositionY = 18, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 61, PositionX = 22, PositionY = 2, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 62, PositionX = 22, PositionY = 3, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 63, PositionX = 22, PositionY = 23, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 64, PositionX = 22, PositionY = 24, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 65, PositionX = 23, PositionY = 4, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 66, PositionX = 23, PositionY = 13, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 67, PositionX = 23, PositionY = 16, Feature = asteroid, Scenario = scenario },
                new ScenarioFeatureData() { Id = 68, PositionX = 24, PositionY = 3, Feature = asteroid, Scenario = scenario }
                );
            #endregion

            #region PLAYERS
            context.PlayerData.AddOrUpdate(x => x.Id,
                new PlayerData() { Id = 1, PlayerGUID = new Guid("550D672D-F40A-4A13-9212-DEB4CFE27F2D"), Name = "ArenaDave" });
            #endregion
        }
    }
}
