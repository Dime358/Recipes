using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipesWEB.Models
{
    public class Reports
    {
        public int Id { get; set; }
        public int commentId { get; set; }
        public string UserId { get; set; }
        public string reportReason  { get; set; }
}
}