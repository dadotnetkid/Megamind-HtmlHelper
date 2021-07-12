using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace HtmlExtensions.Base
{
    public interface IMegamindExtension
    {
        public HtmlHelper HtmlHelper { get; set; }
    }
    public interface IMegamindExtension<TModel> where TModel : class
    {
        public HtmlHelper HtmlHelper { get; set; }
    }
    public interface IMegamindExtension<TModel, TValue>
    {
         HtmlHelper<TModel> HtmlHelper { get; set; }
    }
    public class MegamindExtension : IMegamindExtension
    {
        public HtmlHelper HtmlHelper { get; set; }
    }
    public class MegamindExtension<T> : IMegamindExtension
    {
        public MvcHtmlString MvcHtmlString { get; set; }
        public List<T> Datasource { get; set; }
        public HtmlEditors.FormSettings FormSettings { get; set; }
        public HtmlHelper HtmlHelper { get; set; }
        public string DataUrl { get; set; }
        public string FormScript { get; set; }
        internal string UrlFormId { get; set; }
        internal HtmlEditors.DataLinkUrl DataLinkUrl { get; set; } = new();
    }

    public class MegamindExtension<TModel, TValue> : IMegamindExtension
    {
        public HtmlHelper HtmlHelper { get; set; }
    }

}
