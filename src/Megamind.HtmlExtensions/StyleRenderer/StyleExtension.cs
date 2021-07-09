using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HtmlExtensions.Base;
using HtmlExtensions.ScriptRenderer;

namespace HtmlExtensions.StyleRenderer
{
    public static class StyleExtension
    {
        public static void GetStyles(this MegamindExtension megamind, params StyleSheet[] styles)
        {
            var requestContext = HttpContext.Current.Request.RequestContext;
            var url = new UrlHelper(requestContext);
            var writer = megamind.HtmlHelper.ViewContext.Writer;
            foreach (var i in styles)
            {
                if (!string.IsNullOrEmpty(i.ExtensionSuite.StyleSheet))
                    writer.WriteLine("<link href='" + url.Action("GetStyles", "Script", new { assemblyName = i.ExtensionSuite.StyleSheet }) + "' rel='stylesheet' />");
            }
        }
    }

    public class StyleSheet : BaseClientRenderer
    {

    }
}
