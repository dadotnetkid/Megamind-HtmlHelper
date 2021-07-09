using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;
using Newtonsoft.Json;

namespace HtmlExtensions
{

    public static class HtmlEditors
    {
        public static MegamindExtension<T> Megamind<T>(this HtmlHelper htmlHelper)
        {
            return new MegamindExtension<T>() { HtmlHelper = htmlHelper };
        }

        public static MegamindExtension<T> BuildForm<T>(this MegamindExtension<T> megamindExtension,
            Action<FormSettings> settings
            )
        {
            FormSettings _settings = new FormSettings();
            settings(_settings);
            megamindExtension.FormSettings = _settings;

            return megamindExtension;
        }

        public static MvcHtmlString Render<T>(this MegamindExtension<T> megamindExtension, WebPageBase webPage)
        {
            var model = JsonConvert.DeserializeObject<List<BaseModel>>(JsonConvert.SerializeObject(megamindExtension.Datasource));
            string sHeader = "";
            string sSubHeader = "";
            string colspan = "1";
            int headerCtr = 1;
            string sType = "0";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div class='row'>");
            sb.AppendLine("<div class='col-sm-12'>");
            sb.AppendLine("<table class='table table-low-bordered table-small-padd table-condensed' cellspacing='0'>");

            foreach (var i in model.Where(x => x.attr1 == "0").OrderBy(x => Convert.ToInt16(x.name)))
            {

                if (i.attr2 != "")
                {
                    if (megamindExtension.FormSettings.ShowHeaderRow)
                        if (sHeader != i.attr2)
                        {
                            sHeader = i.attr2;
                            headerCtr++;
                            sb.AppendLine(
                                $"<tr id='tr_{i.id}' class='_all_tr _all_tr_{i.id} _tbl_th' data-toggle='collapse' href='._collapse_tr_{headerCtr}'>");
                            sb.AppendLine(" <th style='text-align: center; width: 20%;' colspan='2'>");
                            sb.AppendLine(sHeader?.ToUpper());
                            sb.AppendLine("<span class='pull-right'>");
                            sb.AppendLine(
                                "<button class='btn btn-default btn-xs'><i class='glyphicon glyphicon-minus' data-toggle='tooltip' title='Click to minimize'></i></button>");
                            if (megamindExtension.FormSettings.ShowSaveButton)
                                sb.AppendLine(
                                    "<span class='btn btn-xs btn-warning sv_btn_frm_@ViewBag.FormID-sv _sv_btn_frm' data-ispost='0'>Save</span>");
                            sb.AppendLine("</span></th></tr>");
                        }
                }

                if (i.attr4 != "")
                {
                    if (sSubHeader != i.attr4)
                    {
                        sSubHeader = i.attr4;
                        sb.AppendLine($"<tr class='_all_tr _all_tr_{i.id} _all_tr_ch_{headerCtr} collapse _collapse_tr_{headerCtr} in'>");
                        sb.AppendLine($"<th colspan='2'>{sSubHeader}</th></tr>");
                    }
                }

                sb.AppendLine(
                    $"<tr class='_all_tr _all_tr_frmid_{megamindExtension.FormSettings.FormId} _all_tr_{i.id} _all_tr_ch_{headerCtr} collapse _collapse_tr_{headerCtr} in' data-id='{headerCtr}'>");
                if (i.text != "")
                {
                    sb.AppendLine($"<th style='width: 20%;' class='_tr_{i.id} _td_parent'>{i.text}</th>");
                }

                var txt = i.text == "" ? "2" : "1";
                sb.AppendLine($"<td class='_all_td _all_td_{i.id} _td_child' colspan='{txt}'>");
                if (i.attr3 != "")
                {


                    sb.AppendLine(webPage.RenderPage("~/Areas/NURSE/Views/Shared/_formfieldtype_" + i.attr3 + ".cshtml",
                        new
                        {
                            id = i.id,
                            idd = "0",
                            attr5 = i.attr5,
                            text = i.text,
                            sclass = "_form_class_" + i.id + "_0_" + megamindExtension.FormSettings.FormId,
                            sclass1 = "_form_class_all_" + megamindExtension.FormSettings.FormId,
                            frmid = megamindExtension.FormSettings.FormId,
                            smodel = model.Where(x => x.attr1 == i.id && x.attr3 == "41"),
                        }).ToHtmlString());
                }

                if (model.Count(x => x.attr1 == i.id) != 0)
                {
                    sb.AppendLine("   <table style='width: 100%;' cellspacing='0'>");
                    sb.AppendLine($"<tr class='_all_tr _all_tr_{i.id}'>");
                    sb.AppendLine($"<td class='_all_td_1 _all_td_1_{i.id}'>");
                    foreach (var iii in model.Where(x => x.attr1 == i.id).OrderBy(x => Convert.ToInt16(x.name)))
                    {

                        sb.AppendLine(webPage.RenderPage(
                            "~/Areas/NURSE/Views/Shared/_formfieldtype_" + iii.attr3 + ".cshtml",
                            new
                            {
                                id = iii.id,
                                idd = i.id,
                                attr5 = iii.attr5,
                                text = iii.text,
                                sclass = "_form_class_" + iii.id + "_" + i.id + "_" +
                                         megamindExtension.FormSettings.FormId,
                                smodel = model.Where(x => x.attr1 == iii.id && x.attr3 == "41"),
                                sclass1 = "_form_class_all_" + megamindExtension.FormSettings.FormId,
                                frmid = megamindExtension.FormSettings.FormId
                            }).ToHtmlString());
                    }

                    sb.AppendLine("</td></tr></table>");
                }

                sb.AppendLine("</td></tr>");
            }

            sb.AppendLine("</table></div></div></div>");
            sb.AppendLine($"<link href='/Content/plugins/icheck/skins/all.css'  rel='stylesheet'/>");
            sb.AppendLine($"<link href='/Content/plugins/nouislider/nouislider.min.css' rel='stylesheet' />");
            sb.AppendLine($"<script src='/Content/plugins/nouislider/nouislider.min.js'></script>");
            if (string.IsNullOrEmpty(megamindExtension.UrlFormId))
                sb.AppendLine($"<input id='url_frm_nur_{megamindExtension.FormSettings.FormId}' data-upd='{megamindExtension.FormSettings.UpdateUrl}' data-sv='{megamindExtension.FormSettings.SavingUrl}' data-frmid='{megamindExtension.FormSettings.FormId}'  {megamindExtension.DataUrl} type='hidden'/> ");
            else
            {
                sb.AppendLine($"<input id='{megamindExtension.UrlFormId}' data-upd='{megamindExtension.FormSettings.UpdateUrl}' data-sv='{megamindExtension.FormSettings.SavingUrl}' data-frmid='{megamindExtension.FormSettings.FormId}' data-frmid_pin_id='0' data-formdetail='{megamindExtension.FormSettings.DetailUrl}' {megamindExtension.DataUrl} type='hidden'/> ");
            }

            if (!megamindExtension.FormSettings.EnableInlineScript)
                if (string.IsNullOrEmpty(megamindExtension.FormScript))
                    sb.AppendLine(
                        $"<script src='/Scripts/nurse/jqfrm_nur_{megamindExtension.FormSettings.FormId}-ja.js'></script>");
                else
                    sb.AppendLine($"<script src='{megamindExtension.FormScript}'></script>");
            else
            {
                sb.AppendLine("<script>");
                sb.AppendLine(OnDocumentLoad(megamindExtension).ToString());
                sb.AppendLine("</script>");
            }
            megamindExtension.MvcHtmlString = MvcHtmlString.Create(sb.ToString());
            return megamindExtension.MvcHtmlString;
        }


