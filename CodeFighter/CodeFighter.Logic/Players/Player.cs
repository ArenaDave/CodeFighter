using CodeFighter.Logic.Ships;
using System;
using System.Collections.Generic;

namespace CodeFighter.Logic.Players
{
    public class Player : ICloneable
    {
        public int ID { get; set; }
        public Guid PlayerID { get; set; }
        public string Name { get; set; }
        public bool IsAI { get; private set; }
        public bool IsDefeated { get; set; }

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

        public dynamic GameLogic;
        public Player()
        {

        }
        public Player(bool isAI)
        {
            this.IsAI = isAI;
        }
    }
}
