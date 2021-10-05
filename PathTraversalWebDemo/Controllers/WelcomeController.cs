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
        //[HttpGet]
        public string GetContent(string fichier)
        {
            // Defence In Depth : plusieurs couches de sécurité plutôt qu'une seule
            if (fichier.Contains("..")) throw new ArgumentException();

            // Ce code-ci est suffisamment brutal pour permettre l'accès par http://localhost:56253/welcome?fichier=/Windows/System32/drivers/etc/hosts
            // Mais ce n'est pas nécessairement le cas si on déploie l'application sur un "vrai" serveur web, car seul IIS Express fonctionne sous l'utilisateur courant
            if (string.IsNullOrEmpty(fichier)) return string.Empty;
            string nomFichier = Path.Combine(@"C:\Code\Github\DemoPathTraversal\PathTraversalWebDemo\wwwroot", "00000032", fichier);

            // Ajout d'une protection aidée par la canonicalisation du nom de fichier (en vrai, pas de "magical chain" bien sûr...)
            string nomFichierReduit = Path.GetFullPath(nomFichier);
            if (!nomFichierReduit.ToLower().StartsWith(@"c:\code\github\demopathtraversal\pathtraversalwebdemo\wwwroot\"))
                throw new ArgumentException();

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

            // Pour éviter la faille ci-dessus, la canonicalisation de fichier n'est pas de mise et il faut d'autres méthodes :
            // - Authentification + impersonification + droits sur les fichiers
            // - Suppression du service de fichiers statiques et de discovery par le serveur web (seulement pour ressources publiques) et
            //   remplacement par du code sur mesure qui récupère les fichiers dans un dossier accessible par le serveur IIS

            return new ContentResult {
                ContentType = "text/html",
                Content = sb.ToString(),
                StatusCode = (int)HttpStatusCode.OK
            };
        }
    }
}
