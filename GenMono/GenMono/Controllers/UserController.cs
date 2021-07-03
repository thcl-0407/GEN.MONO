using System;
using Microsoft.AspNetCore.Mvc;
using JwtManager;
using Microsoft.AspNetCore.Authorization;
using GenMono.Identities.Request;
using Extension;
using DatabaseHelper.Controllers;
using DatabaseHelper.Identities;
using DatabaseHelper.Enums;
using GenMono.Builders;
using System.Threading.Tasks;
using GenMono.Identities.Enums;
using Microsoft.Net.Http.Headers;

namespace GenMono.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private IJwtAuthentication _jwtAuthentication;
        private IUserHelper _userHelper;

        public UserController(IJwtAuthentication jwtAuthentication, IUserHelper userHelper)
        {
            _jwtAuthentication = jwtAuthentication;
            _userHelper = userHelper;
        }

        [HttpPost]
        [Route("email")]
        public async Task<JsonResult> GetUserByEmail(GetUserRequest request)
        {
            /*
            string AccessToken = Request.Headers[HeaderNames.Authorization].ToString().Split("Bearer ")[1];
            string Role = _jwtAuthentication.GetValue(GetClaimType.ROLE, AccessToken);

            if (!Role.Equals(RoleType.ADMIN))
            {
                return Json(FailResponse.BuildFailResponseForUserController());
            }*/

            if (ModelState.IsValid)
            {
                if (request.Payload.IsRightEmail())
                {
                    try
                    {
                        var users = await _userHelper.GetUsersByEmail(request.Payload.Trim());

                        return Json(UserResponseBuilder.Build(DatabaseHelperResponse.SUCCESS, users));
                    }
                    catch
                    {
                        return Json(FailResponse.BuildFailResponseForUserController());
                    }

                }
            }

            return Json(FailResponse.BuildFailResponseForUserController());
        }

        [HttpPost]
        [Route("phone")]
        public async Task<JsonResult> GetUserByPhone(GetUserRequest request)
        {
            /*
            string AccessToken = Request.Headers[HeaderNames.Authorization].ToString().Split("Bearer ")[1];
            string Role = _jwtAuthentication.GetValue(GetClaimType.ROLE, AccessToken);

            if (!Role.Equals(RoleType.ADMIN))
            {
                return Json(FailResponse.BuildFailResponseForUserController());
            }*/

            if (ModelState.IsValid)
            {
                if (request.Payload.IsRightPhoneNo())
                {
                    try
                    {
                        var users = await _userHelper.GetUsersByPhoneNo(request.Payload.Trim());

                        return Json(UserResponseBuilder.Build(DatabaseHelperResponse.SUCCESS, users));
                    }
                    catch
                    {
                        return Json(FailResponse.BuildFailResponseForUserController());
                    }

                }
            }

            return Json(FailResponse.BuildFailResponseForUserController());
        }

        [HttpPost]
        [Route("id")]
        public async Task<JsonResult> GetUserById(GetUserRequest request)
        {
            /*string AccessToken = Request.Headers[HeaderNames.Authorization].ToString().Split("Bearer ")[1];
            string Role = _jwtAuthentication.GetValue(GetClaimType.ROLE, AccessToken);

            if (!Role.Equals(RoleType.ADMIN))
            {
                return Json(FailResponse.BuildFailResponseForUserController());
            }*/

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userHelper.GetUserByID(request.Payload.Trim());
                    var result = new Tuple<DatabaseHelperResponse, User>(DatabaseHelperResponse.SUCCESS, user);

                    return Json(UserResponseBuilder.Build(result));
                }
                catch
                {
                    return Json(FailResponse.BuildFailResponseForUserController());
                }
            }

            return Json(FailResponse.BuildFailResponseForUserController());
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("signin")]
        public JsonResult SignIn(SignInRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = _userHelper.SignIn(request.GetTypeSignIn(),
                        request.EmailOrPhone, request.Password);

                    string AccessToken = null;

                    if (result.Item2 != null)
                    {
                        TokenIdentity identity = new TokenIdentity
                        {
                            UserId = result.Item2.UserID.IsNotNullOrEmpty() ? result.Item2.UserID : "",
                            FullName = result.Item2.FullName.IsNotNullOrEmpty() ? result.Item2.FullName : "",
                            DateOfBirth = result.Item2.DateOfBirth.IsNotNullOrEmpty() ? result.Item2.DateOfBirth : "",
                            Email = result.Item2.Email.IsNotNullOrEmpty() ? result.Item2.Email : "",
                            Gender = result.Item2.Gender.IsNotNullOrEmpty() ? result.Item2.Gender : "",
                            PhoneNo = result.Item2.PhoneNo.IsNotNullOrEmpty() ? result.Item2.PhoneNo : "",
                            Role = Convert.ToString(RoleAccessType.CUSTOMER)
                        };

                        AccessToken = _jwtAuthentication.GenerateAccessToken(identity);
                    }

                    return Json(UserResponseBuilder.BuildForSignIn(result, AccessToken));
                }
                catch
                {
                    Json(FailResponse.BuildFailResponseForUserController());
                }
            }

            return Json(FailResponse.BuildFailResponseForUserController());
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("signup")]
        public JsonResult SignUp(SignUpRequest request)
        {
            if (ModelState.IsValid)
            {
                string HashPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

                User user = new User
                {
                    UserID = Guid.NewGuid().ToString().Trim(),
                    FullName = request.FullName.Trim(),
                    Gender = request.Gender.Trim(),
                    HashPassword = HashPassword.Trim(),
                    DateOfBirth = request.DateOfBirth.Trim(),
                    PhoneNo = request.PhoneNo.Trim().IsNotNullOrEmpty() ? request.PhoneNo.Trim() : null,
                    Email = request.Email.Trim().IsNotNullOrEmpty() ? request.Email.Trim() : null,
                    isActived = Boolean.TrueString.Trim(),
                    isVerifiedEmail = Boolean.FalseString.Trim(),
                    isVerifiedPhone = Boolean.FalseString.Trim()
                };

                try
                {
                    var DatabaseResponse = _userHelper.SignUp(user);
                    var UserResponse = UserResponseBuilder.Build(DatabaseResponse);

                    return Json(UserResponse);
                }
                catch (Exception)
                {
                    return Json(FailResponse.BuildFailResponseForUserController());
                }
            }

            return Json(FailResponse.BuildFailResponseForUserController());
        }

        [HttpPost]
        [Route("changepassword")]
        public JsonResult ChangePassword(ChangePasswordRequest request)
        {
            string AccessToken = Request.Headers[HeaderNames.Authorization].ToString().Split("Bearer ")[1];
            string Role = _jwtAuthentication.GetValue(GetClaimType.ROLE, AccessToken);

            if (!Role.Equals(RoleType.CUSTOMER))
            {
                return Json(FailResponse.BuildFailResponseForUserController());
            }

            if (ModelState.IsValid && !request.OldPassword.Equals(request.NewPassword))
            {
                try
                {
                    string UserID = _jwtAuthentication.GetValue(GetClaimType.USER_ID, AccessToken);
                    var result = _userHelper.UpdatePassword(UserID, request.OldPassword, request.NewPassword);

                    return Json(UserResponseBuilder.Build(result));
                }
                catch
                {
                    return Json(FailResponse.BuildFailResponseForUserController());
                }
            }

            return Json(FailResponse.BuildFailResponseForUserController());
        }

        [HttpPost]
        [Route("forgotpassword")]
        [AllowAnonymous]
        public JsonResult ForgotPassword(ForgotPasswordRequest request)
        {
            if (ModelState.IsValid)
            {
                if (request.EmailOrPhone.IsRightEmail())
                {

                }

                if (request.EmailOrPhone.IsRightPhoneNo())
                {

                }
            }

            return Json(FailResponse.BuildFailResponseForUserController());
        }

        [HttpPost]
        [Route("verifyemail")]
        [AllowAnonymous]
        public dynamic VerifyEmail(VerifyEmailRequest request)
        {
            if (ModelState.IsValid)
            {
                if (request.Email.IsRightEmail())
                {
                    var CurrentUser = _userHelper.GetUserByID(request.UserId).Result;

                    if (CurrentUser != null && CurrentUser.Email.Trim().Equals(request.Email.Trim()))
                    {
                        return new
                        {
                            Status = true,
                            Code = 200
                        };
                    }
                }
            }

            return Json(FailResponse.BuildFailResponseForUserController());
        }

        [HttpPost]
        [Route("updateprofile")]
        [Authorize]
        public JsonResult UpdateProfile(UpdateProfileRequest request)
        {
            string AccessToken = Request.Headers[HeaderNames.Authorization].ToString().Split("Bearer ")[1];
            string Role = _jwtAuthentication.GetValue(GetClaimType.ROLE, AccessToken);

            if (!Role.Equals(RoleType.CUSTOMER))
            {
                return Json(FailResponse.BuildFailResponseForUserController());
            }

            if (ModelState.IsValid)
            {
                User UserToUpdate = new User
                {
                    UserID = _jwtAuthentication.GetValue(GetClaimType.USER_ID, AccessToken),
                    FullName = request.FullName,
                    Country = request.Country,
                    DateOfBirth = request.DateOfBirth,
                    AddressLine = request.AddressLine,
                    Gender = request.Gender,
                    StateOrCity = request.StateOrCity
                };

                var result = _userHelper.UpdateProfile(UserToUpdate);

                return Json(UserResponseBuilder.Build(result));
            }

            return Json(FailResponse.BuildFailResponseForUserController());
        }

        [HttpPost]
        [Route("updateverifystatus")]
        [Authorize]
        public JsonResult UpdateVerifyStatus(UpdateVerifyStatusRequest request)
        {
            string AccessToken = Request.Headers[HeaderNames.Authorization].ToString().Split("Bearer ")[1];
            string Role = _jwtAuthentication.GetValue(GetClaimType.ROLE, AccessToken);

            if (!Role.Equals(RoleType.CUSTOMER))
            {
                return Json(FailResponse.BuildFailResponseForUserController());
            }

            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Email = request.Email,
                    PhoneNo = request.Phone,
                    UserID = _jwtAuthentication.GetValue(GetClaimType.USER_ID, AccessToken)
                };

                try
                {
                    var result = _userHelper.UpdateVerifyStatus(user, request.StatusToUpdate.ToString());

                    return Json(UserResponseBuilder.Build(result));
                }
                catch
                {
                    return Json(FailResponse.BuildFailResponseForUserController());
                }
            }

            return Json(FailResponse.BuildFailResponseForUserController());
        }
    }
}
