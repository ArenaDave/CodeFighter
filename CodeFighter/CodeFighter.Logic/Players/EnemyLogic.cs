using CodeFighter.Logic.Actions;
using CodeFighter.Logic.Orders;
using CodeFighter.Logic.Parts;
using CodeFighter.Logic.Scenarios;
using CodeFighter.Logic.Ships;
using CodeFighter.Logic.Simulations;
using CodeFighter.Logic.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighter.Logic.Players
{
    class EnemyLogic : IGameLogic
    {
        List<Ship> allShips = new List<Ship>();
        List<Feature> allFeatures = new List<Feature>();

        public List<BaseOrder> GetOrders(Ship currentShip, List<Ship> ships, List<Feature> features)
        {
            this.allShips = ships;
            this.allFeatures = features;
            // calculate center of the combat
            // center is the average (mean) of the coordinates
            double sumOfX = ships.Sum(x => x.Position.X);
            double sumOfY = ships.Sum(x => x.Position.Y);
            int centerX = Convert.ToInt32(Math.Round(sumOfX / ships.Count()));
            int centerY = Convert.ToInt32(Math.Round(sumOfY / ships.Count()));
            Point centerPoint = new Point(centerX, centerY);

            // sort ships by owner and distance from center.
            List<Ship> myShips = new List<Ship>();
            List<Ship> theirShips = new List<Ship>();

            foreach (Ship ship in ships.OrderBy(x => x.Position.DistanceTo(centerPoint)))
            {
                if (ship.Owner == currentShip.Owner)
                {
                    myShips.Add(ship);
                }
                else
                {
                    theirShips.Add(ship);
                }
            }


            bool newTarget = true;
            double targetHP = 0;
            double targetDPR = 0;
            double sumOfOurHP = 0;
            double ourLowestRegen = 999;
            double ourLifespan = 0.0d;
            double ourTotalDPR = 0;
            double killRatio = 0.0d;
            Ship targetShip = null;

            List<Ship> targettedShips = new List<Ship>();

            foreach (Ship ship in myShips)
            {
                // get a new target?
                if (newTarget)
                {
                    // reset the values
                    sumOfOurHP = 0;
                    ourLowestRegen = 999;
                    ourLifespan = 0;
                    killRatio = 0;

                    targetShip = theirShips
                        .OrderBy(x => x.Position.DistanceTo(ship.Position))
                        .FirstOrDefault(x => !targettedShips.Contains(x));
                    if (targetShip != null)
                    {
                        // store the target so we don't overlap
                        targettedShips.Add(targetShip);
                    }
                }

                if (targetShip != null)
                {
                    // store new stats
                    sumOfOurHP += getEHP(ship);
                    double currentRegen = getRegen(ship);
                    if (currentRegen < ourLowestRegen)
                        ourLowestRegen = currentRegen;
                    ourTotalDPR += getDPR(ship);

                    // calculate target stats
                    targetHP = getEHP(targetShip);
                    targetDPR = getDPR(targetShip);

                    // calculate lifespan
                    ourLifespan = sumOfOurHP / (targetDPR - ourLowestRegen);

                    // calculate total damage
                    double sumOfDamage = ourTotalDPR * ourLifespan;

                    // calculate kill ratio
                    killRatio = sumOfDamage / targetHP;

                    if (killRatio > 1.0)
                    {
                        newTarget = true;
                    }

                    if (ship.ID == currentShip.ID)
                    {
                        return issueOrders(currentShip, targetShip);
                    }

                }

            }


            // add default actions (pick closest target and issue orders for it)
            Point currentShipPosition = currentShip.Position;
            Ship finalTarget = ships.OrderBy(x => x.Position.DistanceTo(currentShipPosition)).First();
            return issueOrders(currentShip, finalTarget);
        }

        private List<BaseOrder> issueOrders(Ship currentShip, Ship targetShip)
        {
            List<BaseOrder> orders = new List<BaseOrder>();

            // check weapon range, if any weapon is not yet in range, move towards the target ship
            double minRange = 99;
            foreach (WeaponPart weapon in currentShip.Parts.Where(x => x is WeaponPart && !x.IsDestroyed))
            {
                if (weapon.Range < minRange)
                    minRange = weapon.Range;
            }

            Pathing pathHelper = new Pathing(allShips, allFeatures);

            Point targetPosition = pathHelper.GetTargetPointOnRadius(currentShip.Position, targetShip.Position, minRange);
            Point nextPosition = pathHelper.GetNextPoint(currentShip.Position, targetPosition);

            orders.Add(new MoveOrder(currentShip, nextPosition));

            // if any weapon is in range and reloaded, attack the target ship
            foreach (WeaponPart weapon in currentShip.Parts.Where(x => x is WeaponPart && !x.IsDestroyed))
            {
                if (weapon.Range >= currentShip.Position.DistanceTo(targetShip.Position)
                    && weapon.IsLoaded
                    && !weapon.HasFiredThisRound)
                {
                    orders.Add(new FireWeaponOrder(currentShip, weapon, targetShip));
                }
            }

            return orders;
        }

        private double getEHP(Ship targetShip)
        {
            double result = 0;

            result += targetShip.HP.Current;

            foreach (DefensePart defense in targetShip.Parts.Where(x => x is DefensePart && !x.IsDestroyed))
            {
                result += defense.HP.Current;
                result += defense.DR;
            }

            return result;
        }

        private double getDPR(Ship currentShip)
        {
            double result = 0;

            foreach (WeaponPart weapon in currentShip.Parts.Where(x => x is WeaponPart && !x.IsDestroyed))
            {
                if (weapon.ReloadTime > 0)
                {
                    result += Convert.ToDouble(weapon.WeaponDamage) / (weapon.ReloadTime + 1) + (0.05 * (Convert.ToDouble(weapon.WeaponDamage) * weapon.CritMultiplier) / (weapon.ReloadTime + 1));
                }
                else
                {
                    result += Convert.ToDouble(weapon.WeaponDamage) + (0.05 * (Convert.ToDouble(weapon.WeaponDamage) * weapon.CritMultiplier));
                }
            }

            return result;
        }

        private double getRegen(Ship targetShip)
        {
            double result = 0;

            foreach (BasePart part in targetShip.Parts.Where(x => x.Actions.Any(y => y is RepairPart || y is RepairShip) && !x.IsDestroyed))
            {
                foreach (BaseAction action in part.Actions)
                {
                    if (action is RepairPart)
                    {
                        result += Convert.ToDouble(action.ActionValues["RepairAmount"]);
                    }
                    else if (action is RepairShip)
                    {
                        result += Convert.ToDouble(action.ActionValues["RepairAmount"]);
                    }
                }
            }

            return result;
        }


    }
}
