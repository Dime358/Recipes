using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RecipesWEB.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Security;
using Microsoft.AspNet.Identity.EntityFramework;

namespace RecipesWEB.Controllers
{
    public class CommentsController : Controller
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

        public Boolean isEditor()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Editor")
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


        [Authorize]
        public ActionResult Create()
        {
            var model = new Comments();
            ApplicationUser user = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId());
            model.UserName = user.UserName;
            model.CreatedDate = DateTime.UtcNow;
            //model.approved = false;
            model.UserId = User.Identity.GetUserId();
            ViewBag.PostId = new SelectList(db.Posts, "Id", "postTitle");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comments comments)
        {
            if (ModelState.IsValid)
            {

                var post = db.Posts.Where(x => x.Id == comments.PostsID).FirstOrDefault();
                var creatorId = post.UserId;

                ApplicationUser loggedUser = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId());

                if (loggedUser == null)
                {
                    return Json(new { success = false, body = "морате да бидете најавени за да коментирате." }, JsonRequestBehavior.AllowGet);
                }

                comments.UserId = loggedUser.Id;
                comments.UserName = loggedUser.UserName;
                comments.CreatedDate = DateTime.UtcNow;
                db.Comments.Add(comments);
                var result = db.SaveChanges();

                //da se smeni role od admin

                var allUsers = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().Users.ToList();
                var favoriteUsers = new List<ApplicationUser>();
                foreach (var foundUser in allUsers)
                {
                    if (db.Favorites.Where(x => x.userId == foundUser.Id && x.postId == comments.PostsID).FirstOrDefault() != null)
                    {
                        favoriteUsers.Add(foundUser);
                    }
                }

                //za site sto imaat favorite na receptot
                foreach (var registeredUser in favoriteUsers)
                {
                    if (registeredUser.Id != loggedUser.Id && registeredUser.Id != creatorId)
                    {
                        var newNotificationFavs = new Notifications()
                        {
                            forUserId = registeredUser.Id,
                            fromUserName = loggedUser.UserName,
                            postId = post.Id,
                            postTitle = post.postTitle,
                            status = "unseen",
                            notificationType = "favorite",
                            CreatedDate = DateTime.UtcNow
                        };
                        db.Notifications.Add(newNotificationFavs);
                        db.SaveChanges();
                    }

                }

                //za kreatorot na receptot
                if (loggedUser.Id != creatorId)
                {
                    var newNotification = new Notifications()
                    {
                        forUserId = creatorId,
                        fromUserName = loggedUser.UserName,
                        postId = post.Id,
                        postTitle = post.postTitle,
                        notificationType = "normal",
                        status = "unseen",
                        CreatedDate = DateTime.UtcNow
                    };
                    db.Notifications.Add(newNotification);
                    db.SaveChanges();
                }

                var imagePath = db.userImages.Where(x => x.UserId == loggedUser.Id).FirstOrDefault();


                if (result > 0)
                {
                    return Json(new { success = true, commentID = comments.Id ,userImage = imagePath.image, userName = loggedUser.UserName, createdDate = comments.CreatedDate.ToShortDateString(), createdTime = comments.CreatedDate.ToShortTimeString() }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
            }


            //ViewBag.PostId = new SelectList(db.Posts, "Id", "postTitle", comments.PostId);

            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comments comments = db.Comments.Find(id);
            if (comments == null)
            {
                return HttpNotFound();
            }
            return View(comments);
        }

        // POST: Comments/Delete/5
        //пост ид да се истргне
        public ActionResult DeleteConfirmed(int id , int postId)
        {

            var loggedUser = User.Identity.GetUserId();
            Comments comment = db.Comments.Where(x => x.Id == id).FirstOrDefault();

            if (!isAdminUser() && !(loggedUser == comment.UserId))
            {
                return RedirectToAction("accessDenied", "Partial");
            }

            db.Comments.Remove(comment);

            db.Reports.RemoveRange(db.Reports.Where(x => x.commentId == id));
           
            var result = db.SaveChanges();

            if (result > 0)
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false}, JsonRequestBehavior.AllowGet);
            }


        }

        public ActionResult addReport(int id, int? postId, string reason)
        {
            var tmp = User.Identity.GetUserId();
            Reports report = db.Reports.Where(x => x.commentId == id && x.UserId == tmp).FirstOrDefault();

            if (report == null)
            {

                var newReport = new Reports()
                {
                    commentId = id,
                    UserId = User.Identity.GetUserId(),
                    reportReason = reason
                };

                db.Reports.Add(newReport);
                var result = db.SaveChanges();

                if (result > 0)
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
