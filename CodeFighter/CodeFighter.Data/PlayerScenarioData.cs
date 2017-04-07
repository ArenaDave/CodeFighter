using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighter.Data
{
    public class PlayerScenarioData
    {
        [Key]
        [Column(Order = 1)]
        public int PlayerDataId { get; set; }
        public PlayerData PlayerData { get; set; }
        [Key]
        [Column(Order = 2)]
        public int ScenarioDataId { get; set; }
        public ScenarioData ScenarioData { get; set; }
        public DateTime DateRun { get; set; }
        public int FinalScore { get; set; }
        public string AllActions { get; set; }

    }
}
