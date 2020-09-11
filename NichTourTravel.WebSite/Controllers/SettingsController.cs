using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Niche.Core.ExtensionHelpers;
using Niche.Core.IServices;
using Niche.Core.Models;
using NichTourTravel.WebSite.Helpers;
using NichTourTravel.WebSite.Models;

namespace NichTourTravel.WebSite.Controllers
{
    public class SettingsController : Controller
    {
        private readonly IGlobalService<Account> _accountService;
        private readonly IGlobalService<About> _aboutService;
        private readonly ILoggerService _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        public SettingsController(IGlobalService<Account> accountService, IHttpContextAccessor httpContextAccessor, ILoggerService logger, IGlobalService<About> aboutService)
        {
            _accountService = accountService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _aboutService = aboutService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LoadAbouts()
        {
            var _abouts = _aboutService.Search(x => true).OrderByDescending(x=>x.CreatedOn).OrderByDescending(x=>x.CreatedOn);
            var _aboutList = new List<AboutViewModel>();
            foreach (var _about in _abouts)
            {
                _aboutList.Add(new AboutViewModel
                {
                    Body = _about.Body,
                    Index = _aboutList.Count + 1,
                    Id = _about.Id,
                    IsSelected = _about.IsSelected ? "active" : "-",
                    Title = _about.Title,
                    AddedOn = _about.CreatedOn.GetRelativetime()
                });
            }
            return Ok(_aboutList);
        }

        [HttpPost]
        public IActionResult SaveAbout(AboutViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                string message = "";
                var _errors = new List<string>();
                foreach (var _values in ModelState.Values)
                {
                    foreach (var _error in _values.Errors)
                    {
                        _errors.Add(_error.ErrorMessage);
                    }
                }
                message = string.Join(", ", _errors);
                return Ok(new { check = false, message });
            }

            var _abouts = _aboutService.Search(x => x.IsSelected);
            foreach (var about in _abouts)
            {
                about.IsSelected = false;
                _aboutService.Update(about);
            }


            var _about = new About
            {
                IsSelected = true,
                Id = Support.GetID(),
                Body = viewModel.Body,
                Title = viewModel.Title,
                CreatedOn = DateTime.UtcNow
            };
            try
            {
                _aboutService.Add(_about);
                return Ok(new { check = true, message = "About us information has been updated" });
            }
            catch (Exception ex)
            {
                _logger.Log(Niche.Core.Enums.LogLevel.ERROR, ex.Message + ex.InnerException != null ? $" ,InnerException: {ex.InnerException.Message}" : "");
                return Ok(new { check = false, message = "Failed, try again" });
            }
        }
        
        public IActionResult DeleteAbout(int id)
        {
            var about = _aboutService.Get(id);
            if (about.IsSelected)
            {
                return Ok(new { check = false, message = "Please, this about is active, first set another as active then delete." });
            }

            try
            {
                _logger.Log(Niche.Core.Enums.LogLevel.INFORMATION, $"Delete: {about.ToJson()} on {DateTime.UtcNow} UTC time");

                _aboutService.Delete(about);

                return Ok(new { check = true, message = "About text has been deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.Log(Niche.Core.Enums.LogLevel.ERROR, ex.Message + ex.InnerException != null ? $" ,InnerException: {ex.InnerException.Message}" : "");
                return Ok(new { check = false, message = "Failed, try again" });
            }

        }
        public IActionResult LoadAbout(int id)
        {
            var about = _aboutService.Get(id);
            return Ok(about);
        }
        public IActionResult EditAbout(AboutViewModel viewModel)
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
                string message = string.Join(", ", _errors);
                return Ok(new { check = false, message });
            }

            var about = _aboutService.Get(viewModel.Id);
            if (about == null)
            {
                return Ok(new { check = false, message = "Can not find the about information" });
            }
            about.Title = viewModel.Title;
            about.Body = viewModel.Body;

            try
            {
                _aboutService.Update(about);
                return Ok(new { check = true, message = "About information has been updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.Log(Niche.Core.Enums.LogLevel.ERROR, ex.Message + ex.InnerException != null ? $" ,InnerException: {ex.InnerException.Message}" : "");
                return Ok(new { check = false, message = "Failed, try again" });
            }
        }

        public IActionResult SetActive(int id)
        {
            var abouts = _aboutService.Search(x=>true);
            try
            {
                foreach (var _about in abouts)
                {
                    try
                    {
                        if (_about.Id == id)
                        {
                            _about.IsSelected = _about.IsSelected ? false : true;
                        }
                        else
                        {
                            _about.IsSelected = false;
                        }

                        _aboutService.Update(_about);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                return Ok(new { check = true, message = "Activation was done successfully" });
            }
            catch (Exception ex)
            {
                _logger.Log(Niche.Core.Enums.LogLevel.ERROR, ex.Message + ex.InnerException != null ? $" ,InnerException: {ex.InnerException.Message}" : "");
                return Ok(new { check = false, message = "Failed, try again" });
            }
        }
    }
}