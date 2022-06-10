using Newtonsoft.Json;
using RepositorioGitHub.Dominio;
using RepositorioGitHub.Dominio.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;

namespace RepositorioGitHub.Infra.ApiGitHub
{
    public class GitHubApi : IGitHubApi
    {

        private HttpClient client = new HttpClient();

        public GitHubApi()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://api.github.com/");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //_client.AddDefaultHeader("Accept", "application/vnd.github.v3+json");
            client.DefaultRequestHeaders.Add("User-Agent", "Just a test");
        }

        public ActionResult<GitHubRepository> GetRepository(string owner)
        {


            HttpResponseMessage res = client.GetAsync("users/"+owner+"/repos").Result;

            if (res.IsSuccessStatusCode)
            {
                var jsonData = res.Content.ReadAsStringAsync().Result;
                var hubs = JsonConvert.DeserializeObject<GitHubRepository[]>(jsonData);

                var act = new ActionResult<GitHubRepository>();
                act.Results = hubs;
                act.Message = "Sucessfully retrieved users repositories";

                return act;
            }
            else
            {
                throw new Exception("Cannot get retrieve users repositories.");
            }

        }
        public ActionResult<GitHubRepository> GetRepositoryById(long id)
        {
            HttpResponseMessage res = client.GetAsync("repositories/"+id).Result;

            if (res.IsSuccessStatusCode)
            {
                var jsonData = res.Content.ReadAsStringAsync().Result;
                var hub = JsonConvert.DeserializeObject<GitHubRepository>(jsonData);

                var act = new ActionResult<GitHubRepository>();
                act.Result = hub;
                act.Message = "Sucessfully retrieved repositoy "+id;

                return act;
            }
            else
            {
                throw new Exception("Cannot get retrieve "+id+" repository.");
            }
        }

        public ActionResult<RepositoryModel> GetRepositoriesByName(string name)
        {
            HttpResponseMessage res = client.GetAsync("search/repositories?q=" + name).Result;

            if (res.IsSuccessStatusCode)
            {
                Debug.WriteLine("search sucess");

                var jsonData = res.Content.ReadAsStringAsync().Result;
                var hubs = JsonConvert.DeserializeObject<RepositoryModel>(jsonData);

                var act = new ActionResult<RepositoryModel>();
                act.Result = hubs;
                act.Message = "Sucessfully retrieved some repositories";

                return act;
            }
            else
            {
                throw new Exception("Cannot get retrieve some repositories based on query " + name);
            }

        }

        public ActionResult<RepositoryModel> GetRepositoryByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
