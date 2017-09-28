using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecipesWEB.Models
{
    public class UserRatings
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public int rating { get; set; }
    }
}