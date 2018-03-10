using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace LiteDB.AutoApi
{
    [Route("[controller]")]
    public abstract class BaseController<T> : Controller where T : LiteDbModel
    {
        private readonly LiteDatabase Db;
        
        private readonly LiteCollection<T> Collection;

        protected BaseController()
        {
            // Open database (or create if not exits)
            Db = new LiteDatabase($"Filename={Assembly.GetEntryAssembly().GetName().Name}.db;Mode=Exclusive");
            
            // Get model collection
            Collection = Db.GetCollection<T>(typeof(T).Name);
        }
        
        //GET /<controllerName>/<id>
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetOne(int id)
        {
            T record = Collection.FindById(id);

            if (record != null)
            {
                // find and return employee
                return Json(record);
            }

            return NotFound();
        }
        
        //GET /<controllerName>/
        [Route("")]
        [HttpGet]
        public IActionResult GetAll()
        {            
            // return list of entities
            return Json(Collection.FindAll().ToList());
        }
        
        //DELETE /<controllerName>/<id>
        [Route("{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            bool deleted = Collection.Delete(id);
            if (deleted)
            {
                return Ok();
            }
            return NotFound();
        }
        
        //POST /<controllerName>/
        [Route("")]
        [HttpPost]
        public ActionResult Save([FromBody] T record)
        {            
            // upsert
            bool inserted = Collection.Upsert(record);

            if (inserted)
            {
                // return id of saved record
                return StatusCode(201, new {record.Id});
            }

            return Ok();
        }
        
        protected override void Dispose(bool disposing)
        {
            Db?.Dispose();
            base.Dispose(disposing);
        }
    }
}