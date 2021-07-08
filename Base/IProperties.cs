using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlExtensions.ExtensionProperties;

namespace HtmlExtensions
{
    public interface IProperties
    {
        public string Title { get; set; }
        public string OnRowClickEvent { get; set; }
    }

    public class Properties : IProperties
    {
        public string Title { get; set; }
        /// <summary>
        /// data as returning value 
        /// </summary>
        public string OnRowClickEvent { get; set; }
        /// <summary>
        /// Get a value whether it show the delete button
        /// </summary>
        public bool EnableDelete { get; set; }
        /// <summary>
        /// Get a value whether it show the edit button
        /// </summary>
        public bool EnableEdit { get; set; }
        /// <summary>
        /// Get a value whether it trigger edit/detail click event
        /// </summary>
        public bool EnableRowClick { get; set; }
        /// <summary>
        /// Get a value for Custom Css of the datatable
        /// </summary>
        public string CustomCss { get; set; }
        /// <summary>
        /// Get a value for Custom Style of the datatable
        /// </summary>
        public string CustomStyle { get; set; }
        /// <summary>
        /// Get a value for extended header properties
        /// </summary>
        public HeaderProperties HeaderProperties { get; set; } = new();
    }

  
}
