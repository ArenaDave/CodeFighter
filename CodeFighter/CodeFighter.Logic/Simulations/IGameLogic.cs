using CodeFighter.Logic.Orders;
using CodeFighter.Logic.Scenarios;
using CodeFighter.Logic.Ships;
using System.Collections.Generic;

namespace CodeFighter.Logic.Simulations
{
    public interface IGameLogic
    {
        List<BaseOrder> GetOrders(Ship currentShip, List<Ship> ships, List<Feature> features);
    }
}
