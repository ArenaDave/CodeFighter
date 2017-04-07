using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighter.Data
{
    public class ShipPartData
    {
        [Key]
        public int Id { get; set; }
        public int PartDataId { get; set; }
        public PartData PartData { get; set; }
        public int ShipDataId { get; set; }
        public ShipData ShipData { get; set; }
    }
}
