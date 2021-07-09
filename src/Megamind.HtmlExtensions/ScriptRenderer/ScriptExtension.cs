using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HtmlExtensions.Base;

namespace HtmlExtensions.ScriptRenderer
{
    public static class ScriptExtension
    {
        public static void GetScripts(this MegamindExtension megamind, params Script[] scripts)
        {
            var requestContext = HttpContext.Current.Request.RequestContext;
            var url = new UrlHelper(requestContext);
            var writer = megamind.HtmlHelper.ViewContext.Writer;
            foreach (var i in scripts)
            {
                writer.WriteLine("<script src='" + url.Action("Index", "Script", new { assemblyName = i.ExtensionSuite.ScriptFile }) + "' ></script>");
            }
        }
    }

    public class Script : BaseClientRenderer
    {


    }

    public class BaseClientRenderer
    {
        public ExtensionSuite ExtensionSuite { get; set; }
    }
    public class ExtensionSuite
    {
        public string ScriptFile { get; set; }
        public string StyleSheet { get; set; }
        public static ExtensionSuite MegamindSuite => new() { ScriptFile = "HtmlExtensions.Scripts.Megamind.js" };
        public static ExtensionSuite Select2 => new() { ScriptFile = "HtmlExtensions.Scripts.select2.min.js",StyleSheet= "HtmlExtensions.Styles.select2.min.css" };
  
    }
}
