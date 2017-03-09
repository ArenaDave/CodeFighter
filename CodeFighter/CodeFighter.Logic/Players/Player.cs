using CodeFighter.Logic.Ships;
using System;
using System.Collections.Generic;

namespace CodeFighter.Logic.Players
{
    public class Player
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

        public dynamic GameLogic;
    }
}
