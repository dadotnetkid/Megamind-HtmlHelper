﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlExtensions.Base;
using HtmlExtensions.Utils;

namespace HtmlExtensions.TextBox
{
    public static class TextBoxExtension
    {
        public static void TextBox(this MegamindExtension megamind, Action<TextBoxEditorSetting> settings)
        {
            TextBoxEditorSetting textBoxEditorSetting = new TextBoxEditorSetting();
            settings(textBoxEditorSetting);
            textBoxEditorSetting.IMegamindExtension = megamind;
            Render(textBoxEditorSetting);
        }

        private static void Render(TextBoxEditorSetting setting)
        {
            RenderHtml(setting);
            RenderScript(setting);
            RenderClientSideKeyUp(setting);
        }

        private static void RenderClientSideKeyUp(TextBoxEditorSetting setting)
        {
            var writer = new MegamindWriter(setting.IMegamindExtension.HtmlHelper.ViewContext.Writer);
            writer.WriteLine("<script>");
            writer.WriteLine("$(document).ready(function(){")
                .WriteLine("$('#" + setting.Name + "').keyup(function(){")
                .WriteLine(setting.ClientSideEvents.KeyUp)
                .WriteLine("})")
                .WriteLine("})");
            writer.WriteLine("</script>");
        }

        private static void RenderScript(TextBoxEditorSetting setting)
        {
            var writer = new MegamindWriter(setting.IMegamindExtension.HtmlHelper.ViewContext.Writer);
            writer.WriteLine("<script>");
            writer.WriteLine("class " + setting.Name + "Class{")
                .WriteLine("constructor(){")
                .WriteLine("this.control=$('#" + setting.Name + "')")
                .WriteLine("}")
                ;

            writer.WriteLine("GetValue(){")
                .WriteLine("return this.control.val()")
                .WriteLine("}");
            writer.WriteLine("}");
            writer.WriteLine("var " + setting.Name + "=new " + setting.Name + "Class()");

            writer.WriteLine("</script>");

        }

        private static void RenderHtml(TextBoxEditorSetting setting)
        {
            var writer = new MegamindWriter(setting.IMegamindExtension.HtmlHelper.ViewContext.Writer);
            writer.WriteLine("<input type=\"text\" class=\"form-control\" id=\"" + setting.Name + "\">");
        }
    }
}
