using RepositorioGitHub.Business.Contract;
using RepositorioGitHub.Dominio;
using RepositorioGitHub.Dominio.Interfaces;
using RepositorioGitHub.Infra.Contract;
using System;
using System.Linq;
using System.Collections.Generic;

namespace RepositorioGitHub.Business
{
    public class GitHubApiBusiness: IGitHubApiBusiness
    {
        private readonly IContextRepository _context;
        private readonly IGitHubApi _gitHubApi;
        public GitHubApiBusiness(IContextRepository context, IGitHubApi gitHubApi)
        {
            _context = context;
            _gitHubApi = gitHubApi;
        }

        public ActionResult<GitHubRepositoryViewModel> Get()
        {
            Console.WriteLine("Call get no index");

            var actRes = new ActionResult<GitHubRepositoryViewModel>();
            actRes.Results = new List<GitHubRepositoryViewModel>();

            try
            {
                var apiRes = _gitHubApi.GetRepository("endrius-ewald");

                //Maping from Model to ViewModel
                actRes.IsValid = true;
                actRes.Message = apiRes.Message;

                foreach (var item in apiRes.Results)
                {
                    actRes.Results.Add(convertModelToViewModel(item));
                }

                return actRes;
 
            }catch (Exception e) {
                actRes.IsValid = false;
                actRes.Message = e.Message;
            }


            return new ActionResult<GitHubRepositoryViewModel>();
        }

        private GitHubRepositoryViewModel convertModelToViewModel(GitHubRepository input)
        {
            var ret = new GitHubRepositoryViewModel();

            ret.Id = input.Id;
            ret.Name = input.Name;
            ret.FullName = input.FullName;
            ret.Owner = input.Owner;
            ret.Url = input.Url;
            ret.Description = input.Description;
            ret.Language = input.Language;
            ret.UpdatedAt = input.UpdatedAt;

            return ret;
        }

        public ActionResult<GitHubRepositoryViewModel> GetById(long id)
        {
            return new ActionResult<GitHubRepositoryViewModel>();
        }

        public ActionResult<RepositoryViewModel> GetByName(string name)
        {
            return new ActionResult<RepositoryViewModel>();
        }

        public ActionResult<FavoriteViewModel> GetFavoriteRepository()
        {
            return new ActionResult<FavoriteViewModel>();
        }

        public ActionResult<GitHubRepositoryViewModel> GetRepository(string owner, long id)
        {
            return new ActionResult<GitHubRepositoryViewModel>();
        }

        public ActionResult<FavoriteViewModel> SaveFavoriteRepository(FavoriteViewModel view)
        {
            return new ActionResult<FavoriteViewModel>();
        }
    }
}
