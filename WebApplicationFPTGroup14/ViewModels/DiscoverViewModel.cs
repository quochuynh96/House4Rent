using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplicationFPTGroup14.Models;

namespace WebApplicationFPTGroup14.ViewModels
{
    public class DiscoverViewModel
    {
        public List<PostViewModel> listNewPosts { get; set; }
        public List<PostViewModel> listPostsInMonth { get; set; }
        public List<PostViewModel> listPostsCheapest { get; set; }
        public List<PostViewModel> listMostView  { get; set; }
    }
}