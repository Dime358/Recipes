using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecipesWEB.Models.viewModels
{
    public class userProfileViewModel
    {
        public int Id { get; set; }
        public int rating { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public PagedList.IPagedList<Posts> userPostsAll { get; set; }
        public PagedList.IPagedList<Posts> favoritePostsAll { get; set; }
        public string image { get; set; }
        public int numPosts { get; set; }
    }
}