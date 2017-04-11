using CodeFighter.Logic.Ships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighter.Logic.Animations
{
    public class AnimationShipUpdateDetails : IAnimationDetails
    {
        public bool ownerIsAI { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public bool isDestroyed { get; set; }
        public string className { get; set; }
        public string sizeName { get; set; }
        public string hp { get; set; }
        public string pos { get; set; }
        public List<string> parts { get; set; }

        public AnimationShipUpdateDetails(Ship ship)
        {
            this.ownerIsAI = ship.Owner.IsAI;
            this.id = ship.ID;
            this.name = ship.Name;
            this.isDestroyed = ship.IsDestroyed;
            this.className = string.Format("{0}-class {1}",ship.Hull.ClassName, ship.Hull.Size.Name);
            this.sizeName = ship.Hull.Size.Designator;
            this.hp =string.Format("HP: {0}", ship.HP.ToString());
            this.pos = string.Format("Pos: {0}",ship.Position.ToString());
            this.parts = new List<string>();
            foreach (var part in ship.Parts)
                parts.Add(part.ToString());
        }

    }
}
