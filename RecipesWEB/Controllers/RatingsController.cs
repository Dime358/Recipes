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
using RecipesWEB.Models.viewModels;
using Microsoft.AspNet.Identity.Owin;

namespace RecipesWEB.Controllers
{
    public class RatingsController : Controller
    {
        private RecipesWEBContext db = new RecipesWEBContext();

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult newReact(postDetailsViewModel model)
        {
            var usertmp = User.Identity.GetUserId();
            var rated = db.Ratings.Single(x => x.PostsID == model.postId && x.UserId == usertmp);

            if (rated == null)
            {
                var newReact = new Ratings()
                {
                    PostsID = model.postId,
                    UserId = User.Identity.GetUserId(),
                    reacted = true
                };

                db.Ratings.Add(newReact);
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
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }


        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult changeRating(postDetailsViewModel model, int state)
        {
            Posts posts = db.Posts.Where(x => x.Id == model.postId).FirstOrDefault();
            ApplicationUser loggedUser = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId());
            var postOwner = posts.UserId;
            var oldRating = posts.postRating;

            UserRatings rating = db.UserRatings.Where(x => x.UserId == posts.UserId).FirstOrDefault();
            var rated = db.Ratings.Where(x => x.PostsID == model.postId && x.UserId == loggedUser.Id).FirstOrDefault();

            if (rated == null)
            {
                Ratings newReact = new Ratings();
                newReact.PostsID = model.postId;
                newReact.UserId = loggedUser.Id;
                if (state == 1)
                {
                    posts.postRating += 1;
                    rating.rating += 1;
                    newReact.reacted = true;
                }
                else
                {
                    posts.postRating -= 1;
                    rating.rating -= 1;
                    newReact.reacted = false;
                }

                db.Ratings.Add(newReact);
                db.SaveChanges();

                return Json(new { success = true, state = posts.postRating }, JsonRequestBehavior.AllowGet);
            }
            else
            {

                if (state == 1 && rated.reacted == false)
                {
                    posts.postRating += 2;
                    rating.rating += 2;
                    rated.reacted = true;

                    db.SaveChanges();

                    if (loggedUser.Id != postOwner)
                    {
                        var newNotification = new Notifications()
                        {
                            forUserId = postOwner,
                            fromUserName = loggedUser.UserName,
                            postId = posts.Id,
                            postTitle = posts.postTitle,
                            notificationType = "like",
                            status = "unseen",
                            CreatedDate = DateTime.UtcNow
                        };
                        db.Notifications.Add(newNotification);
                        db.SaveChanges();
                    }

                    return Json(new { success = true , state = posts.postRating }, JsonRequestBehavior.AllowGet);
                }
                else if (state == 2 && rated.reacted == true)
                {

                    posts.postRating -= 2;
                    rating.rating -= 2;
                    rated.reacted = false;

                    db.SaveChanges();

                    return Json(new { success = true , state = posts.postRating }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            
        }

        public ActionResult createRating(string userId, string type)
        {

            var model = new UserRatings();
            model.UserId = userId;
            model.rating = 0;
            db.UserRatings.Add(model);
            db.SaveChanges();

            //dodavanje nova slika
            userImages newUserImage = new userImages();
            newUserImage.image = "defaultUser.png";
            newUserImage.UserId = userId;
            db.userImages.Add(newUserImage);
            db.SaveChanges();

            if (type == "user")
            {
                return View();
            }
            else
            {
                return RedirectToAction("approveIndex", "Posts");
            }

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
