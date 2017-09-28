using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RecipesWEB.Models
{
    public class RecipesWEBContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public RecipesWEBContext() : base("name=RecipesWEBContext")
        {
        }

        

        public System.Data.Entity.DbSet<RecipesWEB.Models.Posts> Posts { get; set; }

        public System.Data.Entity.DbSet<RecipesWEB.Models.Comments> Comments { get; set; }

        public System.Data.Entity.DbSet<RecipesWEB.Models.Ratings> Ratings { get; set; }

        public System.Data.Entity.DbSet<RecipesWEB.Models.UserRatings> UserRatings { get; set; }

        public System.Data.Entity.DbSet<RecipesWEB.Models.News> News { get; set; }

        public System.Data.Entity.DbSet<RecipesWEB.Models.Reports> Reports { get; set; }

        public object Users { get; internal set; }

        public System.Data.Entity.DbSet<RecipesWEB.Models.userImages> userImages { get; set; }
        
        public System.Data.Entity.DbSet<RecipesWEB.Models.Favorites> Favorites { get; set; }

        public System.Data.Entity.DbSet<RecipesWEB.Models.Notifications> Notifications { get; set; }
    }
}
