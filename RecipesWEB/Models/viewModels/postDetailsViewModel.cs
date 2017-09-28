using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecipesWEB.Models.viewModels
{
    public class postDetailsViewModel
    {

        public int postId { get; set; }
       // public bool reacted { get; set; }
        [Required]
        public string UserId { get; set; }
        public string LoggedUserName { get; set; }
        public Posts post { get; set; }
        public List<Tuple<Comments, string , string>> comments { get; set; }
        public List<Posts> postsByUser { get; set; }
        public List<Posts> postsByCategory { get; set; }

        public postDetailsViewModel()
        {
            comments = new List<Tuple<Comments, string , string>>();
            postsByUser = new List<Posts>();
            postsByCategory = new List<Posts>();
        }

    }
}