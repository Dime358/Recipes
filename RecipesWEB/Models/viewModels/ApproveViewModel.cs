using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipesWEB.Models.viewModels
{
    public class approveViewModel
    {        
        public PagedList.IPagedList<Posts> Posts { get; set; }
        public PagedList.IPagedList<Tuple<Comments, string , int>> reportedComments { get; set; }
    }
}