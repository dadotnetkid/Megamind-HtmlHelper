using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
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
        public static void TextBoxFor<TModel>(this MegamindExtension megamind, Expression<Func<TModel, object>> expression, Action<TextBoxEditorSetting> action)
        {
            RenderHtmlFor(megamind, expression);

        }

        private static void RenderHtmlFor<TModel>(MegamindExtension megamind, Expression<Func<TModel, object>> expression)
        {
            var html = megamind.HtmlHelper;
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullBindingName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var fieldId = TagBuilder.CreateSanitizedId(fullBindingName);
            var metadata = ModelMetadata.FromLambdaExpression(expression, (ViewDataDictionary<TModel>)html.ViewData);
            var value = metadata.Model;
            var validationAttributes = html.GetUnobtrusiveValidationAttributes(fullBindingName, metadata);

            TagBuilder tag = new TagBuilder("input");
            tag.AddCssClass("form-control");
            tag.Attributes.Add("name", fullBindingName);
            tag.Attributes.Add("id", fieldId);
            tag.Attributes.Add("type", "text");
            tag.Attributes.Add("value", value == null ? "" : value.ToString());
            foreach (var key in validationAttributes.Keys)
            {
                tag.Attributes.Add(key, validationAttributes[key].ToString());
            }
            var writer = new MegamindWriter(megamind.HtmlHelper.ViewContext.Writer);
            writer.WriteLine(tag.ToString());
            SetFieldName(writer,  fieldId);
        }

        private static void SetFieldName(MegamindWriter writer,  string fieldId)
        {
            writer.WriteLine("<script>")
                .WriteLine($"{fieldId} = $('#{fieldId}')")
                .WriteLine("</script>");
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
