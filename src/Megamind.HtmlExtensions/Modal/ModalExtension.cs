using System;
using System.Text;
using System.Web.Mvc;
using HtmlExtensions.Base;

namespace HtmlExtensions.Modal
{
    public static class ModalExtension
    {
        /// <summary>
        /// DataTable
        /// Modal
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static ModalSettings Modal(this MegamindExtension megamindExtension, Action<ModalSettings> settings)
        {
            ModalSettings _settings = new ModalSettings()
            {

            };
            settings(_settings);
            _settings.IMegamindExtension = megamindExtension;
            return _settings;
        }

        public static void Render(this ModalSettings settings)
        {
            var writer = settings.IMegamindExtension.HtmlHelper.ViewContext.Writer;
            StringBuilder sb = new StringBuilder();
            var showModal = "";
            if (settings.ShowOnLoad)
                showModal = "data-show='true'";

            writer.WriteLine($"<div id=\"{settings.Name}\" class=\"modal\" tabindex=\"-1\" role=\"dialog\" {showModal}>");
            writer.WriteLine("   <div class=\"modal-dialog\" role=\"document\">");
            writer.WriteLine("     <div class=\"modal-content\">");
            writer.WriteLine("       <div class=\"modal-header\">");
            writer.WriteLine("        <button type='button' class='close' data-dismiss='modal' aria-label='Close'>");
            writer.WriteLine("          <span aria-hidden=\"true\">&times;</span>");
            writer.WriteLine("        </button>");
            writer.WriteLine($"       <h4 class='modal-title'>{settings.Properties.Title}</h4>");
            writer.WriteLine("      </div>");
            writer.WriteLine("       <div class=\"modal-body\">");
            settings.TemplateBodyContent();
            writer.WriteLine("      </div>");
            writer.WriteLine("       <div class=\"modal-footer\">");
            settings.TemplateFooterContent();
            writer.WriteLine("      </div>");
            writer.WriteLine("    </div>");
            writer.WriteLine("  </div>");
            writer.WriteLine("</div>");
            if (settings.ShowOnLoad)
            {
                sb.AppendLine("<script>")
                    .AppendLine("$(document).ready(function(){")
                    .AppendLine($"$('#{settings.Name}').modal('show')")
                    .AppendLine("});")
                    .AppendLine("</script>");
            }

            writer.WriteLine(sb.ToString());


            /*return MvcHtmlString.Create(sb.ToString());*/
        }



        public class ModalSettings:BaseSetting
        {
            public IMegamindExtension IMegamindExtension { get; set; }
            public bool ShowOnLoad { get; set; }
            public void SetTemplateBodyContent(Action action)
            {
                this.TemplateBodyContent = action;
            }

            public Action TemplateBodyContent { get; set; }

            public void SetTemplateFooterContent(Action action)
            {
                this.TemplateFooterContent = action;
            }

            public Action TemplateFooterContent { get; set; }
        }
    }
}