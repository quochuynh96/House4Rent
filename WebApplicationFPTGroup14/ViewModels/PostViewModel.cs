using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplicationFPTGroup14.Models;

namespace WebApplicationFPTGroup14.ViewModels
{
    public class PostViewModel
    {
        public SearchResult Post { get; set; }
        public List<string> ImageList { get; set; }
        public List<SearchResult> ViewedPost { get; set; }
        public List<SearchResult> SuggestPost { get; set; }

        public string urlImage { get; set; }
        public double LocationX { get; set; }
        public double LocationY { get; set; }
    }
}