using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplicationFPTGroup14.Models;

namespace WebApplicationFPTGroup14.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<City> listCity { get; set; }
        public IEnumerable<University> listUniversity { get; set; }
    }
}