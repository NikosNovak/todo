using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using OwinSelfHosted.Model;

namespace OwinSelfHosted.Controller
{
    /// <summary>
    /// 
    /// </summary>
    public class TodosController : ApiController
    {
        DataAccess db = new DataAccess();

        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IEnumerable<Todo> Get()

        {
            return db.GetAll();
        }

        public Todo Get(int Id)
        {
            var task = db.Get(Id);
            if (task == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return task;
        }

        public void Post([FromBody]Todo newTask)
        {
            db.Insert(newTask);
        }


        public Todo Put(int todoId, [FromBody]Todo changedTask)
        {
            return changedTask;
        }

        public void Delete(int Id)
        {
            db.Delete(Id);
        }
    }
}