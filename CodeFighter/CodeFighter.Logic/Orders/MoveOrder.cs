using CodeFighter.Logic.Animations;
using CodeFighter.Logic.Ships;
using CodeFighter.Logic.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighter.Logic.Orders
{
    public class ShipMovedEventArgs : MessageEventArgs
    {
        public int ShipID { get; set; }
        public Point MovedTo { get; set; }

        public ShipMovedEventArgs(int shipId, Point movedTo, List<string> messages)
            : base(messages)
        {
            this.ShipID = shipId;
            this.MovedTo = movedTo;
        }
    }

    public delegate void ShipMovedEvent(object sender, ShipMovedEventArgs e);

    public class MoveOrder : BaseOrder
    {
        #region Events
        public event ShipMovedEvent OnShipMoved;
        public event MessageEvent OnMessageResult;
        #endregion

        #region Public Properties
        public Point TargetPosition { get; private set; }
        #endregion

        #region Public Methods
        internal override void ExecuteOrder()
        {
            List<string> result = new List<string>();
            result.Add(string.Format("Attempting to execute order: {0}", this.ToString()));

            if(TargetPosition == CurrentShip.Position) // already at target location
            {
                result.Add("Already at target location!");
                OnMessageResult?.Invoke(this, new MessageEventArgs(result));
            }
            else if(CurrentShip.MP.Current<1) // no remaining MP
            {
                result.Add("No remaining MP!! [[BAD CODE!! BAD!! NO BISCUT!!]]");
                OnMessageResult?.Invoke(this, new MessageEventArgs(result));
            }
            else if(!CurrentShip.Position.IsAdjacent(TargetPosition)) // invalid movement location
            {
                result.Add("Target Position is not an adjacent location!! [[BAD CODE!! BAD!! GO TO YOUR ROOM!!]]");
                OnMessageResult?.Invoke(this, new MessageEventArgs(result));
            }
            else if(Simulation.Features.Any(x=>x.Position == TargetPosition && x.IsBlocking) 
                || Simulation.Ships.Any(x=>x.Position == TargetPosition && x.ID != CurrentShip.ID)) // trying to move into blocking feature or ship
            {
                result.Add("ABORT ORDER, IMMINENT COLLISION!! [[BAD CODE!! BAD!! DO THAT OUTSIDE!!]]");
                OnMessageResult?.Invoke(this, new MessageEventArgs(result));
            }
            else
            {
                CurrentShip.Position = TargetPosition;
                result.Add(string.Format("Moved to {0}", TargetPosition.ToString()));
                OnShipMoved?.Invoke(this, new ShipMovedEventArgs(CurrentShip.ID, TargetPosition, result));
            }
        }

        public override string ToString()
        {
            return string.Format("Move {0} to {1}", CurrentShip.ToString(), TargetPosition.ToString());
        }
        #endregion

        #region Constructors
        public MoveOrder(Ship currentShip, Point targetPosition)
            : base(currentShip)
        {
            this.TargetPosition = targetPosition;
        }
        #endregion
    }
}
