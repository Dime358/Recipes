using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipesWEB.Models.viewModels
{
    public class homePageVewModel
    {

        public List<Posts> recentPostsAll { get; set; }
        public List<Posts> recentPostsPredjadenja { get; set; }
        public List<Posts> recentPostsGlavni { get; set; }
        public List<Posts> recentPostsDeserti { get; set; }
        public List<Posts> recentPostsDodatoci { get; set; }

        public List<Posts> topRatingPostsAll { get; set; }
        public List<Posts> topRatingPostsPredjadenja { get; set; }
        public List<Posts> topRatingPostsGlavni { get; set; }
        public List<Posts> topRatingPostsDeserti { get; set; }
        public List<Posts> topRatingPostsDodatoci { get; set; }

        public List<Tuple<ApplicationUser, int, string>> topRatingUsers { get; set; }
        public List<News> latestNews { get; set; }

        public homePageVewModel()
        {
            recentPostsAll = new List<Posts>();
            recentPostsPredjadenja = new List<Posts>();
            recentPostsGlavni = new List<Posts>();
            recentPostsDeserti = new List<Posts>();
            recentPostsDodatoci = new List<Posts>();

            topRatingPostsAll = new List<Posts>();
            topRatingPostsPredjadenja = new List<Posts>();
            topRatingPostsGlavni = new List<Posts>();
            topRatingPostsDeserti = new List<Posts>();
            topRatingPostsDodatoci = new List<Posts>();

            topRatingUsers = new List<Tuple<ApplicationUser, int, string>>();
            latestNews = new List<News>();
        }

    }
}