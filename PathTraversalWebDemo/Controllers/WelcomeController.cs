using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
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
        [HttpGet("content")]
        public string GetContent(string fichier)
        {
            // Ce code-ci est suffisamment brutal pour permettre l'accès par http://localhost:56253/welcome/content?fichier=/Windows/System32/drivers/etc/hosts
            // Mais ce n'est pas nécessairement le cas si on déploie l'application sur un "vrai" serveur web, car seul IIS Express fonctionne sous l'utilisateur courant
            if (string.IsNullOrEmpty(fichier)) return string.Empty;
            string nomFichier = Path.Combine(@"C:\Code\Github\DemoPathTraversal\PathTraversalWebDemo\wwwroot", "00000032", fichier);
            return System.IO.File.ReadAllText(nomFichier);
        }

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
