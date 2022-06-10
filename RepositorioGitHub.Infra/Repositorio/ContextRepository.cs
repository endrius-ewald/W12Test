using RepositorioGitHub.Dominio;
using RepositorioGitHub.Infra.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositorioGitHub.Infra.Repositorio
{
    public class ContextRepository : IContextRepository
    {
        static ISet<Favorite> db = new HashSet<Favorite>();

        public bool ExistsByCheckAlready(Favorite favorite)
        {
            return db.Contains(favorite);
        }

        public List<Favorite> GetAll()
        {
            return db.ToList();

            return new List<Favorite>();
        }

        public bool Insert(Favorite favorite)
        {
            return db.Add(favorite);
        }
    }
}
