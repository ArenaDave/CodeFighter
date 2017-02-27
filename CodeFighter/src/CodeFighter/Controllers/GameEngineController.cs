using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CodeFighter.Code;
using System.Net.Http;
using System.Net;
using CodeFighter.Logic;

namespace CodeFighter.Controllers
{
    [Route("api/[controller]")]
    public class GameEngineController : Controller
    {
        #region not going to use
        // GET api/gameEngine
        [HttpGet]
        public JsonResult Get()
        {
            return new JsonResult(new ClientPacket());// string[] { "value1", "value2" };
        }

        // GET api/gameEngine/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // PUT api/gameEngine/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/gameEngine/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        #endregion

        // POST api/gameEngine
        [HttpPost]
        public JsonResult Post([FromBody]ClientPacket packet)
        {
            try
            {
                if (!validatePlayerID(packet.PlayerID))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return new JsonResult("Invalid Player Token");
                }
                if (!validateScenarioID(packet.ScenarioID))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return new JsonResult("Invalid Scenario Token");
                }
                object processedCode = processPlayerCode(packet.PlayerCode);
                List<Animation> results = runScenario(packet.PlayerID, packet.ScenarioID, processedCode);
                Response.StatusCode = (int)HttpStatusCode.OK;
                return new JsonResult(results);

            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new JsonResult(ex);
            }
        }

        private bool validatePlayerID(Guid playerID)
        {
            return true;
        }

        private bool validateScenarioID(Guid scenarioID)
        {
            return true;
        }

        private object processPlayerCode(string playerCode)
        {
            return null;
        }

        private List<Animation> runScenario(Guid playerID, Guid scenarioID, object processedCode)
        {
            List<Animation> result = new List<Animation>();

            // TODO: add logic to run scenario for player and code

            #region HARD CODED
            Point zeroLoc = new Point(5, 5);
            Point oneLoc = new Point(10, 10);

            // add ships
            result.Add(new Animation(ActionType.Add, new AnimationAdd(0, zeroLoc, false, 2)));
            result.Add(new Animation(ActionType.Add, new AnimationAdd(1, oneLoc, true, 2)));

            // ship 0 turn 1
            zeroLoc.Offset(1, 1);
            result.Add(new Animation(ActionType.Move, new AnimationMove(0, zeroLoc)));
            List<AnimationShot> shots1 = new List<AnimationShot>();
            shots1.Add(new AnimationShot(zeroLoc, oneLoc, false, true, false));
            shots1.Add(new AnimationShot(zeroLoc, oneLoc, false, false, false));
            shots1.Add(new AnimationShot(zeroLoc, oneLoc, false, true, true));
            shots1.Add(new AnimationShot(zeroLoc, oneLoc, false, true, false));
            result.Add(new Animation(ActionType.Shoot, new AnimationShoot(shots1)));

            // ship 1 turn 1
            oneLoc.Offset(-1, -1);
            result.Add(new Animation(ActionType.Move, new AnimationMove(1, oneLoc)));
            List<AnimationShot> shots2 = new List<AnimationShot>();
            shots2.Add(new AnimationShot(oneLoc, zeroLoc, true, true, false));
            shots2.Add(new AnimationShot(oneLoc, zeroLoc, true, false, false));
            shots2.Add(new AnimationShot(oneLoc, zeroLoc, true, true, true));
            shots2.Add(new AnimationShot(oneLoc, zeroLoc, true, true, false));
            result.Add(new Animation(ActionType.Shoot, new AnimationShoot(shots2)));

            // ship 0 turn 2
            zeroLoc.Offset(1, 1);
            result.Add(new Animation(ActionType.Move, new AnimationMove(0, zeroLoc)));
            List<AnimationShot> shots3 = new List<AnimationShot>();
            shots3.Add(new AnimationShot(zeroLoc, oneLoc, false, true, false));
            shots3.Add(new AnimationShot(zeroLoc, oneLoc, false, false, false));
            shots3.Add(new AnimationShot(zeroLoc, oneLoc, false, true, true));
            shots3.Add(new AnimationShot(zeroLoc, oneLoc, false, true, false));
            result.Add(new Animation(ActionType.Shoot, new AnimationShoot(shots3)));
            result.Add(new Animation(ActionType.Kill, new AnimationKill(1)));

            // ship 1 is dead!

            // ship 0 turn 3
            zeroLoc.Offset(-1, 0);
            result.Add(new Animation(ActionType.Move, new AnimationMove(0, zeroLoc)));
            
            #endregion

            return result;
        }
    }
}
