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
            foreach (var i in scripts.OrderBy(x => x.Order))
            {
                writer.WriteLine("<script src='" + url.Action("Index", "Script", new { assemblyName = i.ExtensionSuite.ScriptFile}) + "' ></script>");
            }
        }
    }

    public class Script : BaseClientRenderer
    {

    }

    public class BaseClientRenderer
    {
        public ExtensionSuite ExtensionSuite { get; set; }
        public int Order { get; set; }
    }
    public class ExtensionSuite
    {
        public string ScriptFile { get; set; }
        public string StyleSheet { get; set; }
        public static ExtensionSuite MegamindSuite => new() { ScriptFile = "HtmlExtensions.EmbeddedResources.Scripts.Megamind.js" };
        public static ExtensionSuite Select2 => new() { ScriptFile = "HtmlExtensions.EmbeddedResources.Scripts.select2.min.js", StyleSheet = "HtmlExtensions.EmbeddedResources.Styles.select2.min.css" };
        public static ExtensionSuite DataTable => new() { ScriptFile = "HtmlExtensions.EmbeddedResources.Scripts.datatables.min.js", StyleSheet = "HtmlExtensions.EmbeddedResources.Styles.datatables.min.css" };
        public static ExtensionSuite Moment => new() { ScriptFile = "HtmlExtensions.EmbeddedResources.Scripts.moment.min.js" };

        public static ExtensionSuite FileUpload => new()
        {
            ScriptFile = "HtmlExtensions.EmbeddedResources.Scripts.FileUpload",
            StyleSheet = "HtmlExtensions.EmbeddedResources.Scripts.FileUpload",
            WildCard = true
        };

        public bool WildCard { get; set; }
    }
}
