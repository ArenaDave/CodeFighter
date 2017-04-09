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
using System;

namespace CodeFighter.Logic.Simulations
{
    public class Simulation
    {

        Player enemyPlayer;
        Player currentPlayer;
        internal List<Feature> Features = new List<Feature>();
        internal List<Ship> Ships = new List<Ship>();
        Scenario currentScenario;
        List<Animation> results;

        public Simulation(Guid scenarioGUID, Guid playerGUID, object playerLogic)
        {
            // get player
            currentPlayer = DataFactory.GetPlayer(playerGUID);
            // TODO: add player logic, and/or logic per ship
            currentPlayer.GameLogic = new EnemyLogic();
            enemyPlayer = new Player(true) { ID = -1, Name = "Xorn" };
            enemyPlayer.GameLogic = new EnemyLogic();
            // get scenario
            currentScenario = DataFactory.GetScenario(scenarioGUID, currentPlayer, enemyPlayer);

            // features
            Features.AddRange(currentScenario.Features);

            // ships
            Ships.AddRange(currentScenario.Ships);

        }
                
        public List<Animation> Run()
        {
            // reset animations
            results = new List<Animation>();

            bool continueRunning = true;
            int roundCounter = 1;

            // send all features
            foreach(Feature feature in Features)
            {
                results.Add(new Animation(AnimationActionType.Feature, new AnimationFeatureDetails(feature.ID, feature.Position, feature.Type, feature.IsBlocking)));
            }

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
                    while (shipsAtInitiative.Any(x => !x.IsDestroyed && x.MP.Current > 0)
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
                            foreach (MoveOrder mo in orders.Where(x => x is MoveOrder))
                            {
                                mo.OnShipMoved -= new ShipMovedEvent(MovedHandler);
                                mo.OnShipMoved += new ShipMovedEvent(MovedHandler);

                                mo.OnMessageResult -= new MessageEvent(MessageHandler);
                                mo.OnMessageResult += new MessageEvent(MessageHandler);
                            }

                            // resolve orders
                            foreach (BaseOrder order in orders.OrderBy(x=>x.Priority))
                            {
                                if (!Ships.Any(x => !x.IsDestroyed && x.Owner == currentPlayer) || !Ships.Any(x => !x.IsDestroyed && x.Owner == enemyPlayer))
                                    continue;
                                order.Simulation = this;
                                if (Ships.First(x => x.ID == order.CurrentShip.ID).IsDestroyed)
                                    continue;
                                order.CurrentShip = Ships.First(x => x.ID == order.CurrentShip.ID);
                                if (order is FireWeaponOrder)
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
                    foreach (Ship ship in Ships)
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
            results.Add(new Animation(AnimationActionType.Message, null, new List<string>() { "*** " + Ships.First(x => x.ID == e.ShipID).Name + " Is Destroyed! ***" }));
            results.Add(new Animation(AnimationActionType.ShipUpdate, new AnimationShipUpdateDetails(Ships.First(x => x.ID == e.ShipID))));
        }

        public void WeaponFiredHandler(object sender, WeaponFiredEventArgs e)
        {
            List<AnimationShotDetails> shots = new List<AnimationShotDetails>();
            shots.Add(new AnimationShotDetails(e.ShooterID, e.TargetID, e.IsHit, e.IsCrit,e.FiringType));
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
