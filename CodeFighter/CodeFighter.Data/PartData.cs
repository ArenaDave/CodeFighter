using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighter.Data
{
    public class PartData
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxHP { get; set; }
        public double Mass { get; set; }

        public int DR { get; set; }
        public string DownAdjective { get; set; }
        public string PenetrateVerb { get; set; }

        public double Thrust { get; set; }

        public int WeaponDamage { get; set; }
        public int CritMultiplier { get; set; }
        public int ReloadTime { get; set; }
        public string DamageType { get; set; }
        public string FiringType { get; set; }
        public double Range { get; set; }
        public bool IsPointDefense { get; set; }

        public ICollection<ActionData> ActionData { get; set; }
        public ICollection<ShipPartData> ShipPartData { get; set; }
    }
}
