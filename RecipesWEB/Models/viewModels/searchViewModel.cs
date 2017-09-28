using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipesWEB.Models.viewModels
{
    public class searchViewModel
    {
        public PagedList.IPagedList<Posts> posts { get; set; }
        //public List<ApplicationUser> users { get; set; }
        public PagedList.IPagedList<News> news { get; set; }

    }
}