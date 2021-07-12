using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HtmlExtensions.Base;

namespace HtmlExtensions.FileUpload
{
    public class FileUploadSettings:BaseSetting
    {
        public FileUploadDetailSetting FileUploadDetailSetting { get; set; } = new();
        public ClientSideEvents ClientSideEvents { get; set; } = new();
    }

    public class FileUploadDetailSetting:BaseDetailSetting
    {
        public string UploadUrl { get; set; }
    }

    public class FilesProperties
    {
        private HttpFileCollection getFiles => HttpContext.Current.Request.Files;
        private HttpPostedFile File => getFiles.Count > 0 ? getFiles[0] : null;
        public string FileName => File?.FileName;
        public float? Length => File?.ContentLength;
        public string ContentType => File?.ContentType;
        public async Task<byte[]> ToByteArrayAsync()
        {
            var ms = new MemoryStream();
            await File.InputStream.CopyToAsync(ms);
            return ms.ToArray();
        }
        public byte[] ToByteArray()
        {
            var ms = new MemoryStream();
            File.InputStream.CopyTo(ms);
            return ms.ToArray();
        }

    }
}
