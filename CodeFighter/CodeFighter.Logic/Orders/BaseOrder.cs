using CodeFighter.Logic.Animations;
using CodeFighter.Logic.Parts;
using CodeFighter.Logic.Ships;
using CodeFighter.Logic.Simulations;
using CodeFighter.Logic.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighter.Logic.Orders
{
    public abstract class BaseOrder
    {
        
        public Ship CurrentShip { get; internal set; }
        internal Simulation Simulation { get; set; }

        internal abstract void ExecuteOrder();
        public abstract override string ToString();

        public BaseOrder(Ship currentShip)
        {
            this.CurrentShip = currentShip;
        }
    }
}
