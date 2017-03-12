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
    internal class Simulation
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
            currentPlayer = player;



            // features
            // TODO: load features scenario

            // ships
            // TODO: load from scenario
            Ships.Add(getShip(currentPlayer, 0, "Iowa", "BB", 150, "UNSC Missouri", new Point(5, 5)));
            Ships.Add(getShip(enemyPlayer, 1, "Bunker", "BB", 150, "Alpha-01", new Point(5, 5)));
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

            return ship;
        }

        public List<Animation> Run()
        {
            // reset animations
            results = new List<Animation>();

            bool continueRunning = true;
            int roundCounter = 1;

            // loop through rounds
            while (continueRunning)
            {
                // loop through all initiatives
                for (int initiative = 50; initiative > 0; initiative--)
                {
                    // get ships at this initiative
                    List<Ship> shipsAtInitiative = Ships.Where(x => x.Initiative == initiative).ToList();
                    List<Ship> shipsNotAtInitiative = Ships.Where(x => x.Initiative != initiative).ToList();
                    // shuffle the list
                    shipsAtInitiative.Shuffle();
                    // loop until no ships at this initiative have MP remaining.
                    while (shipsAtInitiative.Any(x => x.MP.Current > 0))
                    {
                        // loop through each ship at this initiative that still have MP
                        foreach (Ship currentShip in shipsAtInitiative.Where(x => x.MP.Current > 0))
                        {
                            // TODO: activate AI
                            List<BaseOrder> orders = currentShip.GameLogic.GetOrders(Ships.Clone(), Features.Clone());

                            // hookup handlers
                            currentShip.OnShipDestroyed -= new ShipDestroyedEvent(ShipDestroyedHandler);
                            currentShip.OnShipDestroyed += new ShipDestroyedEvent(ShipDestroyedHandler);
                            foreach (FireWeaponOrder fwo in orders.Where(x => x is FireWeaponOrder))
                            {
                                fwo.OnWeaponFired -= new WeaponFiredEvent(WeaponFiredHandler);
                                fwo.OnWeaponFired += new WeaponFiredEvent(WeaponFiredHandler);

                                fwo.OnMessageResult -= new MessageEvent(MessageHandler);
                                fwo.OnMessageResult += new MessageEvent(MessageHandler);
                            }

                            // resolve orders
                            foreach (BaseOrder order in orders)
                            {
                                order.Simulation = this;
                                order.ExecuteOrder();
                            }

                            // finally, subtract 1 MP
                            currentShip.MP.Reduce(1);
                        }
                        // END OF SHIPS WITH MP
                        // loop through other ships, checking for Point Defense weapons
                        foreach (Ship currentShip in shipsNotAtInitiative.Where(x => x.Parts.Any(p => p is WeaponPart && ((WeaponPart)p).IsPointDefense && !((WeaponPart)p).HasFiredThisRound)))
                        {
                            // TODO: activate AI code to check for Point Defense weapon actions
                            List<BaseOrder> orders = currentShip.GameLogic.GetOrders(Ships.Clone(), Features.Clone());

                            // hookup handlers
                            currentShip.OnShipDestroyed -= new ShipDestroyedEvent(ShipDestroyedHandler);
                            currentShip.OnShipDestroyed += new ShipDestroyedEvent(ShipDestroyedHandler);
                            foreach (FireWeaponOrder fwo in orders.Where(x => x is FireWeaponOrder))
                            {
                                fwo.OnWeaponFired -= new WeaponFiredEvent(WeaponFiredHandler);
                                fwo.OnWeaponFired += new WeaponFiredEvent(WeaponFiredHandler);

                                fwo.OnMessageResult -= new MessageEvent(MessageHandler);
                                fwo.OnMessageResult += new MessageEvent(MessageHandler);
                            }
                            // resolve orders
                            foreach (FireWeaponOrder fwo in orders.Where(x => x is FireWeaponOrder && ((FireWeaponOrder)x).WeaponToFire.IsPointDefense))
                            {
                                fwo.Simulation = this;
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
                }
            }
            // return animations
            return results;
        }

        public void ShipDestroyedHandler(object sender, ShipEventArgs e)
        {
            results.Add(new Animation(AnimationActionType.Kill, new AnimationKillDetails(e.ShipID)));
        }

        public void WeaponFiredHandler(object sender, WeaponFiredEventArgs e)
        {
            List<AnimationShotDetails> shots = new List<AnimationShotDetails>();
            shots.Add(new AnimationShotDetails(e.ShooterID, e.TargetID, e.IsHit, e.IsCrit));
            results.Add(new Animation(AnimationActionType.Shoot, new AnimationShootingDetails(shots), e.Messages));
        }

        public void MovedHandler(object sender, ShipMovedEventArgs e)
        {
            results.Add(new Animation(AnimationActionType.Move, new AnimationMoveDetails(e.ShipID, e.MovedTo), e.Messages));
        }

        public void MessageHandler(object sender, MessageEventArgs e)
        {
            results.Add(new Animation(AnimationActionType.Message, null, e.Messages));
        }
    }
}
