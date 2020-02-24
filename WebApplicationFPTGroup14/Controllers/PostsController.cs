using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationFPTGroup14.Helpers;
using WebApplicationFPTGroup14.Models;
using WebApplicationFPTGroup14.ViewModels;

namespace WebApplicationFPTGroup14.Controllers
{
    public class PostsController : Controller
    {
        Group14Entities db = new Group14Entities();
        private readonly int PageSize = 5;

        private List<PostViewModel> CreatePostViewModel(List<SearchResult> listPosts)
        {
            var listPosstViewModel = new List<PostViewModel>();
            foreach (var item in listPosts)
            {
                var listUrlImage = db.Images.Where(i => i.HouseID.Equals(item.HouseID)).Select(i => i.Url).ToList();
                var postViewModel = new PostViewModel()
                {
                    ImageList = listUrlImage,
                    Post = item
                };
                listPosstViewModel.Add(postViewModel);
            }
            return listPosstViewModel;
        }

        private List<PostViewModel> CreatePostViewModel(int cityID, int townshipID)
        {
            var listPosts = db.SearchResults.Where(p => p.CityID == cityID && p.TownshipID == townshipID).ToList();
            var listPosstViewModel = new List<PostViewModel>();
            foreach (var item in listPosts)
            {
                var township = db.Townships.SingleOrDefault(p => p.CityID == item.CityID && p.TownshipID == item.TownshipID);
                double locationX = double.Parse(township.LocationX.ToString());
                double locationY = double.Parse(township.LocationY.ToString());
                var listUrlImage = new List<string>();
                foreach (var i in db.Images.ToList())
                {
                    if (i.HouseID == item.HouseID)
                    {
                        listUrlImage.Add(i.Url);
                    }
                }
                var postViewModel = new PostViewModel()
                {
                    ImageList = listUrlImage,
                    Post = item,
                    LocationX = locationX,
                    LocationY = locationY
                };
                listPosstViewModel.Add(postViewModel);
            }
            return listPosstViewModel;
        }

        public string GetImage(int houseID)
        {
            var url = string.Empty;
            var listImagesEntities = db.Images.Where(p => p.HouseID == houseID).ToList();
            foreach(var item in listImagesEntities)
            {
                url = item.Url;
                break;
            }
            return url;
        }

        [HttpGet]
        public ActionResult SearchResult(string pageNumber, string order)
        {
            if (order != null)
            {
                return SearchResult(Session["searchData"] as FormCollection, pageNumber, order);
            }
            else
            {
                return SearchResult(Session["searchData"] as FormCollection, pageNumber, null);
            }
        }

