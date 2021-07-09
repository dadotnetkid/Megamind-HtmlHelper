using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HtmlExtensions.Bootstrap
{
    public static class BootstrapExtension
    {
        public static Bootstrap Bootstrap(this HtmlHelper htmlHelper)
        {
            return new Bootstrap()
            {
                HtmlHelper = htmlHelper
            };
        }
        public static Container Container(this Bootstrap bootstrap)
        {
            return new Container()
            {
                Bootstrap = bootstrap
            };
        }

        public static Rows Rows(this Container container)
        {
            return new Rows()
            {
                Container = container
            };
        }

        public static Columns Columns(this Rows rows)
        {
            return new Columns()
            {
                Rows = rows
            };
        }
    }

    public class Columns
    {
        public Rows Rows { get; set; }
    }

    public class Rows
    {
        public Container Container { get; set; }
    }

    public class Bootstrap
    {
        public HtmlHelper HtmlHelper { get; set; }
    }

    public class Container
    {
        public Bootstrap Bootstrap { get; set; }
    }
}
