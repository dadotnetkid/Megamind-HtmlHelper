using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HtmlExtensions.Base;
using HtmlExtensions.Utils;
using Newtonsoft.Json;

namespace HtmlExtensions.DataTables
{
    public static class DataTableExtension
    {
        /// <summary>
        /// TODO: Render DataTable
        /// </summary>
        /// <param name="megamindExtension"></param>
        /// <param name="settings"></param>
        public static void DataTable(this MegamindExtension megamindExtension, Action<DataTableSetting> settings)
        {
            DataTableSetting _settings = new DataTableSetting();
            settings(_settings);
            _settings.IMegamindExtension = megamindExtension;
            Render(_settings);
        }

        private static void Render(DataTableSetting megamindExtension)
        {

            var writer = new MegamindWriter(megamindExtension.IMegamindExtension.HtmlHelper.ViewContext.Writer);
            var isEditing = Convert.ToBoolean(HttpContext.Current.Request.Params["IsEditing"]?.ToString());
            if (!isEditing)
                Body(megamindExtension);

        }

        private static void Body(DataTableSetting megamindExtension)
        {
            var writer = new MegamindWriter(megamindExtension.IMegamindExtension.HtmlHelper.ViewContext.Writer);
            writer.WriteLine("<div id='container-" + megamindExtension.Name + "'>");
            writer.WriteLine("</div>");
            writer.WriteLine(
                "<table id=\"" + megamindExtension.Name +
                "\" class=\"table table-striped table-bordered " + megamindExtension.Properties.CustomCss + "\" style=\"width:100%;" + megamindExtension.Properties.CustomStyle + "\">");

            AddDataTableColumns(megamindExtension, writer);

            writer.WriteLine("</thead>");


            writer.WriteLine("</table>");

            writer.WriteLine("<script>")
                .WriteLine("var " + megamindExtension.Name + "={}")
                .WriteLine("$(document).ready(function() {")
                .WriteLine("var table= $('#" + megamindExtension.Name + "').DataTable( {")
                .WriteLine("\"processing\": true,")
                .WriteLine("\"serverSide\": " + megamindExtension.ServerSide.ToString().ToLower() + ",");
            if (megamindExtension.ServerSide)
                writer.WriteLine("\"ajax\": \"" + megamindExtension.DetailSetting.ApiRoute + "\",");
            else
                writer.WriteLine("data:" + JsonConvert.SerializeObject(megamindExtension.DataSource) + ",");
            writer.WriteLine("'columns':[");

            foreach (var i in megamindExtension.Columns.ColumnCollection)
            {
                if (!string.IsNullOrEmpty(i.Properties.DisplayFormatString) && i.Properties.Type == typeof(DateTime))
                {
                    writer.WriteLine("{'data':'" + i.Name + "', 'render': function(value){")
                        .WriteLine("  if (value === null) return '';")
                        .WriteLine("value=moment(value).format('" + i.Properties.DisplayFormatString + "')")
                        .WriteLine(" return value ")
                        .WriteLine("}},");
                }
                else
                {
                    writer.WriteLine("{'data':'" + i.Name + "','className':'" + i.Properties.CustomCssClass + "'},");
                }

            }

            if (megamindExtension.Properties.EnableDelete)
            {
                writer.WriteLine(
                    "{'data':null,'className':'dt-center editor-edit','defaultContent': '<a href=\"#\"><i class=\"fa fa-pencil\"></i></a>','orderable':false},");
            }
            if (megamindExtension.Properties.EnableEdit)
            {
                writer.WriteLine("{'data':null,'className':'dt-center editor-delete','defaultContent': '<a href=\"#\"><i class=\"fa fa-trash\"></i></a>','orderable':false},");
            }
            writer.WriteLine(" ]} );");

            OnEditorDoubleClick(megamindExtension);
            OnEditorEditClick(megamindExtension);
            OnEditorDeleteClick(megamindExtension);
            //end of the document ready
            writer.WriteLine("} );");



            PerformEditCallback(megamindExtension, writer);
            PerformDeleteCallback(megamindExtension, writer);
            writer.WriteLine("</script>")
                ;
        }

        private static void PerformDeleteCallback(DataTableSetting dataTableSetting, MegamindWriter writer)
        {
            writer.WriteLine("function PerformDeleteCallback(data){")
                .WriteLine("$.post('" + dataTableSetting.DetailSetting.DeleteCallbackRoute + "',data,function(xhr){")
                .WriteLine("}")
                .WriteLine(")}");
        }

        private static void PerformEditCallback(DataTableSetting megamindExtension, MegamindWriter writer)
        {
            writer.WriteLine("function PerformEditCallback(data){")
                .WriteLine("$.post('" + megamindExtension.DetailSetting.CallbackRoute +
                           "',{IsEditing:true,data:JSON.stringify(data)},function(xhr){")
                .WriteLine("$('#container-" + megamindExtension.Name + "').html(xhr)")
                .WriteLine("})")
                .WriteLine("}")
                ;
        }

        private static void OnEditorDeleteClick(DataTableSetting dataTableSetting)
        {
            if (!dataTableSetting.Properties.EnableEdit)
            {
                return;
            }
            var writer = new MegamindWriter(dataTableSetting.IMegamindExtension.HtmlHelper.ViewContext.Writer);
            writer.WriteLine("$('#" + dataTableSetting.Name + "').on('click', 'td.editor-delete', function (e) {")
                .WriteLine("e.preventDefault();")
                .WriteLine("var data=table.row(this).data();")
                .WriteLine("PerformDeleteCallback(data)")
                .WriteLine("})");
        }

