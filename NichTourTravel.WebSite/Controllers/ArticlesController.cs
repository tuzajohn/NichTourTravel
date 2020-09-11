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
    public class ArticlesController : Controller
    {
        private readonly IGlobalService<Article> _articleService;
        private readonly IGlobalService<Comment> _commentService;
        private readonly IGlobalService<Share> _shareService;
        private readonly IGlobalService<Author> _authorService;
        private readonly IGlobalService<Tag> _tagService;
        private readonly ILoggerService _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        public ArticlesController(IGlobalService<Article> articleService, IGlobalService<Comment> commentService, IGlobalService<Share> shareService, IGlobalService<Author> authorService, IHttpContextAccessor httpContextAccessor, ILoggerService logger, IGlobalService<Tag> tagService)
        {
            _articleService = articleService;
            _commentService = commentService;
            _shareService = shareService;
            _authorService = authorService;
            _logger = logger;
            _tagService = tagService;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public IActionResult ViewArticle(int id)
        {
            var _article = _articleService.Get(id);

            _session.SetString("aId", _article.ArticleId);

            var _comments = _commentService.Search(x => x.ArticleId == _article.ArticleId).OrderBy(x=>x.CreatedOn).ToList();
            var shares = _shareService.Search(x => x.ArticleId == _article.Id.ToString());
            var _author = _authorService.Search(x=>x.AuthorId == _article.AuthorId).FirstOrDefault();

            var _articleViewModel = new ArticleViewModel
            {
                Author = _author != null ? _author.FirstName + " " + _author.LastName : "Niche",
                Body = _article.Body,
                Id = _article.Id,
                ImageURl = _article.ImageURl,
                Tags = _article.TagIds,
                Title = _article.Title,
                Duration = _article.CreatedOn.GetRelativetime(),
                ShareCount = shares.Count()
            };

            var _commentList = new List<CommentViewModel>();
            foreach (var _comment in _comments)
            {
                if (_comment.IsApproved)
                {
                    _commentList.Add(new CommentViewModel
                    {
                        Email = _comment.Email,
                        Message = _comment.Message,
                        Name = _comment.Name,
                        Subject = _comment.Subject,
                        DateDuration = _comment.CreatedOn.GetRelativetime(),
                        IsApproved = _comment.IsApproved,
                        Index = _commentList.Count + 1
                    });
                }
            }
            _articleViewModel.Comments = _commentList;  

            return View(_articleViewModel);
        }

        public IActionResult LoadLatest()
        {
            var _articles = _articleService.Search(x => true).Take(4).OrderByDescending(x => x.CreatedOn).ToList();

            var _articleList = new List<ArticleViewModel>();

            foreach (var _article in _articles)
            {
                var _author = _authorService.Search(x=>x.AuthorId == _article.AuthorId).FirstOrDefault();
                var shares = _shareService.Search(x => x.ArticleId == _article.Id.ToString());
                var _tag = _tagService.Search(x => x.TagId == _article.TagIds).FirstOrDefault();
                _articleList.Add(new ArticleViewModel
                {
                    Author = _author != null ? _author.FirstName + " " + _author.LastName : "Niche",
                    Body = _article.Body,
                    Id = _article.Id,
                    ImageURl = _article.ImageURl,
                    Tags = _tag.Name,
                    Title = _article.Title,
                    Duration = _article.CreatedOn.GetRelativetime(),
                    ShareCount = shares.Count()
                });
            }

            return Ok(_articleList);
        }

        public IActionResult Load(int id)
        {
            var _article = _articleService.Get(id);

            var _author = _authorService.Search(x => x.AuthorId == _article.AuthorId).FirstOrDefault();
            var _comments = _commentService.Search(x => x.ArticleId == _article.ArticleId);
            var shares = _shareService.Search(x => x.ArticleId == _article.Id.ToString());

            var article = new ArticleViewModel
            {
                AddedOn = _article.CreatedOn.GetRelativetime(),
                Title = _article.Title,
                Id = _article.Id,
                Index = 0,
                Author = _author != null ? $"{_author.FirstName} {_author.LastName}" : "Niche",
                Body = _article.Body,
                ImageURl = _article.ImageURl,
                Tags = _article.TagIds,
                Duration = _article.CreatedOn.GetRelativetime(),
                ShareCount = shares.Count(),
                ArticleId = _article.ArticleId
            };
            var _commentList = new List<CommentViewModel>();
            foreach (var _comment in _comments)
            {
                _commentList.Add(new CommentViewModel
                {
                    Email = _comment.Email,
                    Message = _comment.Message,
                    Name = _comment.Name,
                    Subject = _comment.Subject,
                    DateDuration = _comment.CreatedOn.GetRelativetime()
                });
            }
            article.Comments = _commentList;

            return Ok(article);
        }

        public IActionResult LoadAll()
        {
            var _articles = _articleService.Search(x => true).OrderByDescending(x => x.CreatedOn).ToList();

            var _articleList = new List<ArticleViewModel>();

            foreach (var _article in _articles)
            {
                var _author = _authorService.Search(x => x.AuthorId == _article.AuthorId).FirstOrDefault();
                var shares = _shareService.Search(x => x.ArticleId == _article.ArticleId);
                var _comments = _commentService.Search(x => x.ArticleId == _article.ArticleId);
                var _articleView = new ArticleViewModel
                {
                    Author = _author != null ? _author.FirstName + " " + _author.LastName : "Niche",
                    Body = _article.Body,
                    Id = _article.Id,
                    ImageURl = _article.ImageURl,
                    Tags = _article.TagIds,
                    Title = _article.Title,
                    Duration = _article.CreatedOn.GetRelativetime(),
                    ShareCount = shares.Count(),
                    Index = _articleList.Count + 1,
                    AddedOn = _article.CreatedOn.GetRelativetime(),
                    ArticleId = _article.ArticleId
                };
                var _commentList = new List<CommentViewModel>();
                foreach (var _comment in _comments)
                {
                    if (_comment.IsApproved)
                    {
                        _commentList.Add(new CommentViewModel
                        {
                            Email = _comment.Email,
                            Message = _comment.Message,
                            Name = _comment.Name,
                            Subject = _comment.Subject,
                            DateDuration = _comment.CreatedOn.GetRelativetime(),
                            IsApproved = _comment.IsApproved,
                            Index = _commentList.Count + 1
                        });
                    }
                }
                _articleView.Comments = _commentList;
                _articleList.Add(_articleView);
            }

            return Ok(_articleList);
        }

        public IActionResult Delete(int id)
        {
            var _article = _articleService.Get(id);

            try
            {

                _logger.Log(Niche.Core.Enums.LogLevel.INFORMATION, $"Delete: {_article.ToJson()} on {DateTime.UtcNow} UTC time");

                _articleService.Delete(_article);

                return Ok(new { check = true, message = "Article has been deleted successfully" });
            }
            catch (Exception ex)
            {

                _logger.Log(Niche.Core.Enums.LogLevel.ERROR, ex.Message + ex.InnerException != null ? $" ,InnerException: {ex.InnerException.Message}" : "");
                return Ok(new { check = false, message = "Failed, try again" });
            }

        }

        public IActionResult Test()
        {
            return Ok(new { message = "john" });
        }
    }
}