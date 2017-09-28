using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipesWEB.Models
{
    public class Notifications
    {
        public int Id { get; set; }
        public int postId { get; set; }
        public string postTitle { get; set; }
        public string forUserId { get; set; }
        public string fromUserName { get; set; }
        public string status { get; set; }
        public string notificationType { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}