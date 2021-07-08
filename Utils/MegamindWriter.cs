using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlExtensions.Utils
{
    public class MegamindWriter
    {
        private readonly TextWriter _textWriter;

        public MegamindWriter(TextWriter textWriter)
        {
            _textWriter = textWriter;

        }

        public MegamindWriter WriteLine(string content)
        {
            _textWriter.WriteLine(content);
            return new MegamindWriter(_textWriter);
        }
    }
}
