using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecipesWEB.Models
{
    public class Ratings
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public bool reacted { get; set; }
        [Required]
        public int PostsID { get; set; }
        //public virtual Posts Posts { get; set; }
        
    }
}