        public static MegamindExtension<T> UrlFor<T>(this MegamindExtension<T> megamindextension, string dataUrl, string values)
        {
            megamindextension.DataUrl += $" {dataUrl}='{values}'";
            return megamindextension;
        }
        public static MegamindExtension<T> Bind<T>(this MegamindExtension<T> megamindExtension, List<T> model)
        {
            megamindExtension.Datasource = model;
            return megamindExtension;
        }

        public static MegamindExtension<T> SetFormScript<T>(this MegamindExtension<T> megamindExtension, string formScript)
        {
            megamindExtension.FormScript = formScript;
            return megamindExtension;
        }

        public static FormSettings ShowSaveButton(this FormSettings formSettings, bool show)
        {
            formSettings.ShowSaveButton = show;
            return formSettings;
        }
        public static FormSettings ShowHeaderRow(this FormSettings formSettings, bool show)
        {
            formSettings.ShowHeaderRow = show;
            return formSettings;
        }

        public static MegamindExtension<T> SetFormUrlId<T>(this MegamindExtension<T> megamindExtension,
            string formUrlId)
        {
            megamindExtension.UrlFormId = formUrlId;
            return megamindExtension;
        }

        public static MegamindExtension<T> AddLink<T>(this MegamindExtension<T> megamindExtension, string controlId, string element, string linkUrl)
        {
            megamindExtension.UrlFor($"data-form_{controlId}", $"{ linkUrl}?typ = link");
            megamindExtension.DataLinkUrl.LinkUrl += $"LinkedForm('{controlId}',$('{element}'),'{linkUrl}?typ = link');";
            return megamindExtension;
        }
        private static StringBuilder OnDocumentLoad<T>(MegamindExtension<T> megamindExtension)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(SaveFunction(megamindExtension).ToString());
            sb.AppendLine(OnDocumentInit(megamindExtension));

