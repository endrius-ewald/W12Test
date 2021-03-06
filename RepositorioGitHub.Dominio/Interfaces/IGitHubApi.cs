using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositorioGitHub.Dominio.Interfaces
{
    public interface IGitHubApi
    {
        ActionResult<GitHubRepository> GetRepository(string owner);
        ActionResult<GitHubRepository> GetRepositoryById(long id);     
        ActionResult<RepositoryModel> GetRepositoryByName(string name);
        ActionResult<RepositoryModel> GetRepositoriesByName(string name);

    }
}
