using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationFPTGroup14.Models;
using WebApplicationFPTGroup14.ViewModels;

namespace WebApplicationFPTGroup14.Controllers
{
    public class AdminController : Controller
    {
        Group14Entities db = new Group14Entities();
        private readonly int pageSize = 10;

        public ActionResult PostForApproval(string pageNumber)
        {
            if (Session["account"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if(Session["accountRole"].Equals("0"))
                {
                    return RedirectToAction("AllPosts", "Posts");
                }
            }
            int pageN = 1;
            if (pageNumber != null)
            {
                pageN = Int32.Parse(pageNumber);
            }

            List<PostViewModel> list = new List<PostViewModel>();
            var SearchResult = db.SearchResults.Where(p => p.Status.Value.Equals(false)).ToList();
            var postList = SearchResult.Skip((pageN - 1) * pageSize).Take(pageSize).ToList();
            if (postList != null && postList.Count > 0)
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
            var subTotalPage = SearchResult.Count / pageSize;
            var totalPage = (SearchResult.Count % pageSize == 0) ? subTotalPage : subTotalPage + 1;
            ViewBag.PageNumber = pageN;
            ViewBag.TotalPage = totalPage;
            ViewBag.NumberDisApprovalPost = CountDisApprovalPost();
            return View(list);
        }

        [HttpGet]
        public ActionResult AllPost(string pageNumber)
        {
            if (Session["account"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (Session["accountRole"].Equals("0"))
                {
                    return RedirectToAction("AllPosts", "Posts");
                }
            }
            int pageN = 1;
            if (pageNumber != null)
            {
                pageN = Int32.Parse(pageNumber);
            }
            List<PostViewModel> list = new List<PostViewModel>();
            var SearchResult = db.SearchResults.ToList();
            var postList = SearchResult.Skip((pageN - 1) * pageSize).Take(pageSize).ToList();
            if (postList != null && postList.Count > 0)
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
            var subTotalPage = SearchResult.Count / pageSize;
            var totalPage = (SearchResult.Count % pageSize == 0) ? subTotalPage : subTotalPage + 1;
            ViewBag.PageNumber = pageN;
            ViewBag.TotalPage = totalPage;
            return View(list);
        }

        public ActionResult AllAccount(string pageNumber)
        {
            if (Session["account"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (Session["accountRole"].Equals("0"))
                {
                    return RedirectToAction("AllPosts", "Posts");
                }
            }
            int pageN = 1;
            if (pageNumber != null)
            {
                pageN = Int32.Parse(pageNumber);
            }
            var listAccountManagerModel = new List<AccountManagerModel>();
            var listAllAccount = db.Accounts.ToList();
            var listAccount = listAllAccount.Skip((pageN - 1) * pageSize).Take(pageSize).ToList();
            foreach (var item in listAccount)
            {
                var accountInfo = db.Infoes.SingleOrDefault(p => p.AccountID == item.AccountID);
                var accountStatus = db.AccountStatus.SingleOrDefault(p => p.AccountStatusID == item.AccountStatusID);
                City city = null;
                Township township = null;
                if (accountInfo != null)
                {
                    city = db.Cities.SingleOrDefault(p => p.CityID == accountInfo.CityID);
                    township = db.Townships.SingleOrDefault(p => p.TownshipID == accountInfo.TownshipID);
                }

                var accountManagerModelItem = new AccountManagerModel()
                {
                    Account = item,
                    AccountStatus = accountStatus,
                    AccountInfo = accountInfo,
                    City = city,
                    TownShip = township
                };
                listAccountManagerModel.Add(accountManagerModelItem);
            }
            var subTotalPage = listAllAccount.Count / pageSize;
            var totalPage = (listAllAccount.Count % pageSize == 0) ? subTotalPage : subTotalPage + 1;
            ViewBag.PageNumber = pageN;
            ViewBag.TotalPage = totalPage;
            return View(listAccountManagerModel);
        }

        public ActionResult AccountForApproval(string pageNumber)
        {
            if (Session["account"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (Session["accountRole"].Equals("0"))
                {
                    return RedirectToAction("AllPosts", "Posts");
                }
            }
            int pageN = 1;
            if (pageNumber != null)
            {
                pageN = Int32.Parse(pageNumber);
            }
            var listAccountManagerModel = new List<AccountManagerModel>();
            var listAllAccount = db.Accounts.Where(p=>p.AccountStatusID == 2).ToList();
            var listAccount = listAllAccount.Skip((pageN - 1) * pageSize).Take(pageSize).ToList();
            foreach (var item in listAccount)
            {
                var accountInfo = db.Infoes.SingleOrDefault(p => p.AccountID == item.AccountID);
                var accountStatus = db.AccountStatus.SingleOrDefault(p => p.AccountStatusID == item.AccountStatusID);
                City city = null;
                Township township = null;
                if (accountInfo != null)
                {
                    city = db.Cities.SingleOrDefault(p => p.CityID == accountInfo.CityID);
                    township = db.Townships.SingleOrDefault(p => p.TownshipID == accountInfo.TownshipID);
                }

                var accountManagerModelItem = new AccountManagerModel()
                {
                    Account = item,
                    AccountStatus = accountStatus,
                    AccountInfo = accountInfo,
                    City = city,
                    TownShip = township
                };
                listAccountManagerModel.Add(accountManagerModelItem);
            }
            var subTotalPage = listAllAccount.Count / pageSize;
            var totalPage = (listAllAccount.Count % pageSize == 0) ? subTotalPage : subTotalPage + 1;
            ViewBag.PageNumber = pageN;
            ViewBag.TotalPage = totalPage;
            return View(listAccountManagerModel);
        }

        private int CountDisApprovalPost()
        {
            var disApprovalPosts = db.Posts.Where(p => p.Status.Value == false);
            return disApprovalPosts.Count();
        }
        #region Controller for Ajax

        [HttpPost]
        public ActionResult FeedbackForUser(string accountID,string message, int postID)
        {
            try
            {
                //var messenger = new Messenger()
                //{
                //    Receiver = accountID,
                //    IsRead = false,
                //    Message = message,
                //    SentTime = DateTime.Now
                //};
                //db.Messengers.Add(messenger);
                var post = db.Posts.Single(p => p.PostID == postID);
                db.Posts.Remove(post);
                db.SaveChanges();
                return Json(new { success = true, result = true }, JsonRequestBehavior.AllowGet);
            }catch
            {
                return Json(new { success = true, result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult BlockAccount(string accountID)
        {
            var account = db.Accounts.Single(p => p.AccountID.Equals(accountID));
            account.AccountStatusID = 3;
            db.SaveChanges();
            return Json(new { success = true, result = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UnBlockAccount(string accountID)
        {
            var account = db.Accounts.Single(p => p.AccountID.Equals(accountID));
            account.AccountStatusID = 1;
            db.SaveChanges();
            return Json(new { success = true, result = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ApprovalAccount(string accountID)
        {
            var account = db.Accounts.Single(p => p.AccountID.Equals(accountID));
            account.AccountStatusID = 1;
            db.SaveChanges();
            return Json(new { success = true, result = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ApprovalPost(int postId)
        {
            var current = db.Posts.FirstOrDefault(p => p.PostID.Equals(postId));
            current.Status = true;
            db.SaveChanges();
            return Json(new { success = true, result = current.Status }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DissApprovalPost(int postId)
        {
            var current = db.Posts.FirstOrDefault(p => p.PostID.Equals(postId));
            current.Status = false;
            db.SaveChanges();
            return Json(new { success = true, result = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}