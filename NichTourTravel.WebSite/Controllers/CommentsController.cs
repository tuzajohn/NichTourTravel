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
    public class CommentsController : Controller
    {
        private readonly IGlobalService<Article> _articleService;
        private readonly IGlobalService<Comment> _commentService;
        private readonly ILoggerService _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        public CommentsController(IGlobalService<Article> articleService, IGlobalService<Comment> commentService, IHttpContextAccessor httpContextAccessor, ILoggerService logger)
        {
            _articleService = articleService;
            _commentService = commentService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Add(CommentViewModel comment)
        {
            if (!ModelState.IsValid)
            {
                var errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                _logger.Log(Niche.Core.Enums.LogLevel.INFORMATION, string.Join(", ", errors));
                return Json(new { check = false, message = string.Join(", ", errors) });
            }

            try
            {
                _commentService.Add(new Comment
                {
                    ArticleId = _session.GetString("aId"),
                    CreatedOn = DateTime.UtcNow,
                    Email = comment.Email,
                    Id = Support.GetID(),
                    ImageUrl = "",
                    Message = comment.Message,
                    Name = comment.Name,
                    Subject = comment.Subject,
                    IsApproved = false
                });
            }
            catch (Exception ex)
            {
                _logger.Log(Niche.Core.Enums.LogLevel.ERROR, ex.Message + ex.InnerException != null ? $" ,InnerException: {ex.InnerException.Message}" : "");

                return Json(new
                {
                    check = false,
                    message = "Failed to save your comment, kindly check your connection and try again."
                });
            }

            return Ok(new { check = true, message = "You comment has been saved" });
        }

        public IActionResult LoadAll(string articleId)
        {
            var _comments = _commentService.Search(x => x.ArticleId == articleId).OrderByDescending(x => x.CreatedOn);

            var commentViewList = new List<CommentViewModel>();
            var index = 0;
            foreach (var _comment in _comments)
            {
                index++;
                commentViewList.Add(new CommentViewModel
                {
                    Email = _comment.Email,
                    Message = _comment.Message,
                    Name = _comment.Name,
                    Subject = _comment.Subject,
                    DateDuration = _comment.CreatedOn.GetRelativetime(),
                    Index = index,
                    IsApproved = _comment.IsApproved,
                    Id = _comment.Id
                });
            }
            

            return Ok(commentViewList);
        }

        public IActionResult Approve(int id)
        {
            var _comment = _commentService.Get(id);
            if (_comment != null)
            {
                _comment.IsApproved = true;
                _commentService.Update(_comment);
                return Ok(new { check = true, message = "Comment has been approved" });
            }
            return NotFound(new { message = "Comment not found" });
        }

        public IActionResult Delete(int id)
        {
            var _comment = _commentService.Get(id);

            try
            {
                _logger.Log(Niche.Core.Enums.LogLevel.INFORMATION, $"Delete: {_comment.ToJson()} on {DateTime.UtcNow} UTC time");

                _commentService.Delete(_comment);

                return Ok(new { check = true, message = "Comment has been deleted successfully" });
            }
            catch (Exception ex)
            {

                _logger.Log(Niche.Core.Enums.LogLevel.ERROR, ex.Message + ex.InnerException != null ? $" ,InnerException: {ex.InnerException.Message}" : "");
                return Ok(new { check = false, message = "Failed, try again" });
            }

        }
    }
}