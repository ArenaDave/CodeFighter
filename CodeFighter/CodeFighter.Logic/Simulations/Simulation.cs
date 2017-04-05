using CodeFighter.Logic.Actions;
using CodeFighter.Logic.Animations;
using CodeFighter.Logic.Orders;
using CodeFighter.Logic.Parts;
using CodeFighter.Logic.Players;
using CodeFighter.Logic.Scenarios;
using CodeFighter.Logic.Ships;
using CodeFighter.Logic.Utility;
using System.Collections.Generic;
using System.Linq;

namespace CodeFighter.Logic.Simulations
{
    public class Simulation
    {

        Player enemyPlayer;
        Player currentPlayer;
        internal List<ScenarioFeature> Features = new List<ScenarioFeature>();
        internal List<Ship> Ships = new List<Ship>();
        Scenario currentScenario;
        List<Animation> results;

        public Simulation(Scenario scenario, Player player)
        {
            // scenario
            currentScenario = scenario;
            currentScenario.RoundLimit = 30;

            // players
            enemyPlayer = new Player(true);
            enemyPlayer.ID = -1;
            enemyPlayer.Name = "Xorn";
            currentPlayer = player;



            // features
            // TODO: load features scenario

            // ships
            // TODO: load from scenario
            Ships.Add(getShip(currentPlayer, 0, "Iowa", "BB", 40, "UNSC Missouri", new Point(5, 5)));
            Ships.Add(getShip(enemyPlayer, 1, "Bunker", "BB", 40, "Alpha-01", new Point(20, 20)));
            // TODO: add Game Logic to player's ships
        }

        // placeholder until we load from scenarios
        private static Ship getShip(Player currentPlayer, int ID, string hullName, string hullSize, int hp, string name, Point origin)
        {
            ShipHull hull = new ShipHull(hullName, hullSize, hp);
            Ship ship = new Ship(ID, name, currentPlayer, hull, null, origin);

            List<BasePart> partsList = new List<BasePart>();
            // weapons
            partsList.Add(new WeaponPart("Laser Beam", 1, 50, new List<BaseAction>(), 5, 2, DamageType.Energy, FiringType.Beam, 2, 0, true));
            partsList.Add(new WeaponPart("Laser Beam", 1, 50, new List<BaseAction>(), 5, 2, DamageType.Energy, FiringType.Beam, 2, 0, true));
            partsList.Add(new WeaponPart("Plasma Cannon", 1, 50, new List<BaseAction>(), 12, 3, DamageType.Plasma, FiringType.Cannon, 3, 1, false));
            partsList.Add(new WeaponPart("Torpedo", 1, 50, new List<BaseAction>(), 20, 5, DamageType.Explosive, FiringType.Torpedo, 2, 3, true));
            // defenses
            DefensePart shieldGenerator = new DefensePart("Shield Generator", 15, 50, 1, "Down", "Penetrating", new List<BaseAction>());
            shieldGenerator.Actions.Add(new RepairPart(shieldGenerator, 2));
            partsList.Add(shieldGenerator);
            DefensePart shieldGenerator2 = new DefensePart("Shield Generator", 15, 50, 1, "Down", "Penetrating", new List<BaseAction>());
            shieldGenerator2.Actions.Add(new RepairPart(shieldGenerator2, 2));
            partsList.Add(shieldGenerator2);
            partsList.Add(new DefensePart("Armor Plating", 15, 50, 3, "Destroyed", "Shattering", new List<BaseAction>()));
            // actions
            partsList.Add(new ActionPart("Damage Control", 1, 50, "Regen: 5 HPs", new List<BaseAction>() { new RepairShip(ship, 5) }));
            // engines
            partsList.Add(new EnginePart("DU-9 Thruster", 1, 50, new List<BaseAction>(), 250));
            partsList.Add(new EnginePart("DU-9 Thruster", 1, 50, new List<BaseAction>(), 250));
            partsList.Add(new EnginePart("DU-9 Thruster", 1, 50, new List<BaseAction>(), 250));
            partsList.Add(new EnginePart("DU-9 Thruster", 1, 50, new List<BaseAction>(), 250));
            partsList.Add(new EnginePart("DU-9 Thruster", 1, 50, new List<BaseAction>(), 250));
            partsList.Add(new EnginePart("DU-9 Thruster", 1, 50, new List<BaseAction>(), 250));
            ship.Parts = partsList;

            //game logic
            ship.GameLogic = new EnemyLogic();
            ship.Owner = currentPlayer;
            return ship;
        }

