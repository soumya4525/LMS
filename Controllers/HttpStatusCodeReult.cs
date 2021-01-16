using System.Net;
using System.Web.Mvc;

namespace LMS.Controllers
{
    internal class HttpStatusCodeReult : ActionResult
    {
        private HttpStatusCode badRequest;

        public HttpStatusCodeReult(HttpStatusCode badRequest)
        {
            this.badRequest = badRequest;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}