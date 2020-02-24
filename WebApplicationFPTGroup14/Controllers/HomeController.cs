using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationFPTGroup14.Models;
using WebApplicationFPTGroup14.ViewModels;

namespace WebApplicationFPTGroup14.Controllers
{
    public class HomeController : Controller
    {
        Group14Entities db = new Group14Entities();
        public ActionResult Index()
        {
            var listcity = new List<City>();
            var listuniversity = new List<University>();
            listcity = db.Cities.OrderBy(c => c.CityName).ToList();
            listuniversity = db.Universities.OrderBy(u => u.UnisersityName).ToList();
            return View(new IndexViewModel() { listCity = listcity, listUniversity = listuniversity });
        }

        [HttpPost]
        public ActionResult LoadTownship(string cityID)
        {
            int cityIDInt = int.Parse(cityID);
            var listTownship = (from m in db.Townships
                                where m.CityID == cityIDInt
                                select m).OrderBy(t => t.TownshipName).ToList();
            string result = string.Empty;
            if (listTownship.Count == 0)
            {
                result = "<option value=0>Chọn Quận/Huyện</option>";
                return Json(new { success = true, result });
            }
            else
            {
                var townshipId = Request.Cookies["townshipId"];
                foreach (var items in listTownship)
                {
                    if (townshipId != null && townshipId.Value == items.TownshipID.ToString())
                    {
                        result += "<option value='" + items.TownshipID + "' selected> " + items.TownshipName + " </option>";
                    } else
                    {
                        result += "<option value='" + items.TownshipID + "' > " + items.TownshipName + " </option>";
                    }
                }
                return Json(new { success = true, result });
            }
        }

        [HttpPost]
        public ActionResult LoadCityByUniversity(string university)
        {
            if (string.IsNullOrEmpty(university))
                return Json(new { success = true, city = -1, township = -1});

            string city = db.Universities.SingleOrDefault(p => p.UnisersityName.Equals(university)).City.CityID.ToString();
            string township = db.Universities.SingleOrDefault(p => p.UnisersityName.Equals(university)).Township.TownshipID.ToString();
            return Json(new { suscess = true, city = city, township = township });
        }
    }
}