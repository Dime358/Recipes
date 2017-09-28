using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipesWEB.Models
{
    public class Favorites
    {
        public int Id { get; set; }
        public int postId { get; set; }
        public string userId { get; set; }
    }
}