using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFighter.Models
{
    public class ClientPacket
    {
        public Guid ScenarioID { get; set; }
        public Guid PlayerID { get; set; }
        public string PlayerCode { get; set; }

        public List<string> messages { get; set; }
    }
}