        [HttpPost]
        public ActionResult SearchResult(FormCollection data, string pageNumber, string order)
        {
            var listPosts = new List<PostViewModel>();
            var listResult = new List<PostViewModel>();

            if (data == null) return View(listResult);
            else Session["searchData"] = data;

            int city = Convert.ToInt32(data["city"]);
            int district = Convert.ToInt32(data["district"]);
            string room = data["room"];
            string house = data["house"];
            string isSearchAdvance = data["isSearchAdvance"];
            string price = data["price"];
            string area = data["area"];
            string innerBathRoom = data["innerBathRoom"];
            string hasPark = data["hasPark"];
            string kitchenshelf = data["kitchenshelf"];
            string mezzanine = data["mezzanine"];

            Response.Cookies["cityId"].Value = city.ToString();
            Response.Cookies["townshipId"].Value = district.ToString();
            Response.Cookies["house"].Value = house;
            Response.Cookies["room"].Value = room;

            if (city == 0 || district == 0)
            {
                ViewBag.City = "City";
                ViewBag.isHouse = "Phòng trọ / Nhà nguyên căn";
                ViewBag.TownShip = "District";
            }
            else
            {
                ViewBag.City = db.Cities.Where(p => p.CityID == city).First().CityName;
                if (room != null && house != null)
                {
                    ViewBag.isHouse = "Phòng trọ / Nhà nguyên căn";
                }
                else if (room != null)
                {
                    ViewBag.isHouse = "Phòng trọ";
                }
                else
                {
                    ViewBag.isHouse = "Nhà nguyên căn";
                }
                ViewBag.TownShip = db.Townships.Where(p => p.TownshipID == district).First().TownshipName;
            }

            // Get all post
            listPosts = CreatePostViewModel(city, district);

            // Check loai phong
            if (room != null && house == null)
            {
                listResult = listPosts.Where(p => p.Post.IsHouse == false).ToList();
            }
            else if (room == null && house != null)
            {
                listResult = listPosts.Where(p => p.Post.IsHouse == true).ToList();
            }
            else
            {
                listResult = listPosts;
            }

            if (isSearchAdvance != null)
            {
                if (price != null && long.TryParse(price, out long nPrice))
                {
                    listResult = listResult.Where(p => p.Post.Price <= nPrice).ToList();
                }
                if (area != null && long.TryParse(area, out long nArea))
                {
                    listResult = listResult.Where(p => p.Post.Acreage <= nArea).ToList();
                }
                if (innerBathRoom != null)
                {
                    listResult = listResult.Where(p => p.Post.InnerBathRoom.Value.Equals(true)).ToList();
                }
                if (hasPark != null)
                {
                    listResult = listResult.Where(p => p.Post.Parking.Value.Equals(true)).ToList();
                }
                if (kitchenshelf != null)
                {
                    listResult = listResult.Where(p => p.Post.Kitchenshelf.Value.Equals(true)).ToList();
                }
                if (mezzanine != null)
                {
                    listResult = listResult.Where(p => p.Post.Mezzanine.Value.Equals(true)).ToList();
                }
            }

            var subTotalPage = listResult.Count / PageSize;
            var totalPage = (listResult.Count % PageSize == 0) ? subTotalPage : subTotalPage + 1;

            var pageN = 1;
            if (!Int32.TryParse(pageNumber, out pageN)) pageN = 1;
            else if ((pageN < 1) || (pageN > totalPage)) pageN = 1;

            ViewBag.PageName = "SearchResult";
            ViewBag.PageSize = PageSize;
            ViewBag.PageNumber = pageN;
            ViewBag.TotalPage = totalPage;
            if (order != null)
            {
                ViewBag.Order = order;
                if (order.Equals("asc"))
                {
                    var q = from s in listResult
                            orderby s.Post.Price ascending
                            select s;
                    return View(q.ToList().Skip((pageN - 1) * PageSize).Take(PageSize));
                }
                else if (order.Equals("desc"))
                {
                    var q = from s in listResult
                            orderby s.Post.Price descending
                            select s;
                    return View(q.ToList().Skip((pageN - 1) * PageSize).Take(PageSize));
                }
                else
                {
                    return View(listResult.Skip((pageN - 1) * PageSize).Take(PageSize));
                }
            }
            else
            {
                return View(listResult.Skip((pageN - 1) * PageSize).Take(PageSize));
            }
        }

        public ActionResult SinglePost(string postID)
        {
            PostViewModel post = new PostViewModel();
            var success = UpdatePostView(postID);
            if (success)
            {
                var temp = Int32.Parse(postID);
                post.Post = db.SearchResults.FirstOrDefault(p => p.PostID.Equals(temp));
                post.ImageList = db.Images.Where(i => i.HouseID.Equals(post.Post.HouseID)).Select(i => i.Url).ToList();
                if (Request.Cookies["historyPost"] != null)
                {
                    var historyPost = Request.Cookies["historyPost"].Value;
                    if (!String.IsNullOrEmpty(historyPost))
                    {
                        post.ViewedPost = new List<SearchResult>();
                        var arr = historyPost.Split(',');
                        for (var i = arr.Length - 1; i > arr.Length - 6 && i >= 0; i--)
                        {
                            var v = Int32.Parse(arr[i]);
                            post.ViewedPost.Add(db.SearchResults.FirstOrDefault(p => p.PostID == v));
                        }
                    }

                }

                post.SuggestPost = new List<SearchResult>();
                var q = from s in db.SearchResults
                        where s.CityID == post.Post.CityID && s.TownshipID == post.Post.TownshipID
                        orderby s.PostView descending
                        select s;
                int j = 0;
                foreach (var i in q)
                {
                    post.SuggestPost.Add(i);
                    j++;
                    if (j > 3)
                        break;
                }
                return View(post);
            }
            return View(post);
        }

