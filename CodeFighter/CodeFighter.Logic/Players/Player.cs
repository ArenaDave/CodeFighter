using CodeFighter.Logic.Ships;
using CodeFighter.Logic.Simulations;
using System;
using System.Collections.Generic;

namespace CodeFighter.Logic.Players
{
    public class Player : ICloneable
    {
        #region Public Properties
        public int ID { get; set; }
        public Guid PlayerID { get; set; }
        public string Name { get; set; }
        public bool IsAI { get; internal set; }
        public bool IsDefeated { get; set; }
        public IGameLogic GameLogic { get; set; }
        #endregion

        #region Public Methods
        public override string ToString()
        {
            return this.Name;
        }

        public object Clone()
        {
            Player copy = new Player();
            copy.ID = this.ID;
            copy.PlayerID = this.PlayerID;
            copy.Name = (string)this.Name.Clone();
            copy.IsAI = this.IsAI;
            copy.IsDefeated = this.IsDefeated;
            return copy;
        }
        #endregion

        #region Constructors
        public Player()
        {

        }
        public Player(bool isAI)
        {
            this.IsAI = isAI;
        }
        #endregion

        #region Operators
        public override bool Equals(object obj)
        {
            if(obj is Player)
            {
                if (((Player)obj).ID == this.ID)
                    return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return ID;
        }

        public static bool operator ==(Player left, Player right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Player left, Player right)
        {
            return !(left == right);
        }
        #endregion
    }
}
