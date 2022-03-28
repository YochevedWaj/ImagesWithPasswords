using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagesWithPasswords.Data
{
    public class Image
    {
        public int ID { get; set; }
        public string FileName { get; set; }
        public string Password { get; set; }
        public int ViewCount { get; set; }
    }
}
