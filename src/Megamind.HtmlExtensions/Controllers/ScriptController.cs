using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace HtmlExtensions.Controllers
{
    public class ScriptController : Controller
    {
        // GET
        public FileStreamResult Index(string assemblyName)
        {
            var assembly = typeof(MegamindHelper).Assembly;
           
            var res = assembly.GetManifestResourceNames().ToList().Where(x => x.StartsWith(assemblyName));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var i in res.OrderBy(x=>x))
            {
                var stream = assembly.GetManifestResourceStream(i);
                using StreamReader sr = new StreamReader(stream);
                stringBuilder.Append(sr.ReadToEnd());
            }
            var byteArray = Encoding.UTF8.GetBytes(stringBuilder.ToString());
            var ms = new MemoryStream(byteArray);
            
            return new FileStreamResult(ms, "text/javascript");
        }
        [HttpGet]
        [Route("styles/{assemblyname}")]
        public FileStreamResult GetStyles(string assemblyName)
        {
            var assembly = typeof(MegamindHelper).Assembly;
            var res = assembly.GetManifestResourceNames().ToList().Where(x => x.StartsWith(assemblyName));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var i in res)
            {
                var stream = assembly.GetManifestResourceStream(i);
                using (StreamReader sr = new StreamReader(stream))
                {
                    stringBuilder.Append(sr.ReadToEnd());
                }
            }

            var byteArray = Encoding.UTF8.GetBytes(stringBuilder.ToString());
            var ms = new MemoryStream(byteArray);
            return new FileStreamResult(ms, "text/css");
        }

    }
}