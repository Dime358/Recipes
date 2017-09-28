using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecipesWEB.Models
{
    public class Comments
    {
        public int Id { get; set; }
        public string CommentBody { get; set; }
        public DateTime CreatedDate { get; set; }
        [Required]
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int PostsID { get; set; }
    }
}