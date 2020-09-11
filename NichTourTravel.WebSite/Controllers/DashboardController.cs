using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Niche.Core.IServices;
using Niche.Core.Models;
using NichTourTravel.WebSite.Helpers;
using NichTourTravel.WebSite.Models;

namespace NichTourTravel.WebSite.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IGlobalService<Article> _articleService;
        private readonly IGlobalService<Comment> _commentService;
        private readonly IGlobalService<Share> _shareService;
        private readonly IGlobalService<Tag> _tagService;
        private readonly IGlobalService<Author> _authorService;
        private readonly ILoggerService _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public DashboardController(IGlobalService<Article> articleService, IGlobalService<Comment> commentService, IGlobalService<Share> shareService, 
            IGlobalService<Author> authorService, IHttpContextAccessor httpContextAccessor, ILoggerService logger, IWebHostEnvironment webHostEnvironment,
            IGlobalService<Tag> tagService)
        {
            _articleService = articleService;
            _commentService = commentService;
            _shareService = shareService;
            _authorService = authorService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
            _tagService = tagService;
        }

        public IActionResult Index()
        {
            if (_session.GetString("account") == null)
            {
                return RedirectToAction("", "home");
            }
            return View();
        }

        public IActionResult Articles()
        {
            if (_session.GetString("account") == null)
            {
                return RedirectToAction("", "home");
            }
            return View();
        }

        public IActionResult NewArticle()
        {
            if (_session.GetString("account") == null)
            {
                return RedirectToAction("", "home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult AddArticle(NewArticleViewModel articleModel)
        {
            if (!ModelState.IsValid)
            {
                var _errors = new List<string>();
                foreach (var _values in ModelState.Values)
                {
                    foreach (var _error in _values.Errors)
                    {
                        _errors.Add(_error.ErrorMessage);
                    }
                }
                return Ok(new { check = false, message = string.Join(",  ", _errors) });
            }

            string url = "";
            if (articleModel.File != null)
            {
                bool check;
                (check, url) = ImageHelper.SaveImage(articleModel.File, _webHostEnvironment);
                if (!check)
                {
                    if (url != null) _logger.Log(Niche.Core.Enums.LogLevel.ERROR, url);

                    return Ok(new { check = false, message = "Image could not be saved, try again later" });
                }
            }

            try
            {

                var _author = _authorService.Get(int.Parse(articleModel.AuthorId));
                var _tag = _tagService.Get(int.Parse(articleModel.TagIds));
                var _article = _articleService.Add(new Article
                {
                    Id = Support.GetID(),
                    AuthorId = _author.AuthorId,
                    Body = articleModel.Content,
                    CreatedOn = DateTime.UtcNow,
                    ImageURl = url,
                    TagIds = _tag.TagId,
                    Title = articleModel.Title
                });

                return Ok(new { check = true, message = "The article was successfully saved" });
            }
            catch (Exception ex)
            {
                _logger.Log(Niche.Core.Enums.LogLevel.ERROR, ex.Message + ex.InnerException != null ? $" ,InnerException: {ex.InnerException.Message}" : "");

                return Ok(new { check = false, message = "Failed to save" });
            }
        }

    }
}