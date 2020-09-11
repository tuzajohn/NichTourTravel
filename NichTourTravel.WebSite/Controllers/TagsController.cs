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
    public class TagsController : Controller
    {
        private readonly IGlobalService<Article> _articleService;
        private readonly IGlobalService<Tag> _tagService;
        private readonly ILoggerService _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        public TagsController(IGlobalService<Article> articleService, IGlobalService<Tag> tagService, IHttpContextAccessor httpContextAccessor, ILoggerService logger)
        {
            _articleService = articleService;
            _tagService = tagService;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Load(int id)
        {
            var _tag = _tagService.Get(id);

            var tag = new TagViewModel
            {
                AddedOn = _tag.CreatedOn.GetRelativetime(),
                Name = _tag.Name,
                Id = _tag.Id,
                Index = 0
            };

            return Ok(_tag);
        }

        public IActionResult LoadAll()
        {
            var _tagList = new List<TagViewModel>();
            var index = 0;
            var _tags = _tagService.Search(x => true).OrderByDescending(x => x.CreatedOn);
            foreach (var _tag in _tags)
            {
                index++;
                _tagList.Add(new TagViewModel
                {
                    AddedOn = _tag.CreatedOn.GetRelativetime(),
                    Name = _tag.Name,
                    Id = _tag.Id,
                    Index = index
                });
            }

            return Ok(_tagList);
        }

        public IActionResult AddNew(string name)
        {
            try
            {
                var _tag = new Tag
                {
                    CreatedOn = DateTime.UtcNow,
                    Name = name,
                    Id = Support.GetID()
                };

                _tagService.Add(_tag);

                return Ok(new { check = true, message = "New tag has been addedd successfully" });
            }
            catch (Exception ex)
            {
                _logger.Log(Niche.Core.Enums.LogLevel.ERROR, ex.Message + ex.InnerException != null ? $" ,InnerException: {ex.InnerException.Message}" : "");
                return Ok(new { check = false, message = "Failed, try again" });
            }
        }

        public IActionResult Edit(int id, string name)
        {
            var _tag = _tagService.Search(x => x.Id == id).FirstOrDefault();

            _tag.Name = name;

            try
            {
                _tagService.Update(_tag);
                return Ok(new { check = true, message = "Tag has been updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.Log(Niche.Core.Enums.LogLevel.ERROR, ex.Message + ex.InnerException != null ? $" ,InnerException: {ex.InnerException.Message}" : "");
                return Ok(new { check = false, message = "Failed, try again" });
            }
        }

        public IActionResult Delete(int id)
        {
            var _tag = _tagService.Get(id);

            var _articles = _articleService.Search(x => x.TagIds == _tag.TagId);
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

                _logger.Log(Niche.Core.Enums.LogLevel.INFORMATION, $"Delete: {_tag.ToJson()} on {DateTime.UtcNow} UTC time");

                _tagService.Delete(_tag);

                return Ok(new { check = true, message = "Tag has been deleted successfully" });
            }
            catch (Exception ex)
            {

                _logger.Log(Niche.Core.Enums.LogLevel.ERROR, ex.Message + ex.InnerException != null ? $" ,InnerException: {ex.InnerException.Message}" : "");
                return Ok(new { check = false, message = "Failed, try again" });
            }

        }
    }
}