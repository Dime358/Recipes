using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
using RecipesWEB.Models;
using RecipesWEB.Models.viewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecipesWEB.Controllers
{

    public class HomeController : Controller
    {
        private RecipesWEBContext db = new RecipesWEBContext();

        public ActionResult Index()
        {

            var recentPostsAll = db.Posts.Where(x => x.approved == true).OrderByDescending(x => x.CreatedDate).Take(10).ToList();
            var recentPostsPredjadenja = db.Posts.Where(x => x.approved == true && x.postCategory == "Појадок").OrderByDescending(x => x.CreatedDate).Take(10).ToList();
            var recentPostsGlavni = db.Posts.Where(x => x.approved == true && x.postCategory == "Ручек").OrderByDescending(x => x.CreatedDate).Take(10).ToList();
            var recentPostsDeserti = db.Posts.Where(x => x.approved == true && x.postCategory == "Вечера").OrderByDescending(x => x.CreatedDate).Take(10).ToList();
            var recentPostsDodatoci = db.Posts.Where(x => x.approved == true && x.postCategory == "Десерт").OrderByDescending(x => x.CreatedDate).Take(10).ToList();

            var topRatingPostsAll = db.Posts.Where(x => x.approved == true && x.postRating > 0).OrderByDescending(x => x.postRating).Take(10).ToList();
            var topRatingPostsPredjadenja = db.Posts.Where(x => x.approved == true && x.postCategory == "Појадок" && x.postRating > 0).OrderByDescending(x => x.postRating).Take(10).ToList();
            var topRatingPostsGlavni = db.Posts.Where(x => x.approved == true && x.postCategory == "Ручек" && x.postRating > 0).OrderByDescending(x => x.postRating).Take(10).ToList();
            var topRatingPostsDeserti = db.Posts.Where(x => x.approved == true && x.postCategory == "Вечера" && x.postRating > 0).OrderByDescending(x => x.postRating).Take(10).ToList();
            var topRatingPostsDodatoci = db.Posts.Where(x => x.approved == true && x.postCategory == "Десерт" && x.postRating > 0).OrderByDescending(x => x.postRating).Take(10).ToList();

            var userRatings = db.UserRatings.Where(x => x.rating > 0).OrderByDescending(x => x.rating).Take(6).ToList();
            var latestNews = db.News.OrderByDescending(x => x.CreatedDate).Take(6).ToList();


            //List<ApplicationUser> topRatingUsers = new List<ApplicationUser>();

            var topRatingUsers = new List<Tuple<ApplicationUser, int , string>>();


            foreach (var userRating in userRatings)
            {
                ApplicationUser user = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(userRating.UserId);
                //topRatingUsers.Add(user);
                var images = db.userImages.Where(x => x.UserId == userRating.UserId).FirstOrDefault();
                topRatingUsers.Add(Tuple.Create(user, userRating.rating, images.image));
            }


            var model = new homePageVewModel();

            model.recentPostsAll = recentPostsAll;
            model.recentPostsPredjadenja = recentPostsPredjadenja;
            model.recentPostsGlavni = recentPostsGlavni;
            model.recentPostsDeserti = recentPostsDeserti;
            model.recentPostsDodatoci = recentPostsDodatoci;

            model.topRatingPostsAll = topRatingPostsAll;
            model.topRatingPostsPredjadenja = topRatingPostsPredjadenja;
            model.topRatingPostsGlavni = topRatingPostsGlavni;
            model.topRatingPostsDeserti = topRatingPostsDeserti;
            model.topRatingPostsDodatoci = topRatingPostsDodatoci;

            model.topRatingUsers = topRatingUsers;
            model.latestNews = latestNews;

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult userProfile(string userId, int? favPosts, int? userPosts)
        {

            if(userId == null)
            {
                return RedirectToAction("accessDenied", "Partial");
            }
            ApplicationUser user = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(userId);
            var userPostsAll = db.Posts.Where(x => x.approved == true && x.UserId == user.Id).OrderBy(x => x.CreatedDate).ToList();
            UserRatings ratingTable = db.UserRatings.Where(x => x.UserId == userId).FirstOrDefault();
            var userImages = db.userImages.Where(x => x.UserId == userId).FirstOrDefault();
            var favorites = db.Favorites.Where(x => x.userId == user.Id).ToList();

            var favoritePostsAll = new List<Posts>();

            foreach(var item in favorites)
            {
                var post = db.Posts.Where(x => x.Id == item.postId).FirstOrDefault();
                favoritePostsAll.Add(post);
            }

            if (userPostsAll.Count > 0)
            {
                if (userPostsAll.Count() > 8)
                {
                    ViewBag.showUserPosts = "yes";
                }
            }
            if (favoritePostsAll.Count > 0)
            {
                if (favoritePostsAll.Count() > 8)
                {
                    ViewBag.showFavPosts = "yes";
                }
            }


            int pageSize = 8;
            int favNumber = (favPosts ?? 1);
            int userNumber = (userPosts ?? 1);

            var model = new userProfileViewModel()
            {
                UserId = user.Id,
                UserName = user.UserName,
                rating = ratingTable.rating,
                image = userImages.image,
                numPosts = userPostsAll.Count(),
                userPostsAll = userPostsAll.ToPagedList(userNumber, pageSize),
                favoritePostsAll = favoritePostsAll.ToPagedList(favNumber, pageSize),
            };

            ViewBag.favNumber = favNumber;
            ViewBag.userNumber = userNumber;

            return View(model);

        }
        public ActionResult postStatus(string id)
        {
            ApplicationUser user = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(id);
            var unApprovedPosts = db.Posts.Where(x => x.UserId == id && x.approved == false).ToList();


            var model = new postStatus()
            {
                unApprovedPosts = unApprovedPosts
            };

            return View(model);
        }



        public ActionResult Search(string searchString, int? postNumber , int? newsNumber )
        {
            if (!String.IsNullOrEmpty(searchString))
            {

                if(searchString.Length < 3)
                {
                    ViewBag.error = "error";
                    return View();
                }

                ViewBag.searchString = searchString;

                var posts = db.Posts.Where(x => x.approved == true && x.postTitle.Contains(searchString) || x.postBody.Contains(searchString) || x.UserName.Contains(searchString) || x.postCategory.Contains(searchString) || x.ingredients.Contains(searchString)).ToList();
                var news = db.News.Where(x => x.newsTitle.Contains(searchString) || x.newsBody.Contains(searchString)).ToList();

                if (posts != null)
                {
                    if (posts.Count() > 10)
                    {
                        ViewBag.showPostPage = "yes";
                    }
                }
                if (news != null)
                {
                    if (news.Count() > 10)
                    {
                        ViewBag.showNewsPage = "yes";
                    }
                }

                int pageSize = 10;
                int postNumberPage = (postNumber ?? 1);
                int newsNumberPage = (newsNumber ?? 1);

                var model = new searchViewModel();
                model.posts = posts.ToPagedList(postNumberPage, pageSize);
                model.news = news.ToPagedList(newsNumberPage, pageSize);

                ViewBag.postNumberPage = postNumberPage;
                ViewBag.newsNumberPage = newsNumberPage;

                return View(model);
            }

            ViewBag.error = "Погрешен влез.Ве молиме внесете коректна фраза и обидете се повторно.";

            return View();
        }
        [Authorize]
        public ActionResult addUserImage(HttpPostedFileBase ImageUser)
        {

            if (ImageUser != null && ImageUser.ContentLength > 0)
            {

                string displayName = ImageUser.FileName;
                string fileExtension = Path.GetExtension(displayName);

                if (fileExtension.ToLower() == ".jpg" || fileExtension.ToLower() == ".png" || fileExtension.ToLower() == ".jpeg")
                {
                    var userId = User.Identity.GetUserId();
                    var userImage = db.userImages.Where(x => x.UserId == userId).FirstOrDefault();

                    string fileName = string.Format("{0}.{1}", Guid.NewGuid(), fileExtension);
                    string path = Path.Combine(Server.MapPath("~/Content/images/userImages/"), fileName);
                    ImageUser.SaveAs(path);

                    userImage.image = fileName;
                    db.SaveChanges();
                }
                //else da se dodade

                return RedirectToAction("Index", "Manage");
            }

            return RedirectToAction("Index", "Manage");
        }
    }
}