using GenMono.Identities.Response;
using DatabaseHelper.Identities;
using GenMono.Identities.Enums;
using System;

namespace GenMono.Builders
{
    public static class FailResponse
    {
        public static BaseResponse<User> BuildFailResponseForUserController()
        {
            return new BaseResponse<User>
            {
                Status = false,
                Code = Convert.ToInt32(ResponseCode.BAD_REQUEST),
                Message = "Fail to handle your request. Please try it again",
                Payload = null,
                Payloads = null
            };
        }
    }
}
