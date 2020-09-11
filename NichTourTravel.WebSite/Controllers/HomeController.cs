using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Niche.Core.IServices;
using Niche.Core.Models;
using NichTourTravel.WebSite.Helpers;
using NichTourTravel.WebSite.Models;

namespace NichTourTravel.WebSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGlobalService<Article> _articleService;
        private readonly IGlobalService<Comment> _commentService;
        private readonly IGlobalService<Share> _shareService;
        private readonly IGlobalService<Author> _authorService;
        private readonly IGlobalService<About> _aboutService;
        public HomeController(ILogger<HomeController> logger, IGlobalService<Article> articleService, IGlobalService<Comment> commentService, IGlobalService<Share> shareService,
            IGlobalService<Author> authorService, IGlobalService<About> aboutService)
        {
            _logger = logger;
            _articleService = articleService;
            _commentService = commentService;
            _shareService = shareService;
            _authorService = authorService;
            _aboutService = aboutService;
        }

        public IActionResult Index()
        {
            var _articles = _articleService.Search(x => x.Id != 0).OrderByDescending(x => x.CreatedOn).ToList();

            var _articleViewModels = new List<ArticleViewModel>();
            foreach (var _article in _articles)
            {
                var shares = _shareService.Search(x => x.ArticleId == _article.Id.ToString());
                var _author = _authorService.Search(x=>x.AuthorId == _article.AuthorId).FirstOrDefault();
                
                _articleViewModels.Add(new ArticleViewModel
                {
                    Author = _author != null? _author.FirstName + " " + _author.LastName: "Niche",
                    Body = _article.Body,
                    Id = _article.Id,
                    ImageURl = _article.ImageURl,
                    Tags = _article.TagIds,
                    Title = _article.Title,
                    Duration = _article.CreatedOn.GetRelativetime(),
                    ShareCount = shares.Count()
                });
            }
            

            return View(_articleViewModels);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult ContactUs(ContactUsViewModel contactUs)
        {
            if (!ModelState.IsValid)
            {
                var errors = new List<string>();
                foreach (var values in ModelState.Values)
                {
                    foreach (var error in values.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }

                return Ok(new { check = false, message = string.Join("?, ", errors) });
            }



            return Ok(new { check = true, message = "We have well received your message and shall get back to you soon" });
        }

        public IActionResult About()
        {
            var _about = _aboutService.Search(x => x.IsSelected).FirstOrDefault();
            return View(_about);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
