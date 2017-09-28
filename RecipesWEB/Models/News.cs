using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecipesWEB.Models
{
    public class News
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Внесете наслов на веста")]
        public string newsTitle { get; set; }
        [Required(ErrorMessage = "Внесете текст на веста")]
        public string newsBody { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string imagePath { get; set; }
    }
}