using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipesWEB.Models.viewModels
{
    public class postStatus
    {
        public List<Posts> unApprovedPosts { get; set; }

        public postStatus()
        {
            unApprovedPosts = new List<Posts>();

        }
    }
}