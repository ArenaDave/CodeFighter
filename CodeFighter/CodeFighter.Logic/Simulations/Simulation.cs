using CodeFighter.Logic.Animations;
using CodeFighter.Logic.Players;
using CodeFighter.Logic.Scenarios;
using System.Collections.Generic;

namespace CodeFighter.Logic.Simulations
{
    public class Simulation
    {
        Player enemyPlayer;
        Player currentPlayer;
        List<ScenarioFeature> features;



        public Simulation(Scenario currentScenario, Player currentPlayer)
        {
            // players
            enemyPlayer = new Player();
            this.currentPlayer = currentPlayer;

            // TODO: features

            // ships
            for (int i = 0; i < currentScenario.Ships.Count; i++)
            {
                ScenarioShip ss = currentScenario.Ships[i];
            }


        }

        public List<Animation> Run()
        {
            List<Animation> results = new List<Animation>();
            // round loop
            
            //
            return results;
        }
    }
}
