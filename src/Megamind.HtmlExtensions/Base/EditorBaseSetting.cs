using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlExtensions.Base
{
    public class EditorBaseSetting : BaseSetting
    {
        public ClientSideEvents ClientSideEvents { get; set; } = new();
    }
}
