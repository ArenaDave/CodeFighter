using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighter.Data
{
    public class PartCountData
    {
        [Key]
        public int Id { get; set; }
        public string PartType { get; set; }
        public string ActionMechanism { get; set; }
        public int CountOfParts { get; set; }

        //public ICollection<ShipHullPartCountData> ShipHullPartCountData { get; set; }
    }
}
