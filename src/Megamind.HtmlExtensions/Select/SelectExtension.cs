using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlExtensions.Base;
using HtmlExtensions.Utils;
using Newtonsoft.Json;

namespace HtmlExtensions.Select
{
    public static class SelectExtension
    {
        public static void Select(this MegamindExtension megamindExtension, Action<SelectSetting> settings)
        {
            SelectSetting selectSetting = new SelectSetting();
            settings(selectSetting);
            selectSetting.IMegamindExtension = megamindExtension;
            Render(selectSetting);
        }

        private static void Render(SelectSetting selectSetting)
        {
            RenderHtml(selectSetting);
            RenderScript(selectSetting);
        }

        private static void RenderHtml(SelectSetting selectSetting)
        {
            var writer = new MegamindWriter(selectSetting.IMegamindExtension.HtmlHelper.ViewContext.Writer);
            writer.WriteLine("<select id='" + selectSetting.Name + "' class='" + selectSetting.Properties.CustomCss + "'>");

            writer.WriteLine("</select>");

        }

        private static void RenderScript(SelectSetting selectSetting)
        {
            var writer = new MegamindWriter(selectSetting.IMegamindExtension.HtmlHelper.ViewContext.Writer);
            writer.WriteLine("<script>");
            if (!selectSetting.ServerSide)
            {
                writer.WriteLine(" var arrayOfData = " + JsonConvert.SerializeObject(selectSetting.DataSource) + "");
              
            }
            SetDataSourceMember(selectSetting, writer);
            writer.WriteLine("$('#" + selectSetting.Name + "').select2(");
            SetDataSource(selectSetting, writer);
            SetServerSideDataSource(selectSetting, writer);

            writer.WriteLine(")");
            writer.WriteLine("</script>");

        }

        private static void SetDataSourceMember(SelectSetting selectSetting, MegamindWriter writer)
        {
            writer
                .WriteLine("function SetDataSourceMember(arrayData){")
                .WriteLine("var data=$.map(arrayData,function(obj){")
                .WriteLine("obj.id=obj." + selectSetting.ValueMember)
                .WriteLine("obj.text=obj." + selectSetting.DisplayMember)
                .WriteLine("return obj;")
                .WriteLine("})")
                .WriteLine("return data;")
                .WriteLine("}")
                ;
        }

        private static void SetServerSideDataSource(SelectSetting selectSetting, MegamindWriter writer)
        {
            if (!selectSetting.ServerSide)
                return;
            writer.WriteLine("{")
                .WriteLine("ajax:{")
                .WriteLine("url:'" + selectSetting.DetailSetting.ApiRoute + "',")
                .WriteLine("processResults:function(data){")
                .WriteLine("return{")
                .WriteLine("results:SetDataSourceMember(data)")
                .WriteLine("};")
                .WriteLine("}")
                .WriteLine("}")
                .WriteLine("}")
                ;
        }

        private static void SetDataSource(SelectSetting selectSetting, MegamindWriter writer)
        {
            if (!selectSetting.ServerSide)
            {
                writer.WriteLine("{data:SetDataSourceMember(arrayOfData)}");
            }
        }
    }
}
