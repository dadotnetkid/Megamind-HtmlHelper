using System.Web.Mvc;

namespace HtmlExtensions.Base
{
    public interface IMegamindExtension
    {
        public HtmlHelper HtmlHelper { get; set; }
    }

    public class MegamindExtension : IMegamindExtension
    {
        public HtmlHelper HtmlHelper { get; set; }
    }

}