            return sb;
        }

        private static string InitDefVal<T>(MegamindExtension<T> megamindExtension)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("function InitDefVal(){")
                .AppendLine("var pin = getStorage('nr_pin');")
                .AppendLine("  var vid = getStorage('nr_vid');")
                .AppendLine(" $('._all_tr_glas').hide();        ")
                .AppendLine("  $('._all_tr_glas_' + moment(new Date()).format('H')).show();")
                .AppendLine("  _.delay(function () {")
                .AppendLine("    var id = $('#" + megamindExtension.UrlFormId + "').data('frmid_pin_id');")
                .AppendLine("console.log(id);")
                .AppendLine("if (parseInt(id) == id) {")
                .AppendLine(" if (id != '0') {")
                .AppendLine("     ajaxWrapper.Get($('#" + megamindExtension.UrlFormId + "').data('formdetail'), { id: id }, function (x) {")
                .AppendLine("$.each(x, function (y, z) {")
                //set value for checkbox
                .AppendLine("if (typeof ($('#chk_' + z.id).attr('id')) != 'undefined') {")
                .AppendLine("console.log(z.name);")
                .AppendLine("if(z.name==='true'){")
                .AppendLine(" $('#chk_' + z.id).iCheck('check');}}")
                //set value for txtbox
                .AppendLine(" $('#txt_' + z.id).val(z.name);")
                .AppendLine("$.each($('._chkval'), function () {")

