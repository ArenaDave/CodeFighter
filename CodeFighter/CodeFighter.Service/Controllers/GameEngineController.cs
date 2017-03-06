using CodeFighter.Logic;
using CodeFighter.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            return new JsonResult() { Data = new ClientPacket() };
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest,ex.ToString());
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
