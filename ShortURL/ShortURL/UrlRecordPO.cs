using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShortURL
{
    [Table("ShortURLs")]
    public class UrlRecordPO
    {
        public int Id { get; set; }
        public string ShortURL { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public DateTime LastModificationTime { get; set; } = DateTime.Now;
        public int AccessNumber { get; set; } = 0;
    }
}
