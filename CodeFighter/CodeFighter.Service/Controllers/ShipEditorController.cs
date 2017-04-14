using CodeFighter.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CodeFighter.Service.Controllers
{
    public class ShipEditorController : ApiController
    {
        // GET: api/ShipEditor
        public IEnumerable<string> Get()
        {
            using (CodeFighterContext db = new CodeFighterContext())
            {
                //var ships = 
            }
            return null;
        }

        // GET: api/ShipEditor/5
        public string Get(int id)
        {
            return "value";
        }

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
