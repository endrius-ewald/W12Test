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
            var actRes = new ActionResult<GitHubRepositoryViewModel>();
            actRes.Results = new List<GitHubRepositoryViewModel>();

            try
            {
                var apiRes = _gitHubApi.GetRepository("endrius-ewald");

                //Maping from Model to ViewModel
                foreach (var item in apiRes.Results)
                {
                    actRes.Results.Add(convertModelToViewModel(item));
                }

                actRes.IsValid = true;
                actRes.Message = apiRes.Message;

                return actRes;
 
            }catch (Exception e) {
                actRes.IsValid = false;
                actRes.Message = e.Message;

                return actRes;
            }
        }

        public ActionResult<GitHubRepositoryViewModel> GetById(long id)
        {
            var actRes = new ActionResult<GitHubRepositoryViewModel>();

            try
            {
                var apiRes = _gitHubApi.GetRepositoryById(id);

                //Maping from Model to ViewModel
                actRes.Result = convertModelToViewModel(apiRes.Result);

                actRes.IsValid = true;
                actRes.Message = apiRes.Message;

                return actRes;
            }
            catch (Exception e)
            {
                actRes.IsValid = false;
                actRes.Message = e.Message;

                return actRes;
            }

        }


        public ActionResult<GitHubRepositoryViewModel> GetRepository(string owner, long id)
        {
            return GetById(id);
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


        public ActionResult<RepositoryViewModel> GetByName(string name)
        {
            var actRes = new ActionResult<RepositoryViewModel>();

            try
            {
                var apiRes = _gitHubApi.GetRepositoriesByName(name);

                //Maping from Model to ViewModel
                var model = new RepositoryViewModel();
                model.Repositories = apiRes.Result.Repositories;
                actRes.Result = model;

                actRes.IsValid = true;
                actRes.Message = apiRes.Message;

                return actRes;

            }
            catch (Exception e)
            {
                actRes.IsValid = false;
                actRes.Message = e.Message;

                return actRes;
            }

        }

        public ActionResult<FavoriteViewModel> GetFavoriteRepository()
        {

            var actRes = new ActionResult<FavoriteViewModel>();
            actRes.Results = new List<FavoriteViewModel>();

            try
            {
                var dbRes = _context.GetAll();

                //Maping from Model to ViewModel
                foreach (var item in dbRes)
                {
                    actRes.Results.Add(convertFavorite(item));
                }

                actRes.IsValid = true;
                actRes.Message = "Sucessfull retrieved favorites";

                return actRes;

            }
            catch (Exception e)
            {
                actRes.IsValid = false;
                actRes.Message = e.Message;

                return actRes;
            }

        }

        public ActionResult<FavoriteViewModel> SaveFavoriteRepository(FavoriteViewModel view)
        {
            var actRes = new ActionResult<FavoriteViewModel>();

            Favorite aux = new Favorite();
            //MapViewModeltoModel
            {
                aux.Id = view.Id;
                aux.Description = view.Description;
                aux.Language = view.Language;
                aux.UpdateLast = view.UpdateLast;
                aux.Owner = view.Owner;
                aux.Name = view.Name;

            }

            if (_context.Insert(aux))
            {
                actRes.IsValid = true;
                actRes.Message = "Favorito salvo com sucesso.";
            }
            else
            {
                actRes.IsValid = false;
                actRes.Message = "Favorito já existente.";
            }

            return actRes;
        }

        private FavoriteViewModel convertFavorite(Favorite input)
        {
            FavoriteViewModel view = new FavoriteViewModel();
            //MapModeltoViewModel
            {
                view.Id = input.Id;
                view.Description = input.Description;
                view.Language = input.Language;
                view.UpdateLast = input.UpdateLast;
                view.Owner = input.Owner;
                view.Name = input.Name;
            }

            return view;
        }

    }
}
