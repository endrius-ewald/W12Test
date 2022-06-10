using RepositorioGitHub.Dominio;
using RepositorioGitHub.Infra.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Diagnostics;


namespace RepositorioGitHub.Infra.Repositorio
{
    public class ContextRepository : DbContext, IContextRepository
    {
        //static ISet<Favorite> db = new HashSet<Favorite>();

        public DbSet<Favorite> fav { get; set;}

        public bool ExistsByCheckAlready(Favorite favorite)
        {
            return false;
        }

        public List<Favorite> GetAll()
        {
            return fav.ToList();
        }

        public bool Insert(Favorite favorite)
        {
            fav.Add(favorite);
            int r = this.SaveChanges();
            return r > 0 ? true : false;
        }
        
    }
}
