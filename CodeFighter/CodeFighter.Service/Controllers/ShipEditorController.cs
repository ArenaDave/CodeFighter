using CodeFighter.Data;
using CodeFighter.Logic.Parts;
using CodeFighter.Logic.Scenarios;
using CodeFighter.Logic.Ships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace CodeFighter.Service.Controllers
{
    [System.Web.Mvc.RoutePrefix("api/ShipEditor")]
    public class ShipEditorController : ApiController
    {
        // GET: api/ShipEditor
        //public IEnumerable<string> Get()
        //{
        //    using (CodeFighterContext db = new CodeFighterContext())
        //    {
        //        //var ships = 
        //    }
        //    return null;
        //}

        [System.Web.Mvc.HttpGet]
        [System.Web.Mvc.Route("GetHullsByKeel")]
        public ActionResult GetHullsByKeel(string designator)
        {
            List<ShipHull> hulls = DataFactory.GetShipHulls(designator);
            return new JsonResult() { Data = hulls, ContentType = typeof(WeaponPart).ToString() };
        }

        [System.Web.Mvc.HttpGet]
        [System.Web.Mvc.Route("GetKeels")]
        public ActionResult GetKeels()
        {
            List<Keel> keels = Keel.All();

            return new JsonResult() { Data = keels };
        }


        // GET: api/ShipEditor/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/ShipEditor
        public void Post([FromBody]string value)
        {
        }
        
        // DELETE: api/ShipEditor/5
        public void Delete(int id)
        {
        }
    }
}
