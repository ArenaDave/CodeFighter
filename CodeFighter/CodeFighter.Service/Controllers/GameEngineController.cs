using CodeFighter.Logic;
using CodeFighter.Logic.Animations;
using CodeFighter.Logic.Players;
using CodeFighter.Logic.Scenarios;
using CodeFighter.Logic.Simulations;
using CodeFighter.Logic.Utility;
using CodeFighter.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;

namespace CodeFighter.Controllers
{
    //[System.Web.Http.Route("api/[controller]")]
    public class GameEngineController : ApiController
    {
        #region not going to use
        // GET api/gameEngine
        [System.Web.Http.HttpGet]
        public ActionResult Get()
        {
            List<string> test = new List<string>();
            test.Add("one");
            test.Add("two");
            return new JsonResult() { Data = new ClientPacket() { messages = test } };
        }

        #endregion

        // POST api/gameEngine
        [System.Web.Http.HttpPost]
        public ActionResult Post([FromBody]ClientPacket packet)
        {
            try
            {

                if (!validatePlayerID(packet.PlayerID))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid Player Token");
                }
                if (!validateScenarioID(packet.ScenarioID))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid Scenario Token");
                }
                object processedCode = processPlayerCode(packet.PlayerCode);
                List<Animation> results = runScenario(packet.PlayerID, packet.ScenarioID, processedCode);
                return new JsonResult() { Data = results };

            }
            catch (Exception ex)
            {
                List<Animation> results = new List<Animation>();
                results.Add(new Animation(AnimationActionType.Message, null, new List<string>() { ex.ToString() }));
                return new JsonResult() { Data = results };
            }
        }

        private bool validatePlayerID(Guid playerID)
        {
            return true;
        }

        private bool validateScenarioID(Guid scenarioID)
        {
            return scenarioID == Guid.Empty;
        }

        private object processPlayerCode(string playerCode)
        {
            return null;
        }

        private List<Animation> runScenario(Guid playerID, Guid scenarioID, object processedCode)
        {
            // animation results are what are sent to the client
            List<Animation> result = new List<Animation>();

            // get scenario
            Scenario currentScenario = getScenario(scenarioID);
            // get player
            Player currentPlayer = getPlayer(playerID);


            // setup simulation
            Simulation sim = new Simulation(currentScenario, currentPlayer);

            // run simulation
            result.AddRange(sim.Run());


            #region HARD CODED
            //Point zeroLoc = new Point(5, 5);
            //Point oneLoc = new Point(10, 10);

            //// add ships
            //result.Add(new Animation(AnimationActionType.Add, new AnimationAddDetails(0, zeroLoc, false, 2)));
            //result.Add(new Animation(AnimationActionType.Add, new AnimationAddDetails(1, oneLoc, true, 2)));

            //// ship 0 turn 1
            //zeroLoc.Offset(1, 1);
            //result.Add(new Animation(AnimationActionType.Move, new AnimationMoveDetails(0, zeroLoc)));
            //List<AnimationShotDetails> shots1 = new List<AnimationShotDetails>();
            //shots1.Add(new AnimationShotDetails(0, 1, true, false));
            //shots1.Add(new AnimationShotDetails(0, 1, false, false));
            //shots1.Add(new AnimationShotDetails(0, 1, true, true));
            //shots1.Add(new AnimationShotDetails(0, 1, true, false));

            //result.Add(new Animation(AnimationActionType.Shoot, new AnimationShootingDetails(shots1)));

            //// ship 1 turn 1
            //oneLoc.Offset(-1, -1);
            //result.Add(new Animation(AnimationActionType.Move, new AnimationMoveDetails(1, oneLoc)));
            //List<AnimationShotDetails> shots2 = new List<AnimationShotDetails>();
            //shots2.Add(new AnimationShotDetails(1, 0, true, false));
            //shots2.Add(new AnimationShotDetails(1, 0, false, false));
            //shots2.Add(new AnimationShotDetails(1, 0, true, true));
            //shots2.Add(new AnimationShotDetails(1, 0, true, false));

            //result.Add(new Animation(AnimationActionType.Shoot, new AnimationShootingDetails(shots2)));

            //// ship 0 turn 2
            //zeroLoc.Offset(1, 1);
            //result.Add(new Animation(AnimationActionType.Move, new AnimationMoveDetails(0, zeroLoc)));
            //List<AnimationShotDetails> shots3 = new List<AnimationShotDetails>();
            //shots3.Add(new AnimationShotDetails(0, 1, true, false));
            //shots3.Add(new AnimationShotDetails(0, 1, false, false));
            //shots3.Add(new AnimationShotDetails(0, 1, true, true));
            //shots3.Add(new AnimationShotDetails(0, 1, true, false));

            //result.Add(new Animation(AnimationActionType.Shoot, new AnimationShootingDetails(shots3)));
            //result.Add(new Animation(AnimationActionType.Kill, new AnimationKillDetails(1)));

            //// ship 1 is dead!

            //// ship 0 turn 3
            //zeroLoc.Offset(-1, 0);
            //result.Add(new Animation(AnimationActionType.Move, new AnimationMoveDetails(0, zeroLoc)));

            #endregion

            return result;
        }

        private Scenario getScenario(Guid scenarioID)
        {
            return new Scenario();
        }

        private Player getPlayer(Guid playerID)
        {
            return new Player() { Name = "Bob" };
        }
    }
}
