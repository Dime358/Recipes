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
using PagedList;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;

namespace RecipesWEB.Controllers
{
    public class PostsController : Controller
    {
        private RecipesWEBContext db = new RecipesWEBContext();


        //auth funkcii

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

        // GET: Posts
        public ActionResult Index(string sortType, string cat, int? page)
        {

            ViewBag.Sort = sortType;
            ViewBag.Category = cat;

            var posts = new List<Posts>();

            if (cat == null || cat == "сите")
            {
                posts = db.Posts.Where(x => x.approved == true).ToList();
            }
            if (cat != "сите" && cat != null)
            {
                
                var categoryArraySplit = cat.Split('|');
                foreach (var category in categoryArraySplit)
                {
                   var categoryPosts = db.Posts.Where(x => x.approved == true && x.postCategory == category).ToList();
                   foreach(var post in categoryPosts)
                    {
                        posts.Add(post);
                    }
                }
            }

            switch (sortType)
            {
                case "title":
                    posts = posts.OrderBy(x => x.postTitle).ToList();
                    break;
                case "date":
                    posts = posts.OrderByDescending(x => x.CreatedDate).ToList();
                    break;
                case "category":
                    posts = posts.OrderBy(x => x.postCategory).ToList();
                    break;
                case "rating":
                    posts = posts.OrderByDescending(x => x.postRating).ToList();
                    break;
                default:
                    posts = posts.OrderByDescending(x => x.CreatedDate).ToList();
                    break;
            }

            int pageSize = 9;
            int pageNumber = (page ?? 1);

            return View(posts.ToPagedList(pageNumber, pageSize));

        }

        // GET: Posts/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var loggedUser = User.Identity.GetUserId();
            Posts posts = db.Posts.Where(x => x.Id == id).FirstOrDefault();

            if (isAdminUser() || loggedUser == posts.UserId )
            {
                ViewBag.showControlls = "yes";
            }

            if (loggedUser != null)
            {
                ViewBag.showCommentControls = "yes";
            }

            var allcomments = db.Comments.Where(x => x.PostsID == id).OrderByDescending(x => x.CreatedDate).ToList();
            var comments = new List<Tuple<Comments, string , string>>();

            foreach ( var item in allcomments)
            {
                var imagePath = db.userImages.Where(x => x.UserId == item.UserId).FirstOrDefault();

                var showDelete = "no";
                if (item.UserId == loggedUser || isAdminUser())
                {
                    showDelete = "yes";
                }

                comments.Add(Tuple.Create(item,imagePath.image,showDelete));
            }

            Ratings rated = db.Ratings.Where(x => x.PostsID == id && x.UserId == loggedUser).FirstOrDefault();
            var postsByCategory = db.Posts.Where(x => x.postCategory == posts.postCategory && x.Id != posts.Id && x.approved == true).OrderByDescending(x => x.postRating).Take(12).ToList();
            var postsByUser = db.Posts.Where(x => x.UserId == posts.UserId && x.Id != posts.Id && x.approved == true).OrderByDescending(x => x.postRating).Take(5).ToList();

            //favorites
            if(loggedUser != null)
            {
                ViewBag.showFavorite = "yes";

                var favorites = db.Favorites.Where(x => x.userId == loggedUser && x.postId == id).FirstOrDefault();

                if (favorites == null)
                {
                    ViewBag.favorite = "notFavorite";
                }
                else
                {
                    ViewBag.favorite = "Favorite";
                }
            }
            if (posts == null)
            {
                return HttpNotFound();
            }

            
            if (rated != null)
            {
                if (rated.reacted == true)
                {
                    ViewBag.votedState = "like";
                }
                else
                {
                    ViewBag.votedState = "dislike";
                }
            }


            if (comments == null || comments.Count == 0 )
            {
                ViewBag.showComments = "no";
            }

            if (!posts.approved && isAdminUser())
            {
                ViewBag.showAdminApprove = "yes";
            }



            var model = new postDetailsViewModel()
            {
                postId = posts.Id,
                UserId = posts.UserId,
                post = posts,
                comments = comments,
               // reacted = voted,
                LoggedUserName = User.Identity.GetUserName(),
                postsByCategory = postsByCategory,
                postsByUser = postsByUser,
            };

            return View(model);
        }


        // GET: Posts/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Posts post, HttpPostedFileBase ImagePost)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser loggedUser = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId());

                Posts newPost = new Posts();
                newPost = post;
                newPost.CreatedDate = DateTime.UtcNow;
                newPost.postRating = 0;

                if (isAdminUser())
                {
                    newPost.approved = true;
                }
                else
                {
                    newPost.approved = false;
                }

                newPost.approveState = "Нов рецепт";
                newPost.UserId = loggedUser.Id;
                newPost.UserName = loggedUser.UserName;

