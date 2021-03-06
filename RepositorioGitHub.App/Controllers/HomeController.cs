using RepositorioGitHub.Business;
using RepositorioGitHub.Business.Contract;
using RepositorioGitHub.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RepositorioGitHub.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGitHubApiBusiness _business;
        private static bool _isFavorite {get; set;}

        public HomeController(IGitHubApiBusiness business)
        {
            _business = business;
        }
        public ActionResult Index()
        {
            
            var model = _business.Get();

            
            if (model.IsValid)
            {
                TempData["success"] = model.Message;
            }
            else
            {
                TempData["warning"] = model.Message;
            }
            
            return View(model);
        }

        public ActionResult Details(int id)
        {
            
            var model = _business.GetById(id);
            model.Result.isFavorite = _isFavorite;

           
            if (model.IsValid)
            {
                TempData["success"] = model.Message;
            }
            else
            {
                TempData["warning"] = model.Message;
            }
            
            return View(model);
        }


        public ActionResult DetailsRepository(long id, string login)
        {
            ActionResult<GitHubRepositoryViewModel> model = new ActionResult<GitHubRepositoryViewModel>();

            if (string.IsNullOrEmpty(login) && id == 0)
            {
                return RedirectToAction("GetRepositorie");
            }
            else
            {

                model = _business.GetRepository(login, id);
                //model.Result.isFavorite = _business.existFavorite(id); //NotImplemented

                if (model.IsValid)
                {
                    TempData["success"] = model.Message;
                }
                else
                {
                    TempData["warning"] = model.Message;
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult GetRepositorie()
        {
            ActionResult<RepositoryViewModel> model = new ActionResult<RepositoryViewModel>();

            return View(model);
        }

        [HttpPost]
        public ActionResult GetRepositorie(ActionResult<RepositoryViewModel> view)
        {
            ActionResult<RepositoryViewModel> model = new ActionResult<RepositoryViewModel>();
            if (string.IsNullOrEmpty(view.Result?.Name))
            {
                model.IsValid = false;
                model.Message = "O Campo Nome Repositório tem que ser Presenchido";
                TempData["warning"] = model.Message;
                return View(model);
            }

            model = _business.GetByName(view.Result.Name);

            if (model.IsValid)
            {
                TempData["success"] = model.Message;
            }
            else
            {
                TempData["warning"] = model.Message;
            }

            return View(model);
        }


        public ActionResult Favorite()
        {

            ActionResult<FavoriteViewModel> model = new ActionResult<FavoriteViewModel>();

            var response = _business.GetFavoriteRepository();
           
            model = response;

            if (model.IsValid)
            {
                TempData["success"] = model.Message;
            }
            else
            {
                TempData["warning"] = model.Message;
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult FavoriteSave(long id, string owner, string name, string language, string lastUpdat, string description)
        {     
            ActionResult< GitHubRepositoryViewModel> model = new ActionResult<GitHubRepositoryViewModel>();


            if (string.IsNullOrEmpty(owner) && string.IsNullOrEmpty(name) && string.IsNullOrEmpty(language)
                && string.IsNullOrEmpty(lastUpdat)&& string.IsNullOrEmpty(description))
            {
                model.IsValid = false;
                model.Message = "Não foi possivel realizar esta operação";
              
                return Json(new
                {
                    Data = model
                }, JsonRequestBehavior.AllowGet);

            }
            else
            {

                FavoriteViewModel view = new FavoriteViewModel()
                {
                    Id = id,
                    Description = description,
                    Language = language,
                    Owner = owner,
                    UpdateLast =  DateTime.Parse(lastUpdat),
                    Name = name
                    
                };

              var response = _business.SaveFavoriteRepository(view);

                if (response.IsValid)
                {
                    TempData["success"] = response.Message;
                }
                else
                {
                    TempData["warning"] = response.Message;
                }

                return Json(new
                {
                    Data = response
                }, JsonRequestBehavior.AllowGet);
            }
            
        }
    }
}