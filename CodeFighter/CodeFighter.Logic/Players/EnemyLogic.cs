using CodeFighter.Logic.Orders;
using CodeFighter.Logic.Scenarios;
using CodeFighter.Logic.Ships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighter.Logic.Players
{
    public class EnemyLogic
    {
        public List<BaseOrder> GetOrders(List<Ship> ships, List<ScenarioFeature> features)
        {
            List<BaseOrder> results = new List<BaseOrder>();
            // TODO: add AI here

            return results;
        }
    }
}
