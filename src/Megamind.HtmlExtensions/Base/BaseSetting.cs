using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlExtensions.Base
{
    public class BaseSetting
    {
        /// <summary>
        /// Get Element Id
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Get a value for extended Element properties
        /// </summary>
        public Properties Properties { get; set; } = new();

        public IMegamindExtension IMegamindExtension { get; set; }


        /// <summary>
        /// Gets a value that indicate whether it calls using API Endpoint
        /// </summary>
        public bool ServerSide { get; set; }
        /// <summary>
        /// Get the value of the model if server is false
        /// </summary>
        public object DataSource { get; set; }
    }
}
