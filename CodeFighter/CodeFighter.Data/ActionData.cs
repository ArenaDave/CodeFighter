using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighter.Data
{
    public class ActionData
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ActionValuesJSON { get; set; }
        public bool TargetSelfPart { get; set; }
        public bool TargetSelfShip { get; set; }

        public ICollection<PartData> PartData { get; set; }
    }
}
