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
    public class AccountController : Controller
    {
        private readonly IGlobalService<Account> _accountService;
        private readonly ILoggerService _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        public AccountController(IGlobalService<Account> accountService, ILoggerService logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _accountService = accountService;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Logout()
        {
            _session.Remove("account");
            return RedirectToAction("index", "home");
        }
        public IActionResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return Ok(new { check = false, message = "Provide both username and password" });
            }

            var _accounts = _accountService.Search(x => x.Username == username && x.Password == password);
            if (_accounts.Count() == 0)
            {
                return Ok(new { check = false, message = "Wrong username or password" });
            }

            var _account = _accounts.FirstOrDefault();
            _session.SetString("account", (new AccountViewModel
            {
                Name = _account.Name,
                Username = _account.Username
            }).ToJson());

            return Ok(new { check = true, message = $"Hello {_account.Name}, we are redirecting you..." });
        }

        [HttpPost]
        public IActionResult CreateAccount(AccountViewModel viewModel)
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

            if (viewModel.Password != viewModel.ConfirmPassword)
            {
                return Ok(new { check = false, message = "Passwords not matching" });
            }
            var _accounts = _accountService.Search(x => x.Username.Contains(viewModel.Username));
            if (_accounts != null && _accounts.Count() > 0)
            { 
                return Ok(new { check = false, message = "User exist already" });
            }

            var _account = new Account
            {
                CreatedOn = DateTime.UtcNow,
                Id = Support.GetID(),
                Name = viewModel.Name,
                Password = viewModel.Password,
                Username = viewModel.Username
            };

            try
            {
                _accountService.Add(_account);

                return Ok(new { check = true, message = "User has been added" });
            }
            catch (Exception ex)
            {
                _logger.Log(Niche.Core.Enums.LogLevel.ERROR, ex.Message + ex.InnerException != null ? $" ,InnerException: {ex.InnerException.Message}" : "");

                return Ok(new { check = false, message = "Failed to save" });
            }

        }

        public IActionResult ChangeName(string name, int id)
        {
            var _account = _accountService.Get(id);
            if (_account == null)
            {
                return Ok(new { check = false, message = "Account does not exist" });
            }

            if (string.IsNullOrEmpty(name))
            {
                return Ok(new { check = false, message = "The account name can not be empty" });
            }

            _account.Name = name;
            try
            {
                _accountService.Update(_account);
                _logger.Log(Niche.Core.Enums.LogLevel.INFORMATION, $"Account-name: {_account.ToJson()} was updated on {DateTime.UtcNow} UTC");
                return Ok(new { check = true, message = "Name was changed successfully" });
            }
            catch (Exception ex)
            {
                _logger.Log(Niche.Core.Enums.LogLevel.ERROR, ex.Message + ex.InnerException != null ? $" ,InnerException: {ex.InnerException.Message}" : "");

                return Ok(new { check = false, message = "Failed to update" });
            }

        }
        public IActionResult ChangePassword(string oldPassword, string newPassword, string confirmPassword, int id)
        {
            if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword))
            {
                return Ok(new { check = false, message = "Old and new passwords are both required" });
            }

            if (newPassword != confirmPassword)
            {
                return Ok(new { check = false, message = "Passwords do not match" });
            }

            var _account = _accountService.Get(id);
            if (_account == null)
            {
                return Ok(new { check = false, message = "Account does not exist" });
            }

            _account.Password = newPassword;
            try
            {
                _accountService.Update(_account);
                _logger.Log(Niche.Core.Enums.LogLevel.INFORMATION, $"Account-password: {_account.ToJson()} was updated on {DateTime.UtcNow} UTC");
                return Ok(new { check = true, message = "Password was changed successfully" });
            }
            catch (Exception ex)
            {
                _logger.Log(Niche.Core.Enums.LogLevel.ERROR, ex.Message + ex.InnerException != null ? $" ,InnerException: {ex.InnerException.Message}" : "");

                return Ok(new { check = false, message = "Failed to update" });
            }
        }

        public IActionResult LoadAll()
        {
            return Ok(_accountService.Search(x => true).OrderByDescending(x => x.CreatedOn).ToList());
        }
    }
}