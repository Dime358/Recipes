using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipesWEB.Models.viewModels
{
    public class notificationsViewModel
    {
        public List<Notifications> notifications { get; set; }

        public int notificationNumber { get; set; }
        public bool viewMoreStatus { get; set; }

        public int adminNum { get; set; }

        public notificationsViewModel()
        {
            notifications = new List<Notifications>();
        }

    }
}