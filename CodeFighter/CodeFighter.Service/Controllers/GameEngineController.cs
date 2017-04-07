using CodeFighter.Data;
using CodeFighter.Logic.Animations;
using CodeFighter.Logic.Simulations;
using CodeFighter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            using (CodeFighterContext db = new CodeFighterContext())
            {
                return db.PlayerData.Any(x => x.PlayerGUID.Equals(playerID));
            }
        }

        private bool validateScenarioID(Guid scenarioID)
        {
            using (CodeFighterContext db = new CodeFighterContext())
            {
                return db.ScenarioData.Any(x => x.ScenarioGUID.Equals(scenarioID));
            }
        }

        private object processPlayerCode(string playerCode)
        {
            // TODO: add CS-Script processing
            return null;
        }

        private List<Animation> runScenario(Guid playerID, Guid scenarioID, object processedCode)
        {
            // animation results are what are sent to the client
            List<Animation> result = new List<Animation>();

            // setup simulation
            Simulation sim = new Simulation(scenarioID, playerID, processedCode);

            // run simulation
            result.AddRange(sim.Run());

            return result;
        }


    }
}
