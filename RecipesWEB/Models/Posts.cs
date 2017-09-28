using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RecipesWEB.Models
{
    public class Posts
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Внесете наслов на рецептот")]
        [StringLength(60, ErrorMessage = "невалидна големина на наслов", MinimumLength = 6)]
        public string postTitle { get; set; }
        [Required(ErrorMessage = "Внесете опис на рецептот")]
        public string postBody { get; set; }
        [Required(ErrorMessage = "Внесете категорија")]
        public string postCategory { get; set; }
        public DateTime CreatedDate { get; set; }
        public int postRating { get; set; }
        public string postTags { get; set; }
        [Required(ErrorMessage = "Внесете состојки за рецептот")]
        public string ingredients { get; set; }
        public bool approved { get; set; }
        public string approveState { get; set; }
        [Required(ErrorMessage = "Внесете време на подготовка")]
        [Range(1,1000,ErrorMessage = "невалидно време")]
        public int prepTime { get; set; }
        [Required(ErrorMessage = "Внесете тежина за подготовка")]
        public string difficulty { get; set; }
        [Required(ErrorMessage = "Внесете број на порции")]
        [Range(1, 20, ErrorMessage = "невалидна вредност")]
        public int numPortions { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
        public string imagePath { get; set; }
    }
}