                .AppendLine("    if ($(this).data('id') == z.id) {")
                .AppendLine("if ($(this).data('val') == z.name) {")
                .AppendLine("    $(this).iCheck('check');")
                .AppendLine("}").AppendLine("}").AppendLine("});").AppendLine("});")
                .AppendLine("});")
                .AppendLine("}")
                .AppendLine("}")
                .AppendLine("}, 1000);}");
            return sb.ToString();
        }
        private static string OnDocumentInit<T>(MegamindExtension<T> megamindExtension)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("$(document).ready(function () {");
            sb.AppendLine("$('#" + megamindExtension.UrlFormId + "').data('frmid_pin_id', getStorage('nr_frmid_pin_id'));");
            sb.AppendLine("BlackCheck($('.icheck'));");
            sb.AppendLine("InitDT($('._dt'));");
            sb.AppendLine("InitTimePicker($('._time'));");
            sb.AppendLine($"AddSVBtn('sv_btn_frm_{megamindExtension.FormSettings.FormId}');")
                .AppendLine(" $(document).on('click', '#sv_btn_frm_" + megamindExtension.FormSettings.FormId + "', function () {")

                .AppendLine("SaveFunction()")
                .AppendLine("});");
            sb.AppendLine("InitDefVal()");
            sb.AppendLine(megamindExtension.DataLinkUrl?.LinkUrl);
            sb.AppendLine("});");
            sb.AppendLine(InitDefVal(megamindExtension));
            return sb.ToString();
        }

        private static StringBuilder SaveFunction<T>(MegamindExtension<T> megamindExtension)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("function SaveFunction(){")
                .AppendLine("var ss='';")
                .AppendLine("var det=[];")
                .AppendLine("$.each($('._textval'),function(){")
                .AppendLine("ss = ss + $(this).val();")
                .AppendLine("if ($(this).val() != '') {")
                .AppendLine("det.push({")
                .AppendLine("id:$(this).data('id'),")
                .AppendLine("text:$(this).data('idd'),")
                .AppendLine("name:$(this).val()")
                .AppendLine("})")
                .AppendLine("}")
                .AppendLine("});")
                .AppendLine("$.each($('._selval'), function () {")
                .AppendLine("ss = ss + $(this).val();")
                .AppendLine("if ($(this).val() != ''){")
                .AppendLine(" det.push({")
                .AppendLine(" id: $(this).data('id'),")
                .AppendLine(" text: $(this).data('idd'),")
                .AppendLine("   name: $(this).val()")
                .AppendLine("})")
                .AppendLine("}")
                .AppendLine("})")
                .AppendLine("$.each($('._radval_form_" + megamindExtension.FormSettings.FormId + "'), function () {")
                .AppendLine("ss = ss + $(this).val();")
                .AppendLine("if ($(this).val() != ''){")
                .AppendLine(" det.push({")
                .AppendLine(" id: $(this).data('id'),")
                .AppendLine(" text: $(this).data('idd'),")
                .AppendLine("   name: $(this).val()")
                .AppendLine("})")
                .AppendLine("}")
                .AppendLine("})")
                .AppendLine("$.each($('._chkval_form_" + megamindExtension.FormSettings.FormId + "'), function () {")
                .AppendLine("ss = ss + $(this).val();")
                .AppendLine("if ($(this).val() != ''){")
                .AppendLine(" det.push({")
                .AppendLine(" id: $(this).data('id'),")
                .AppendLine(" text: $(this).data('idd'),")
                .AppendLine("   name: $(this).val()")
                .AppendLine("})")
                .AppendLine("}")
                .AppendLine("})")
                ;
            sb.AppendLine("var main = {")
                .AppendLine("FormID: $('#" + megamindExtension.UrlFormId + "').data('frmid'),")
                .AppendLine("VisitID: getStorage('nr_vid'),")
                .AppendLine("IPID: '0',")
                .AppendLine("RegistrationNo: getStorage('nr_pin'),")
                .AppendLine("Age: $('#txtage').text(),")
                .AppendLine("Nationality: $('#txtpatientID').data('nat') };")
                .AppendLine("ajaxWrapper.Post($('#" + megamindExtension.UrlFormId + "').data('sv'), JSON.stringify({ mod: main, detail: det }), function (x) {")
                .AppendLine("swal({ title: 'Nursing Form', text: x.Message, html: true, type: 'success' });")
                .AppendLine(" });")
                .AppendLine("console.log('det:'+JSON.stringify(det)+'mod:'+main)")
                .AppendLine("}");




            return sb;
        }
        public class MegamindExtension<T>
        {
            public MvcHtmlString MvcHtmlString { get; set; }
            public List<T> Datasource { get; set; }
            public FormSettings FormSettings { get; set; }
            public HtmlHelper HtmlHelper { get; set; }
            public string DataUrl { get; set; }
            public string FormScript { get; set; }
            internal string UrlFormId { get; set; }
            internal DataLinkUrl DataLinkUrl { get; set; } = new();
        }

        public class DataLinkUrl
        {
            internal string LinkUrl { get; set; }
        }

        public class FormSettings
        {
            public string FormId { get; set; }
            public string SavingUrl { get; set; }
            public string UpdateUrl { get; set; }
            internal bool ShowSaveButton { get; set; }
            internal bool ShowHeaderRow { get; set; }
            public bool EnableInlineScript { get; set; }

            public void SetTemplateContent(Action<FormSettingContent> action)
            {
                action(this.FormSettingContent);
            }

            public FormSettingContent FormSettingContent { get; set; } = new();
            public string DetailUrl { get; set; }
        }

        public class FormSettingContent
        {

        }
    }

    public class BaseModel
    {
        public string id { get; set; }
        public string text { get; set; }
        public string name { get; set; }

        public string attr1 { get; set; }
        public string attr2 { get; set; }
        public string attr3 { get; set; }
        public string attr4 { get; set; }
        public string attr5 { get; set; }
        public string attr6 { get; set; }
        public string attr7 { get; set; }
        public string attr8 { get; set; }
        public string attr9 { get; set; }
        public string attr10 { get; set; }
        public string attr11 { get; set; }
        public string attr12 { get; set; }
        public string attr13 { get; set; }
        public string attr14 { get; set; }
        public string attr15 { get; set; }

        public string attr16 { get; set; }
        public string attr17 { get; set; }
        public string attr18 { get; set; }
        public string attr19 { get; set; }
        public string attr20 { get; set; }
    }
}