        public List<Animation> Run()
        {
            // reset animations
            results = new List<Animation>();
            
            bool continueRunning = true;
            int roundCounter = 1;
            
            // reset all ships
            foreach (Ship ship in Ships)
            {
                // TODO: remove hard-coded size category
                results.Add(new Animation(AnimationActionType.Add, new AnimationAddDetails(ship.ID, ship.Position, ship.Owner.IsAI, 2)));
                results.Add(new Animation(AnimationActionType.ShipUpdate, new AnimationShipUpdateDetails(ship)));
                ship.EndOfTurn();
            }

            // loop through rounds
            while (continueRunning)
            {
                results.Add(new Animation(AnimationActionType.NewRound, null, new List<string>() { string.Format("Round: {0}", roundCounter) }));
                // loop through all initiatives
                for (int initiative = 50; initiative > 0; initiative--)
                {
                    // get ships at this initiative
                    List<Ship> shipsAtInitiative = Ships.Where(x => x.Initiative == initiative && !x.IsDestroyed).ToList();
                    List<Ship> shipsNotAtInitiative = Ships.Where(x => x.Initiative != initiative && !x.IsDestroyed).ToList();
                    // shuffle the list
                    shipsAtInitiative.Shuffle();
                    // loop until no ships at this initiative have MP remaining.
                    while (shipsAtInitiative.Any(x =>!x.IsDestroyed && x.MP.Current > 0) 
                        && Ships.Any(x => !x.IsDestroyed && x.Owner == currentPlayer)
                        && Ships.Any(x => !x.IsDestroyed && x.Owner == enemyPlayer))
                    {
                        // loop through each ship at this initiative that still have MP
                        foreach (Ship currentShip in shipsAtInitiative.Where(x => x.MP.Current > 0 && !x.IsDestroyed))
                        {
                            // TODO: activate AI
                            Ship currentShipRef = currentShip;
                            List<BaseOrder> orders = currentShip.GameLogic.GetOrders(currentShipRef, Ships.Clone(), Features.Clone());

                            // hookup handlers
                            foreach (FireWeaponOrder fwo in orders.Where(x => x is FireWeaponOrder))
                            {
                                fwo.OnWeaponFired -= new WeaponFiredEvent(WeaponFiredHandler);
                                fwo.OnWeaponFired += new WeaponFiredEvent(WeaponFiredHandler);

                                fwo.OnTargetDestroyed -= new ShipDestroyedEvent(ShipDestroyedHandler);
                                fwo.OnTargetDestroyed += new ShipDestroyedEvent(ShipDestroyedHandler);

                                fwo.OnMessageResult -= new MessageEvent(MessageHandler);
                                fwo.OnMessageResult += new MessageEvent(MessageHandler);
                            }
                            foreach(MoveOrder mo in orders.Where(x=>x is MoveOrder))
                            {
                                mo.OnShipMoved -= new ShipMovedEvent(MovedHandler);
                                mo.OnShipMoved += new ShipMovedEvent(MovedHandler);

                                mo.OnMessageResult -= new MessageEvent(MessageHandler);
                                mo.OnMessageResult += new MessageEvent(MessageHandler);
                            }

                            // resolve orders
                            foreach (BaseOrder order in orders)
                            {
                                if (!Ships.Any(x => !x.IsDestroyed && x.Owner == currentPlayer) || !Ships.Any(x => !x.IsDestroyed && x.Owner == enemyPlayer))
                                    continue;
                                order.Simulation = this;
                                if (Ships.First(x => x.ID == order.CurrentShip.ID).IsDestroyed)
                                    continue;
                                order.CurrentShip = Ships.First(x => x.ID == order.CurrentShip.ID);
                                if(order is FireWeaponOrder)
                                {
                                    FireWeaponOrder fwo = (FireWeaponOrder)order;
                                    if (Ships.First(x => x.ID == fwo.TargetShip.ID).IsDestroyed)
                                        continue;
                                    fwo.TargetShip = Ships.First(x => x.ID == fwo.TargetShip.ID);
                                }
                                order.ExecuteOrder();
                            }

                            // finally, subtract 1 MP
                            currentShip.MP.Reduce(1);
                        }
                        // END OF SHIPS WITH MP
                        // loop through other ships, checking for Point Defense weapons
                        foreach (Ship currentShip in shipsNotAtInitiative.Where(x => !x.IsDestroyed && x.Parts.Any(p => p is WeaponPart && !p.IsDestroyed && ((WeaponPart)p).IsPointDefense && !((WeaponPart)p).HasFiredThisRound)))
                        {
                            // TODO: activate AI code to check for Point Defense weapon actions
                            Ship currentShipRef = currentShip;
                            List<BaseOrder> orders = currentShip.GameLogic.GetOrders(currentShipRef, Ships.Clone(), Features.Clone());

                            // hookup handlers
                            foreach (FireWeaponOrder fwo in orders.Where(x => x is FireWeaponOrder))
                            {
                                fwo.OnWeaponFired -= new WeaponFiredEvent(WeaponFiredHandler);
                                fwo.OnWeaponFired += new WeaponFiredEvent(WeaponFiredHandler);

                                fwo.OnTargetDestroyed -= new ShipDestroyedEvent(ShipDestroyedHandler);
                                fwo.OnTargetDestroyed += new ShipDestroyedEvent(ShipDestroyedHandler);

                                fwo.OnMessageResult -= new MessageEvent(MessageHandler);
                                fwo.OnMessageResult += new MessageEvent(MessageHandler);
                            }
                            // resolve orders
                            foreach (FireWeaponOrder fwo in orders.Where(x => x is FireWeaponOrder && ((FireWeaponOrder)x).WeaponToFire.IsPointDefense))
                            {
                                fwo.Simulation = this;
                                fwo.CurrentShip = Ships.First(x => x.ID == fwo.CurrentShip.ID);
                                fwo.TargetShip = Ships.First(x => x.ID == fwo.TargetShip.ID);
                                fwo.ExecuteOrder();
                            }
                        }

                    }
                    // END OF INITIATIVE
                }



                // END OF ROUND
                // check if either player is defeated
                if (!Ships.Any(x => !x.IsDestroyed && x.Owner == currentPlayer))
                {
                    currentPlayer.IsDefeated = true;
                }
                if (!Ships.Any(x => !x.IsDestroyed && x.Owner == enemyPlayer))
                {
                    enemyPlayer.IsDefeated = true;
                }
                // check if loop should end
                if (enemyPlayer.IsDefeated || currentPlayer.IsDefeated)
                {
                    continueRunning = false;
                }
                else if (roundCounter >= currentScenario.RoundLimit)
                {
                    continueRunning = false;
                }
                else
                {
                    roundCounter++;
                    foreach(Ship ship in Ships)
                    {
                        results.Add(ship.EndOfTurn());
                    }
                }
            }
            // return animations
            return results;
        }