        private bool UpdatePostView(string postID)
        {
            var result = false;
            if (!String.IsNullOrEmpty(postID))
            {
                var temp = 0;
                if (Int32.TryParse(postID, out temp))
                {
                    var current = db.Posts.FirstOrDefault(p => p.PostID.Equals(temp));
                    if (current != null)
                    {
                        current.PostView = current.PostView + 1;
                        db.SaveChanges();
                        result = true;
                    }
                }
            }
            return result;
        }

        [HttpGet]
        public ActionResult UploadPost()
        {
            if (Session["account"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (Session["accountRole"].Equals("1"))
                {
                    return RedirectToAction("AllPost", "Admin");
                }
            }
            string accountID = Session["account"].ToString();
            var listMessage = db.Messengers.Where(p => p.Receiver.Equals(accountID) && p.IsRead == false).ToList();
            string message = string.Empty;
            foreach (var item in listMessage)
            {
                message += "<li class='list-group-item'><div class='w-100'><a href='ReadMess?receiver=" + item.Receiver + "&sentTime=" + item.SentTime + "'>Xoá tin  </a>" + item.Message + "</div><div class='w-100 text-muted'>" + "Thời gian gửi: " + item.SentTime + "</div></li>";
            }
            ViewBag.mess = message;
            ViewBag.count = listMessage.Count();
            var cityList = db.Cities.OrderBy(c => c.CityName).ToList();
            return View(cityList);
        }

        [HttpPost]
        public ActionResult UploadPost(FormCollection data)
        {
            var maxHouseID = db.HouseForRents.Max(h => h.HouseID);
            var innerBathRoom = data["innerBathRoom"] == null ? false : true;
            var parking = data["parking"] == null ? false : true;
            var kitchenshelf = data["kitchenshelf"] == null ? false : true;
            var mezzanine = data["mezzanine"] == null ? false : true;

            var house = new HouseForRent()
            {
                Acreage = Convert.ToInt32(data["acreage"]),
                Price = CurrencyHelper.CurrencyToNumber(data["price"], '.'),
                CityID = Convert.ToInt32(data["city"]),
                TownshipID = Convert.ToInt32(data["district"]),
                InnerBathRoom = innerBathRoom,
                Parking = parking,
                MaxPeople = Convert.ToInt32(data["maxPeople"]),
                StatusID = 1,
                Note = data["addressDetail"],
                AccountID = Session["account"].ToString(),
                Address = data["addressRoom"],
                IsHouse = data["type"].Equals("0") ? false : true,
                Kitchenshelf = kitchenshelf,
                Mezzanine = mezzanine
            };

            // Tai hinh anh len
            var urlList = UploadFile(Request.Files);
            if (urlList.Count > 0)
            {
                // Luu thong tin phong tro
                db.HouseForRents.Add(house);
                db.SaveChanges();

                var houseId = db.HouseForRents.Max(h => h.HouseID);
                foreach (var url in urlList)
                {
                    // Luu hinh anh
                    SaveImage(url, houseId);
                }
                var post = new Post()
                {
                    HouseID = houseId,
                    PostTitle = data["postTitle"],
                    Description = data["description"],
                    PostDay = DateTime.Now,
                    PostView = 0,
                    Status = false
                };
                db.Posts.Add(post);
                db.SaveChanges();
            }
            return RedirectToAction("UploadPost", "Posts");
        }

        private void SaveImage(string url, int houseId)
        {
            var image = new Image() { ImageID = 6, Url = url, HouseID = houseId };
            db.Images.Add(image);
            db.SaveChanges();
        }

        private List<string> UploadFile(HttpFileCollectionBase files)
        {
            var urlList = new List<string>();
            var maxImageId = db.Images.Max(i => i.ImageID);
            try
            {
                for (int i = 0; i < files.Count; i++)
                {
                    if (files[i].ContentLength > 0)
                    {
                        var fileName = System.IO.Path.GetFileNameWithoutExtension(files[i].FileName);
                        var newFileName = files[i].FileName.Replace(fileName, (maxImageId + 1).ToString());
                        var filePath = Server.MapPath("~/Content/Images/Houserent/" + newFileName);
                        files[i].SaveAs(filePath);
                        urlList.Add(newFileName);
                        maxImageId++;
                    }
                }
                return urlList;
            }
            catch (Exception) { return null; }
        }

        public ActionResult Discover()
        {
            #region load 4 listPost
            var listPosstViewModel = new List<PostViewModel>();
            //load new post
            var q = from s in db.SearchResults
                    orderby s.PostDay descending
                    select s;
            var listNewPosts = CreatePostViewModel(q.ToList());

            //load most view
            q = from s in db.SearchResults
                orderby s.PostView descending
                select s;
            var listMostView = CreatePostViewModel(q.ToList());

            //load Cheapest
            q = from s in db.SearchResults
                orderby s.Price ascending
                select s;
            var listPostsCheapest = CreatePostViewModel(q.ToList());

            //load post in month
            var month = DateTime.Now.Month;
            var listResult = db.SearchResults.Where(p => p.PostDay.Value.Month == month).OrderBy(p => p.PostDay).ToList();
            var listViewInMonth = CreatePostViewModel(listResult);
            #endregion

            var discover = new DiscoverViewModel()
            {
                listMostView = listMostView,
                listNewPosts = listNewPosts,
                listPostsCheapest = listPostsCheapest,
                listPostsInMonth = listViewInMonth
            };
            return View(discover);
        }

        [HttpGet]
        public ActionResult AllPosts(string pageNumber)
        {
            if (Session["account"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (Session["accountRole"].Equals("1"))
                {
                    return RedirectToAction("AllPost", "Admin");
                }
            }

            int pageN = 1;
            if (pageNumber != null)
            {
                pageN = Int32.Parse(pageNumber);
            }
            string accountID = Session["account"].ToString();
            var listMessage = db.Messengers.Where(p => p.Receiver.Equals(accountID) && p.IsRead == false).ToList();
            string message = string.Empty;
            foreach (var item in listMessage)
            {
                message += "<li class='list-group-item'><div class='w-100'><a href='ReadMess?receiver=" + item.Receiver + "&sentTime=" + item.SentTime + "'>Xoá tin  </a>" + item.Message + "</div><div class='w-100 text-muted'>" + "Thời gian gửi: " + item.SentTime + "</div></li>";
            }
            ViewBag.mess = message;
            List<PostViewModel> list = new List<PostViewModel>();
            var q = from s in db.SearchResults
                    where s.AccountID == accountID
                    select s;
            var SearchResult = q.ToList();
            var postList = SearchResult.Skip((pageN - 1) * PageSize).Take(PageSize).ToList();
            if (postList != null && postList.Count() > 0)
            {
                foreach (var item in postList)
                {
                    list.Add(new PostViewModel()
                    {
                        Post = item,
                        ImageList = db.Images.Where(i => i.HouseID.Equals(item.HouseID)).Select(i => i.Url).ToList()
                    });
                }
            }
            var subTotalPage = SearchResult.Count / PageSize;
            var totalPage = (SearchResult.Count % PageSize == 0) ? subTotalPage : subTotalPage + 1;
            ViewBag.PageNumber = pageN;
            ViewBag.TotalPage = totalPage;
            return View(list);
        }

        [HttpGet]
        public ActionResult ReadMess(string receiver, string sentTime)
        {
            var time = DateTime.Parse(sentTime);
            using (var db = new Group14Entities())
            {
                Messenger mess = null;
                var list = db.Messengers.Where(p => p.Receiver.Equals(receiver)).ToList();
                foreach (var item in list)
                {
                    if (item.SentTime.ToString() == time.ToString())
                    {
                        mess = item;
                    }
                }
                if (mess != null)
                {
                    mess.IsRead = true;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("AllPosts", "Posts");
        }
    }
}