                if (ImagePost != null && ImagePost.ContentLength > 0)
                {
                    string displayName = ImagePost.FileName;
                    string fileExtension = Path.GetExtension(displayName);

                    if (fileExtension.ToLower() == ".jpg" || fileExtension.ToLower() == ".png" || fileExtension.ToLower() == ".jpeg")
                    {
                        string fileName = string.Format("{0}.{1}", Guid.NewGuid(), fileExtension);
                        string path = Path.Combine(Server.MapPath("~/Content/images/postImages/"), fileName);
                        ImagePost.SaveAs(path);
                        newPost.imagePath = fileName;
                    }
                    else
                    {
                        newPost.imagePath = "defaultPostImage.jpg";
                    }
                }
                else
                {
                    newPost.imagePath = "defaultPostImage.jpg";
                }

                db.Posts.Add(newPost);
                var result = db.SaveChanges();

                if (result > 0)
                {
                    return Json(new { success = true, body = "Успешно креиран рецепт." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, body = "Грешка во креирањето на рецепт.Обидете се повторно." }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false, body = "Грешка во креирањето на рецепт.Обидете се повторно." }, JsonRequestBehavior.AllowGet);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            var loggedUser = User.Identity.GetUserId();
            Posts posts = db.Posts.Where(x => x.Id == id).FirstOrDefault();

            if (!isAdminUser() && !(loggedUser == posts.UserId))
            {
                return RedirectToAction("accessDenied", "Partial");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Posts posts = db.Posts.Find(id);
            if (posts == null)
            {
                return HttpNotFound();
            }
            return View(posts);
        }

        // POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Posts posts , HttpPostedFileBase ImagePost)
        {
            var loggedUser = User.Identity.GetUserId();
            Posts checkPost = db.Posts.Where(x => x.Id == posts.Id).FirstOrDefault();

            if (!isAdminUser() && !(loggedUser == checkPost.UserId))
            {
                return RedirectToAction("accessDenied", "Partial");
            }

            if (ModelState.IsValid)
            {
                var oldPost = db.Posts.Where(x => x.Id == posts.Id).FirstOrDefault();
                oldPost.postTitle = posts.postTitle;
                oldPost.postBody = posts.postBody;
                oldPost.postCategory = posts.postCategory;
                oldPost.difficulty = posts.difficulty;
                oldPost.numPortions = posts.numPortions;
                oldPost.prepTime = posts.prepTime;
                oldPost.postTags = posts.postTags;
                oldPost.ingredients = posts.ingredients;

                if (isAdminUser())
                {
                    oldPost.approved = true;
                }
                else
                {
                    oldPost.approved = false;
                }
                
                oldPost.approveState = "едитиран рецепт";

                

                if (ImagePost != null && ImagePost.ContentLength > 0)
                {
                    string displayName = ImagePost.FileName;
                    string fileExtension = Path.GetExtension(displayName);

                    if (fileExtension.ToLower() == ".jpg" || fileExtension.ToLower() == ".png" || fileExtension.ToLower() == ".jpeg")
                    {
                        string fileName = string.Format("{0}.{1}", Guid.NewGuid(), fileExtension);
                        string path = Path.Combine(Server.MapPath("~/Content/images/postImages/"), fileName);
                       

                        if (oldPost.imagePath != fileName)
                        {
                            if (System.IO.File.Exists(Path.Combine(Server.MapPath("~/Content/images/postImages/"), oldPost.imagePath)))
                            {
                                System.IO.File.Delete(Path.Combine(Server.MapPath("~/Content/images/postImages/"), oldPost.imagePath));
                            }
                            ImagePost.SaveAs(path);
                            oldPost.imagePath = fileName;
                        }
                        else
                        {
                           // posts.imagePath = oldPost.imagePath;
                        }
                        
                    }
                
                }

                // db.Entry(posts).State = EntityState.Modified;
                var result = db.SaveChanges();
                if (result > 0)
                {
                    if (isAdminUser())
                    {
                        return Json(new { success = true, body = "Успешно променет рецепт." , admin = true , postID = posts.Id}, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = true, body = "Успешно променет рецепт." , admin = false }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { success = false, body = "Грешка во промената на рецептот.Обидете се повторно." }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false, body = "Грешка во промената на рецептот.Обидете се повторно." }, JsonRequestBehavior.AllowGet);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var loggedUser = User.Identity.GetUserId();
            Posts posts = db.Posts.Where(x => x.Id == id).FirstOrDefault();

            //Posts posts = db.Posts.Find(id);
            if (posts == null)
            {
                return HttpNotFound();
            }

            //proverka
            if (!isAdminUser() && !(loggedUser == posts.UserId))
            {
                return RedirectToAction("accessDenied", "Partial");
            }

            return View(posts);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Posts posts = db.Posts.Find(id);
            var loggedUser = User.Identity.GetUserId();
            

            if (!isAdminUser() && !(loggedUser == posts.UserId))
            {
                return RedirectToAction("accessDenied", "Partial");
            }


            string fullPath = Request.MapPath("~/Content/images/postImages/" + posts.imagePath);

            db.Posts.Remove(posts);
            db.Notifications.RemoveRange(db.Notifications.Where(x => x.postId == id));
            db.Favorites.RemoveRange(db.Favorites.Where(x => x.postId == id));

            db.SaveChanges();

            //doadeno brisenje na slikata pri brisenje na post
            if (System.IO.File.Exists(fullPath) && fullPath != "defaultPostImage.jpg")
            {
                System.IO.File.Delete(fullPath);
            }

            if (isAdminUser())
            {
                return RedirectToAction("approveIndex");
            }
            else
            {
                return RedirectToAction("Index");
            }
            
        }

        [Authorize(Roles = "Admin")]
        public ActionResult approveIndex(int? pagePosts, int? pageComments)
        {
            var posts = db.Posts.Where(x => x.approved == false).ToList();
            //var comments = db.Comments.Where(x => x.UserId == User.Identity.GetUserId()).ToList();

            var reports = db.Reports.ToList();
            List<int> reportsId = new List<int>();

            foreach (var report in reports)
            {
                reportsId.Add(report.commentId);
            }

            var comments = db.Comments.Where(t => reportsId.Contains(t.Id)).ToList();

            var reportedComments = new List<Tuple<Comments, string, int>>();


            foreach (var comment in comments)
            {
                var reason1 = db.Reports.Where(t => t.commentId == comment.Id && t.reportReason == "spam").ToList();
                var reason2 = db.Reports.Where(t => t.commentId == comment.Id && t.reportReason == "offensive").ToList();
                var number = db.Reports.Where(t => t.commentId == comment.Id).ToList().Count();
                
                if(reason1.Count() > reason2.Count())
                    reportedComments.Add(Tuple.Create(comment, reason1.FirstOrDefault().reportReason, number));
                else
                    reportedComments.Add(Tuple.Create(comment, reason2.FirstOrDefault().reportReason, number));
            }



            if (posts != null)
            {
                if (posts.Count() > 12)
                {
                    ViewBag.showPostPage = "yes";
                }
            }
            if (reportedComments != null)
            {
                if (reportedComments.Count() > 12)
                {
                    ViewBag.showCommentsPage = "yes";
                }
            }


            int pageSize = 12;
            int pageNumberPosts = (pagePosts ?? 1);
            int pageNumberComments = (pageComments ?? 1);

            var model = new approveViewModel()
            {
                Posts = posts.ToPagedList(pageNumberPosts, pageSize),
                reportedComments = reportedComments.ToPagedList(pageNumberComments, pageSize)
            };

            ViewBag.pagePosts = pageNumberPosts;
            ViewBag.pageComments = pageNumberComments;

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult approveChange(int id, string tip)
        {
            if (tip == "post")
            {
                TempData["activeType"] = tip;

                Posts posts = db.Posts.Where(x => x.Id == id).FirstOrDefault();
                posts.approved = true;
                db.SaveChanges();
                return RedirectToAction("approveIndex");
                
            }
            if (tip == "comment")
            {
                TempData["activeType"] = tip;

                Comments comment = db.Comments.Where(x => x.Id == id).FirstOrDefault();
                db.Comments.Remove(comment);
                db.SaveChanges();

                db.Reports.RemoveRange(db.Reports.Where(x => x.commentId == id));
                db.SaveChanges();

                return RedirectToAction("approveIndex");
            }

            TempData["activeType"] = tip;
            return RedirectToAction("approveIndex");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult approveComment(int id)
        {
            ViewBag.activeType = "comment";

            db.Reports.RemoveRange(db.Reports.Where(x => x.commentId == id));
            db.SaveChanges();
            return RedirectToAction("approveIndex");
        }

        //test go iam i vo comments
        public ActionResult addReport(int id, int? postId)
        {
            var tmp = User.Identity.GetUserId();
            Reports report = db.Reports.Where(x => x.commentId == id && x.UserId == tmp).FirstOrDefault();

            if (report == null)
            {
                var newReport = new Reports()
                {
                    commentId = id,
                    UserId = User.Identity.GetUserId(),
                };

                db.Reports.Add(newReport);
                db.SaveChanges();

            }

            return RedirectToAction("Details", new { id = postId });
        }

        public ActionResult manageFavorites(int postId)
        {
            var loggedUser = User.Identity.GetUserId();
            var favorites = db.Favorites.Where(x => x.userId == loggedUser && x.postId == postId).FirstOrDefault();

            if (favorites == null)
            {
                var newFavorite = new Favorites()
                {
                    postId = postId,
                    userId = loggedUser
                };

                db.Favorites.Add(newFavorite);
                var result = db.SaveChanges();

                if (result > 0)
                {
                    return Json(new { success = true, status = "add" , add = "favorite", remove = "notFavorite" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false , }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                db.Favorites.Remove(favorites);
                var result = db.SaveChanges();

                if (result > 0)
                {
                    return Json(new { success = true, status = "remove" , add = "notFavorite", remove = "favorite" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, }, JsonRequestBehavior.AllowGet);
                }
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
