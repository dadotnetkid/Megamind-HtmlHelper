using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HtmlExtensions.Base
{
    public static class MegamindHtmlHelper
    {
        public static MegamindExtension Megamind(this HtmlHelper htmlHelper)
        {
            return new MegamindExtension()
            {
                HtmlHelper = htmlHelper
            };
        }

    }
}
