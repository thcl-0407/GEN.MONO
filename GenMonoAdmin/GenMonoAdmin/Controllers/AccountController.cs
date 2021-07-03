using GenMonoAdmin.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using DatabaseHelper.Controls;
using System;
using GenMonoAdmin.Builders;
using Microsoft.AspNetCore.Authorization;

namespace GenMonoAdmin.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [Authorize]
    public class AccountController : Controller
    {
        private IAccountDbHelper _accountDbHelper;

        public AccountController(IAccountDbHelper accountDbHelper)
        {
            _accountDbHelper = accountDbHelper;
        }

        [HttpPost]
        [Route("signin")]
        [AllowAnonymous]
        public JsonResult SignIn(SignInRequestModel request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = _accountDbHelper.SignIn(request.UserName, request.Password);
                    return Json(new BuildAccountResponseClient().Build(result));
                }
                catch (Exception)
                {
                    return Json(CommonResponse.BuildExceptionRespone());
                }
            }

            return Json(CommonResponse.BuildExceptionRespone());
        }

        [HttpPost]
        [Route("generatehash")]
        public dynamic GenerateHash(GenerateHashRequest request)
        {
            return new {
                Id = Guid.NewGuid().ToString(),
                Hash = BCrypt.Net.BCrypt.HashPassword(request.value)
            };
        }
    }
}
