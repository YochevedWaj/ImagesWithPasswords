using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagesWithPasswords.Web.Models
{
    public class ViewImageViewModel
    {
        public bool CheckForPassword { get; set; }
        public string ImagePath { get; set; }
        public int ImageViewCount { get; set; }
        public int ImageID { get; set; }
        public string ErrorMessage { get; set; }
    }
}
