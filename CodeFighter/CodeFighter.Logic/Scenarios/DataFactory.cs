using CodeFighter.Data;
using CodeFighter.Logic.Actions;
using CodeFighter.Logic.Parts;
using CodeFighter.Logic.Players;
using CodeFighter.Logic.Ships;
using CodeFighter.Logic.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighter.Logic.Scenarios
{
    public static class DataFactory
    {
        public static Scenario GetScenario(Guid scenarioGuid, Player currentPlayer, Player enemyPlayer)
        {
            Scenario result = new Scenario();
            using (CodeFighterContext db = new CodeFighterContext())
            {
                ScenarioData scenario = db.ScenarioData
                    .Include("Features").Include("Features.Feature")
                    .Include("Ships").Include("Ships.Ship")
                    .Include("Ships.Ship.ShipHull").Include("Ships.Ship.Parts")
                    .Include("Ships.Ship.Parts.PartData").Include("Ships.Ship.Parts.PartData.ActionData")
                    .FirstOrDefault(x => x.ScenarioGUID.Equals(scenarioGuid));
                if (scenario != null)
                {
                    // scenario data
                    result = new Scenario(scenario);

                    // features
                    foreach (ScenarioFeatureData fd in scenario.Features)
                    {
                        result.Features.Add(new Feature(fd));
                    }

                    // ships
                    foreach (ScenarioShipData sd in scenario.Ships)
                    {
                        Ship ship = new Ship();
                        // create hull
                        ShipHull shipHull = new ShipHull(sd.Ship.ShipHull.ClassName, sd.Ship.ShipHull.HullSize);

                        // create parts
                        List<BasePart> parts = new List<BasePart>();
                        foreach (ShipPartData spd in sd.Ship.Parts)
                        {
                            PartData pd = spd.PartData;
                            List<BaseAction> actions = new List<BaseAction>();
                            BasePart part = null;
                            switch (pd.Type)
                            {
                                case "Weapon":
                                    part = new WeaponPart(pd, shipHull.Size.Classification, actions);
                                    break;
                                case "Defense":
                                    part = new DefensePart(pd, shipHull.Size.Classification, actions);
                                    break;
                                case "Action":
                                    part = new ActionPart(pd, shipHull.Size.Classification, actions);
                                    break;
                                case "Engine":
                                    part = new EnginePart(pd, shipHull.Size.Classification, actions);
                                    break;
                            }

                            foreach (ActionData ad in pd.ActionData)
                            {
                                switch (ad.Type)
                                {
                                    case "RepairPart":
                                        var repairPartActions = JsonConvert.DeserializeObject<Dictionary<string, object>>(ad.ActionValuesJSON);
                                        actions.Add(new RepairPart(part, new CloneableDictionary<string, object>(repairPartActions)));
                                        break;
                                    case "RepairShip":
                                        var repairShipActions = JsonConvert.DeserializeObject<Dictionary<string, object>>(ad.ActionValuesJSON);
                                        actions.Add(new RepairShip(ship, new CloneableDictionary<string, object>(repairShipActions)));
                                        break;
                                }
                            }
                            if (actions.Count > 0)
                                part.Actions = actions;

                            parts.Add(part);
                        }

                        

                        Point origin = new Point(sd.StartingPositionX, sd.StartingPositionY);

                        ship.ID = sd.Id;
                        ship.Name = sd.ShipName;
                        ship.Owner = sd.IsPlayer?currentPlayer:enemyPlayer;
                        ship.Hull = shipHull;
                        ship.Parts = parts;
                        ship.Position = origin;
                        ship.IsDestroyed = false;
                        ship.MP = new StatWithMax(ship.MaxMP);
                        ship.GameLogic = ship.Owner.GameLogic;

                        result.Ships.Add(ship);
                    }

                }
            }


            return result;
        }

        public static Player GetPlayer(Guid playerGuid)
        {
            Player result = new Player();
            using (CodeFighterContext db = new CodeFighterContext())
            {
                PlayerData data = db.PlayerData.FirstOrDefault(x => x.PlayerGUID.Equals(playerGuid));
                if (data != null)
                {
                    result.ID = data.Id;
                    result.PlayerID = data.PlayerGUID;
                    result.Name = data.Name;
                    result.IsDefeated = false;
                    result.IsAI = false;
                    // TODO: store and retrieve player's code
                    result.GameLogic = new EnemyLogic();
                }
            }

            return result;
        }

    }
}
