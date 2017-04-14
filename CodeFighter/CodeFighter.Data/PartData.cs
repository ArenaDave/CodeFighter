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
        
        public string DefenseType { get; set; }
        
        public string DamageType { get; set; }
        public string FiringType { get; set; }

        public ICollection<ActionData> ActionData { get; set; }
        public ICollection<ShipPartData> ShipPartData { get; set; }
    }
}
