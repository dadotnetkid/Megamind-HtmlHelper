using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlExtensions.Base;

namespace HtmlExtensions.FileUpload
{
    public static class FileUploadExtenstion
    {
        public static void UploadControl(this MegamindExtension megamind, Action<FileUploadSettings> settings)
        {
            FileUploadSettings _settings = new FileUploadSettings();
            settings(_settings);

            _settings.IMegamindExtension = megamind;
            Render(_settings);
        }

        private static void Render(FileUploadSettings settings)
        {
            if (string.IsNullOrEmpty(settings.Name))
                throw new ArgumentNullException("Name should not empty");
            RenderHtml(settings);
            RenderScript(settings);
            RenderUploadScript(settings);
        }

        private static void RenderUploadScript(FileUploadSettings settings)
        {
            var writer = settings.Writer;
            writer.WriteLine("<script>")
                .WriteLine("    function UploadFile(ctrl) {")
                .WriteLine("        var file = $(ctrl)[0].files[0];")
                .WriteLine("        var formData = new FormData();")
                .WriteLine("        formData.append($(ctrl).attr('name'), file);")
                .WriteLine("        $.ajax({")
                .WriteLine("            url: '" + settings.FileUploadDetailSetting.UploadUrl + "',")
                .WriteLine("            type: 'POST',")
                .WriteLine("            data: formData,")
                .WriteLine("            processData: false,")
                .WriteLine("            contentType: false,");
            RenderAfterCallbackEvent(settings);
            writer.WriteLine("        })")
            .WriteLine("   }")
            .WriteLine("</script>");
            ;

        }

        private static void RenderAfterCallbackEvent(FileUploadSettings settings)
        {
            if (string.IsNullOrEmpty(settings.ClientSideEvents.AfterUpload))
                return;
            var writer = settings.Writer;
            writer.WriteLine("success:" + settings.ClientSideEvents.AfterUpload)
                .WriteLine(",");
        }

        private static void RenderScript(FileUploadSettings settings)
        {
            var writer = settings.Writer;
            writer.WriteLine("<script>");
            writer.WriteLine("$(document).ready(function(){")
                .WriteLine("$('#" + settings.Name + "').on('change',function(){")
                .WriteLine("UploadFile(this);")
                .WriteLine("})")
                .WriteLine("})");

            writer.WriteLine("</script>");
        }

        private static void RenderHtml(FileUploadSettings settings)
        {
            var writer = settings.Writer;
            writer.WriteLine("<input type='file' name='" + settings.Name + "' id='" + settings.Name + "'/>");
        }
    }
}
