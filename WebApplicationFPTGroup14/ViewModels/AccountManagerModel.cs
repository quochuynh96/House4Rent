using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplicationFPTGroup14.Models;

namespace WebApplicationFPTGroup14.ViewModels
{
    public class AccountManagerModel
    {
        public Account Account { get; set; }
        public AccountStatu AccountStatus { get; set; }
        public Info AccountInfo { get; set; }
        public City City { get; set; }
        public Township TownShip { get; set; }
    }
}