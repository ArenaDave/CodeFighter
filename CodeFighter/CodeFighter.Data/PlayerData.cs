using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighter.Data
{
    
    public class PlayerData
    {
        [Key]
        public int Id { get; set; }
        public Guid PlayerGUID { get; set; }
        public string Name { get; set; }
    }
}
