using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GenMonoAdmin.Models.Requests;
using Extension;
using GenMonoAdmin.Builders;
using DatabaseHelper.Controls;

namespace GenMonoAdmin.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private IUserDbHelper _userDbHelper;

        public UserController(IUserDbHelper userDbHelper)
        {
            _userDbHelper = userDbHelper;
        }

        [HttpPost]
        [Route("phone")]
        public JsonResult GetUsersByPhone(GetUserRequest request)
        {
            if (request.Phone.isNullOrEmpty())
            {
                return Json(CommonResponse.BuildExceptionRespone());
            }
            else
            {
                var result = _userDbHelper.GetUsersByPhone(request.Phone);

                return Json(new BuildUserResponseClient().Build(result));
            }
        }

        [HttpPost]
        [Route("email")]
        public JsonResult GetUsersByEmail(GetUserRequest request)
        {
            if (request.Email.isNullOrEmpty())
            {
                return Json(CommonResponse.BuildExceptionRespone());
            }
            else
            {
                var result = _userDbHelper.GetUsersByEmail(request.Email);

                return Json(new BuildUserResponseClient().Build(result));
            }
        }
    }
}