        public void ShipDestroyedHandler(object sender, ShipEventArgs e)
        {
            results.Add(new Animation(AnimationActionType.Kill, new AnimationKillDetails(e.ShipID)));
            results.Add(new Animation(AnimationActionType.Message, null, new List<string>() { "*** "+Ships.First(x => x.ID == e.ShipID).Name + " Is Destroyed! ***" }));
            results.Add(new Animation(AnimationActionType.ShipUpdate, new AnimationShipUpdateDetails(Ships.First(x => x.ID == e.ShipID))));
        }

        public void WeaponFiredHandler(object sender, WeaponFiredEventArgs e)
        {
            List<AnimationShotDetails> shots = new List<AnimationShotDetails>();
            shots.Add(new AnimationShotDetails(e.ShooterID, e.TargetID, e.IsHit, e.IsCrit));
            results.Add(new Animation(AnimationActionType.Shoot, new AnimationShootingDetails(shots), e.Messages));
            results.Add(new Animation(AnimationActionType.ShipUpdate, new AnimationShipUpdateDetails(Ships.First(x => x.ID == e.ShooterID))));
            results.Add(new Animation(AnimationActionType.ShipUpdate, new AnimationShipUpdateDetails(Ships.First(x => x.ID == e.TargetID))));
        }

        public void MovedHandler(object sender, ShipMovedEventArgs e)
        {
            results.Add(new Animation(AnimationActionType.Move, new AnimationMoveDetails(e.ShipID, e.MovedTo), e.Messages));
            results.Add(new Animation(AnimationActionType.ShipUpdate, new AnimationShipUpdateDetails(Ships.First(x => x.ID == e.ShipID))));
        }

        public void MessageHandler(object sender, MessageEventArgs e)
        {
            results.Add(new Animation(AnimationActionType.Message, null, e.Messages));
        }
    }
}
