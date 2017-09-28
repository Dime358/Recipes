using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RecipesWEB.Models;
using RecipesWEB.Models.viewModels;
using Microsoft.AspNet.Identity;
using PagedList;
using Microsoft.AspNet.Identity.EntityFramework;

namespace RecipesWEB.Controllers
{
    public class PartialController : Controller
    {
        private RecipesWEBContext db = new RecipesWEBContext();


        public Boolean isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        // GET: Partial
        [ChildActionOnly]
        public ActionResult NavigationRight()
        {
            if (isAdminUser())
            {
                ViewBag.admin = "yes";
            }

            var userNotifications = new notificationsViewModel();
            var loggedUser = User.Identity.GetUserId();
            if(loggedUser == null)
            {
                return PartialView("_LoginPartial");
            }
            
            var allNotifications = db.Notifications.Where(x => x.forUserId == loggedUser && x.status == "unseen").ToList();
            var topNotifications = allNotifications;

            if (allNotifications.Count() >= 5)
            {
                topNotifications = db.Notifications.Where(x => x.forUserId == loggedUser && x.status == "unseen").Take(5).ToList();
            }

            if (allNotifications.Count() >= 6)
            {
                userNotifications.viewMoreStatus = true; 
            }
            else
            {
                userNotifications.viewMoreStatus = false;
            }

            //mozna optimizacija ponatamu
            var posts = db.Posts.Where(x => x.approved == false).ToList();
            var reports = db.Reports.ToList();

            if (posts.Count > 0 || reports.Count > 0)
            {
                userNotifications.adminNum = 1;
            }
            else
            {
                userNotifications.adminNum = 0;
            }


            userNotifications.notifications = topNotifications;
            userNotifications.notificationNumber = allNotifications.Count();

            return PartialView("_LoginPartial", userNotifications);
            
        }

        [Authorize]
        public ActionResult viewNotification(int id , int postId)
        {
            var notification = db.Notifications.Where(x => x.Id == id).FirstOrDefault();
            notification.status = "seen";
            db.SaveChanges();
            return Redirect("/Posts/Details?id=" + postId);
        }

        [Authorize]
        public ActionResult notifications(int? page)
        {
            var loggedUser = User.Identity.GetUserId();
            var allnotifications = db.Notifications.Where(x => x.forUserId == loggedUser).OrderByDescending(x => x.CreatedDate).ToList();


            if (allnotifications != null)
            {
                if (allnotifications.Count() > 10)
                {
                    ViewBag.showNotificationPage = "yes";
                }
            }
           

            int pageSize = 10;
            int pageNumber = (page ?? 1);



            return View(allnotifications.ToPagedList(pageNumber, pageSize));
        }

        [Authorize]
        public ActionResult markAsSeen(int [] checkData)
        {
            var notifications = new List<Notifications>();
            var loggedUser = User.Identity.GetUserId();

            foreach (var check in checkData)
            {
               var foundNotification = db.Notifications.Where(x => x.Id == check && x.forUserId == loggedUser).FirstOrDefault();
               notifications.Add(foundNotification);
            }
            
            foreach(var item in notifications)
            {
                item.status = "seen";
            }

            db.SaveChanges();

            var redirectUrl = new UrlHelper(Request.RequestContext).Action("notifications", "Partial");
            return Json(new { Url = redirectUrl });
        }

        [Authorize]
        public ActionResult deleteNotifications(int[] checkData)
        {

            var loggedUser = User.Identity.GetUserId();
            var notifications = new List<Notifications>();

            foreach (var check in checkData)
            {
                var foundNotification = db.Notifications.Where(x => x.Id == check && x.forUserId == loggedUser).FirstOrDefault();
                notifications.Add(foundNotification);
            }

            foreach (var item in notifications)
            {
                db.Notifications.Remove(item);
            }

            db.SaveChanges();

            var redirectUrl = new UrlHelper(Request.RequestContext).Action("notifications", "Partial");
            return Json(new { Url = redirectUrl });
        }

        public ActionResult accessDenied()
        {
            return View();
        }

    }
}