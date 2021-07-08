using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlExtensions.Base;

namespace HtmlExtensions.Select
{
    public class SelectSetting : BaseSetting
    {
        public string DisplayMember { get; set; }
        public string ValueMember { get; set; }
        public SelectDetailSetting DetailSetting { get; set; } = new();
    }
}
