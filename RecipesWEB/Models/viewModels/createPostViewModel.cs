using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecipesWEB.Models.viewModels
{
    public class createPostViewModel
    {

        public int Id { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "невалидна големина на наслов", MinimumLength = 6)]
        public string postTitle { get; set; }
        [Required]
        public string postBody { get; set; }
        [Required]
        public string postCategory { get; set; }

        public DateTime CreatedDate { get; set; }
        public int postRating { get; set; }
        public string postTags { get; set; }
        public string approveState { get; set; }

        [Required]
        [Range(1, 1000)]
        public int prepTime { get; set; }
        [Required]
        public string difficulty { get; set; }
        [Required]
        [Range(1, 20)]
        public int numPortions { get; set; }
        [Required]
        public string ingredients { get; set; }
        public bool approved { get; set; }
        public string UserId { get; set; }
        //moze ke treba required
        public string UserName { get; set; }

        public virtual ICollection<Comments> Comments { get; set; }
        public HttpPostedFileBase ImagePost { get; set; }

    }
}