        private static void AddDataTableColumns(DataTableSetting dataTableSetting, MegamindWriter writer)
        {
            writer.WriteLine($"<thead class='{dataTableSetting.Properties.HeaderProperties.CustomCss}'>").WriteLine("<tr>");
            foreach (var i in dataTableSetting.Columns.ColumnCollection)
            {

                writer.WriteLine($"<th style='{i.Properties.InlineStyle}'>{i.Caption}</th>");
            }

            if (dataTableSetting.Properties.EnableEdit)
                writer.WriteLine($"<th></th>");
            if (dataTableSetting.Properties.EnableDelete)
                writer.WriteLine($"<th></th>");
            writer.WriteLine("</tr>");
        }

        private static void OnEditorDoubleClick(DataTableSetting dataTableSetting)
        {
            if (!dataTableSetting.Properties.EnableRowClick)
                return;
            var writer = new MegamindWriter(dataTableSetting.IMegamindExtension.HtmlHelper.ViewContext.Writer);
            writer
                .WriteLine("$('#" + dataTableSetting.Name + " tbody').on('dblclick','tr',function () {")
                .WriteLine("var data=table.row(this).data();")
                .WriteLine("PerformEditCallback(data)")
                .WriteLine("})")
                ;
        }

        private static void OnEditorEditClick(DataTableSetting dataTableSetting)
        {
            if (!dataTableSetting.Properties.EnableEdit)
            {
                return;
            }
            var writer = new MegamindWriter(dataTableSetting.IMegamindExtension.HtmlHelper.ViewContext.Writer);
            writer.WriteLine("$('#" + dataTableSetting.Name + "').on('click', 'td.editor-edit', function (e) {")
                .WriteLine("e.preventDefault();")
                .WriteLine("var data=table.row(this).data();")
                .WriteLine("PerformEditCallback(data)")
                .WriteLine("})");
        }
        /// <summary>
        /// Set a value for columns
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="action">Add column definition</param>
        public static void Add(this DataTableColumnCollection collection, Action<DataTableColumnDefinition> action)
        {
            DataTableColumnDefinition dataTableColumnDefinition = new();
            action(dataTableColumnDefinition);
            collection.ColumnCollection.Add(dataTableColumnDefinition);
        }
        /// <summary>
        /// sdfsdf
        /// </summary>
        public class DataTableSetting : BaseSetting
        {
            /// <summary>
            /// Get a value of the Columns added
            /// </summary>
            public DataTableColumnCollection Columns { get; set; } = new();
            /// <summary>
            /// Get a value that indicate details settings like Callback,Api,Update,Delete Route
            /// </summary>
            public DataTableDetailSetting DetailSetting { get; set; } = new();
            /// <summary>
            /// Set template content for edit / details on row click or edit click
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="action"></param>
            public void SetTemplateContent<T>(Action<T> action)
            {
                var isEditing = Convert.ToBoolean(HttpContext.Current.Request.Params["IsEditing"]?.ToString());

                if (isEditing)
                {
                    var str = HttpContext.Current.Request["data"];
                    var data = JsonConvert.DeserializeObject<T>(str);
                    action.Invoke(data);
                }
            }
        }

        public class DataTableDetailSetting : BaseDetailSetting
        {
            /// <summary>
            /// Get a value for postback route for partial view
            /// </summary>
            public string CallbackRoute { get; set; }
            /// <summary>
            /// Get a value for the updating actionmethod/api
            /// </summary>
            public string UpdateCallbackRoute { get; set; }
            /// <summary>
            /// Get a value for the delete actionmethod /api
            /// </summary>
            public string DeleteCallbackRoute { get; set; }
            /// <summary>
            /// Get a value for the add actionmethod /api
            /// </summary>
            public string AddNewCallbackRoute { get; set; }
        }
        public class DataTableColumnCollection
        {

            public List<DataTableColumnDefinition> ColumnCollection { get; set; } = new();
        }

        public class DataTableColumnDefinition
        {
            private string _caption;
            /// <summary>
            /// Get a value for Column Name
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Get a value for Column Text
            /// </summary>
            public string Caption
            {
                get
                {
                    if (string.IsNullOrEmpty(_caption))
                        _caption = Name;
                    return _caption;
                }
                set => _caption = value;
            }
            /// <summary>
            /// Get a value for the extended properties of the column
            /// </summary>
            public DataTableColumnProperties Properties { get; set; } = new();
        }


        public class DataTableColumnProperties
        {
            /// <summary>
            /// Get a value for formatting the values of the column
            /// </summary>
            public string DisplayFormatString { get; set; }
            public Type Type { get; set; }
            /// <summary>
            /// Set custom class to the data columns
            /// </summary>
            /// <param name="cssClass"></param>
            public void AddClass(string cssClass)
            {
                this.CustomCssClass += cssClass;

            }

            public void AddStyle(string style)
            {
                this.InlineStyle += style;
            }


            internal string CustomCssClass { get; set; }
            internal string InlineStyle { get; set; }
        }
    }




}
