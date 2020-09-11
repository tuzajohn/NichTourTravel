using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Niche.Core.ExtensionHelpers;
using Niche.Core.IServices;
using Niche.Core.Models;
using NichTourTravel.WebSite.Helpers;
using NichTourTravel.WebSite.Models;

namespace NichTourTravel.WebSite.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IGlobalService<Article> _articleService;
        private readonly IGlobalService<Author> _authorService;
        private readonly ILoggerService _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        public AuthorController(IGlobalService<Article> articleService, IGlobalService<Author> authorService, IHttpContextAccessor httpContextAccessor, ILoggerService logger)
        {
            _articleService = articleService;
            _authorService = authorService;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Load(int id)
        {
            var _author = _authorService.Search(x => x.Id == id).FirstOrDefault();

            var author = new AuthorViewModel
            {
                AddedOn = _author.CreatedOn.GetRelativetime(),
                FirstName = _author.FirstName,
                LastName = _author.LastName,
                Id = _author.Id,
                index = 0
            };

            return Ok(author);
        }
        public IActionResult LoadAll()
        {
            var authors = new List<AuthorViewModel>();
            var index = 0;
            var _authors = _authorService.Search(x => true).OrderByDescending(x => x.CreatedOn);
            foreach (var _author in _authors)
            {
                index++;
                authors.Add(new AuthorViewModel
                {
                    AddedOn = _author.CreatedOn.GetRelativetime(),
                    FirstName = _author.FirstName,
                    LastName = _author.LastName,
                    Id = _author.Id,
                    index = index
                });
            }

            return Ok(authors);
        }

        public IActionResult AddNew(string Firstname, string Lastname)
        {
            try
            {
                var _author = new Author
                {       
                    CreatedOn = DateTime.UtcNow,
                    FirstName = Firstname,
                    LastName = Lastname,
                    Id = Support.GetID()
                };

                _authorService.Add(_author);

                return Ok(new { check = true, message = "Author has been addedd successfully" });
            }
            catch (Exception ex)
            {
                _logger.Log(Niche.Core.Enums.LogLevel.ERROR, ex.Message + ex.InnerException != null ? $" ,InnerException: {ex.InnerException.Message}" : "");
                return Ok(new { check = false, message = "Failed, try again" });
            }
        }


        public IActionResult Edit(AuthorViewModel authorView)
        {
            var _author = _authorService.Search(x => x.Id == authorView.Id).FirstOrDefault();

            _author.FirstName = authorView.FirstName;
            _author.LastName = authorView.LastName;

            try
            {
                _authorService.Update(_author);
                return Ok(new { check = true, message = "Author has been updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.Log(Niche.Core.Enums.LogLevel.ERROR, ex.Message + ex.InnerException != null ? $" ,InnerException: {ex.InnerException.Message}" : "");
                return Ok(new { check = false, message = "Failed, try again" });
            }
        }

        public IActionResult Delete(int id)
        {
            var _author = _authorService.Get(id);

            var _articles = _articleService.Search(x => x.AuthorId == _author.AuthorId);
            if (_articles.Count() > 0)
            {
                foreach (var _article in _articles)
                {
                    _article.AuthorId = default;
                    _articleService.Update(_article);
                }
            }

            try
            {

                _logger.Log(Niche.Core.Enums.LogLevel.INFORMATION, $"Delete: {_author.ToJson()} on {DateTime.UtcNow} UTC time");

                _authorService.Delete(_author);

                return Ok(new { check = true, message = "Author has been deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.Log(Niche.Core.Enums.LogLevel.ERROR, ex.Message + ex.InnerException != null ? $" ,InnerException: {ex.InnerException.Message}" : "");
                return Ok(new { check = false, message = "Failed, try again" });
            }

        }
    }
}