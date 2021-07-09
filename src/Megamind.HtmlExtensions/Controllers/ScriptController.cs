using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace HtmlExtensions.Controllers
{
    public class ScriptController : Controller
    {
        // GET
        public FileStreamResult Index(string assemblyName)
        {
            var assembly = typeof(MegamindHelper).Assembly;
            var res = assembly.GetManifestResourceNames().ToList().Where(x => x.Contains(assemblyName));
            var sr = assembly.GetManifestResourceStream(res.FirstOrDefault());
            return new FileStreamResult(sr, "text/javascript");
        }
        [HttpGet]
        [Route("styles/{assemblyname}")]
        public FileStreamResult GetStyles(string assemblyName)
        {
            var assembly = typeof(MegamindHelper).Assembly;
            var res = assembly.GetManifestResourceNames().ToList().Where(x => x.Contains(assemblyName));
            var sr = assembly.GetManifestResourceStream(res.FirstOrDefault());
            return new FileStreamResult(sr, "text/css");
        }
      
    }
}