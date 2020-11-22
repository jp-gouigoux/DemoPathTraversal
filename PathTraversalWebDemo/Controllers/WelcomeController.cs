using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PathTraversalWebDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WelcomeController : ControllerBase
    {
        [HttpGet]
        public ContentResult Get()
        {
            // Suppose this data comes from authentication for customer SALVIA
            string CustomerCode = "00000032";
            string CustomerName = "SALVIA";
            string CustomerAgent = "JP Gouigoux";

            StringBuilder sb = new StringBuilder();
            sb.Append("<h1>Bienvenue, " + CustomerName + " !</h1>");
            sb.Append("<h3>Votre correspondant</h3>");
            sb.Append("<p>" + CustomerAgent + "</p>");
            sb.Append("<h3>Vos documents</h3>");
            sb.Append("<iframe src='" + Request.Scheme + "://" + Request.Host + "/" + CustomerCode + "/' width='100%' height='100%' frameborder='0'/>");

            return new ContentResult {
                ContentType = "text/html",
                Content = sb.ToString(),
                StatusCode = (int)HttpStatusCode.OK
            };
        }
    }
}
