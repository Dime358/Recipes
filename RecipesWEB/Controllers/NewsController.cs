using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RecipesWEB.Models;
using System.IO;
using PagedList;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace RecipesWEB.Controllers
{
    public class NewsController : Controller
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


        // GET: News
        public ActionResult Index(int? page)
        {
            if (isAdminUser() || isEditor())
            {
                ViewBag.showControlls = "yes";
            }
            else
            {
                ViewBag.showControlls = "no";
            }

            var news = db.News.OrderByDescending(x => x.CreatedDate).ToList();
            int pageSize = 9;
            int pageNumber = (page ?? 1);

            return View(news.ToPagedList(pageNumber, pageSize));

        }

        // GET: News/Details/5
        public ActionResult Details(int? id)
        {
            var loggedUser = User.Identity.GetUserId();
            News news = db.News.Where(x => x.Id == id).FirstOrDefault();

            if (isAdminUser() || loggedUser == news.UserId)
            {
                ViewBag.showControlls = "yes";
            }
            else
            {
                ViewBag.showControlls = "no";
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // GET: News/Create
        [Authorize(Roles = "Admin, Editor")]
        public ActionResult Create()
        {
            return View();
        }


        // POST: News/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Editor")]
        public ActionResult Create(News news, HttpPostedFileBase ImageNews)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser loggedUser = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId());
                news.CreatedDate = DateTime.UtcNow;
                news.UserId = loggedUser.Id;
                news.UserName = loggedUser.UserName;


                if (ImageNews != null && ImageNews.ContentLength > 0)
                {
                    string displayName = ImageNews.FileName;
                    string fileExtension = Path.GetExtension(displayName);

                    if (fileExtension.ToLower() == ".jpg" || fileExtension.ToLower() == ".png" || fileExtension.ToLower() == ".jpeg")
                    {
                        string fileName = string.Format("{0}.{1}", Guid.NewGuid(), fileExtension);
                        string path = Path.Combine(Server.MapPath("~/Content/images/newsImages/"), fileName);
                        ImageNews.SaveAs(path);
                        news.imagePath = fileName;
                    }
                    else
                    {
                        news.imagePath = "defaultNewsImage.jpg";
                    }
                }
                else
                {
                    news.imagePath = "defaultNewsImage.jpg";
                }

                db.News.Add(news);
                var result = db.SaveChanges();
                if (result > 0)
                {
                    return Json(new { success = true, body = "Успешно креирана вест." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, body = "Грешка во креирањето на веста.Обидете се повторно." }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { success = false, body = "Грешка во креирањето на веста.Обидете се повторно." }, JsonRequestBehavior.AllowGet);
        }

        // GET: News/Edit/5
        public ActionResult Edit(int? id)
        {
            var loggedUser = User.Identity.GetUserId();
            News news = db.News.Where(x => x.Id == id).FirstOrDefault();


            if (!isAdminUser() && !(loggedUser == news.UserId))
            {
                return RedirectToAction("accessDenied", "Partial");
            }



            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: News/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(News news, HttpPostedFileBase ImageNews)
        {
            var loggedUser = User.Identity.GetUserId();
            News checknews = db.News.Where(x => x.Id == news.Id).FirstOrDefault();


            if (!isAdminUser() && !(loggedUser == checknews.UserId))
            {
                return RedirectToAction("accessDenied", "Partial");
            }


            if (ModelState.IsValid)
            {
                var oldNews = db.News.Where(x => x.Id == news.Id).FirstOrDefault();
                oldNews.newsTitle = news.newsTitle;
                oldNews.newsBody = news.newsBody;

                if (ImageNews != null && ImageNews.ContentLength > 0)
                {
                    string displayName = ImageNews.FileName;
                    string fileExtension = Path.GetExtension(displayName);

                    if (fileExtension.ToLower() == ".jpg" || fileExtension.ToLower() == ".png" || fileExtension.ToLower() == ".jpeg")
                    {
                        string fileName = string.Format("{0}.{1}", Guid.NewGuid(), fileExtension);
                        string path = Path.Combine(Server.MapPath("~/Content/images/newsImages/"), fileName);


                        if (oldNews.imagePath != fileName)
                        {
                            if (System.IO.File.Exists(Path.Combine(Server.MapPath("~/Content/images/newsImages/"), oldNews.imagePath)))
                            {
                                System.IO.File.Delete(Path.Combine(Server.MapPath("~/Content/images/newsImages/"), oldNews.imagePath));
                            }
                            ImageNews.SaveAs(path);
                            oldNews.imagePath = fileName;
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
                        return Json(new { success = true, body = "Успешна промена", admin = true, newsID = news.Id }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = true, body = "Успешна промена", admin = false, }, JsonRequestBehavior.AllowGet);
                    }
                    
                }
                else
                {
                    return Json(new { success = false, body = "Грешка во промената.Обидете се повторно." }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false, body = "Грешка во промената.Обидете се повторно." }, JsonRequestBehavior.AllowGet);
        }

        // GET: News/Delete/5
        public ActionResult Delete(int? id)
        {
            var loggedUser = User.Identity.GetUserId();
            News news = db.News.Where(x => x.Id == id).FirstOrDefault();

            if (!isAdminUser() && !(loggedUser == news.UserId))
            {
                return RedirectToAction("accessDenied", "Partial");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            if (news == null)
            {
                return HttpNotFound();
            }

            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var loggedUser = User.Identity.GetUserId();
            News news = db.News.Where(x => x.Id == id).FirstOrDefault();

            if (!isAdminUser() && !(loggedUser == news.UserId))
            {
                return RedirectToAction("accessDenied", "Partial");
            }

           
            string fullPath = Request.MapPath("~/Content/images/newsImages/" + news.imagePath);

            db.News.Remove(news);
            db.SaveChanges();

            //doadeno brisenje na slikata pri brisenje na vest
            if (System.IO.File.Exists(fullPath) && fullPath != "defaultNewsImage.jpg")
            {
                System.IO.File.Delete(fullPath);
            }

            return RedirectToAction("Index");
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
