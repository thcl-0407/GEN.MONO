using System;
using GenMono.Identities.Enums;
using GenMono.Identities.Response;
using DatabaseHelper.Identities;
using DatabaseHelper.Enums;
using System.Collections.Generic;

namespace GenMono.Builders
{
    public static class UserResponseBuilder
    {
        public static BaseResponse<User>  Build(Tuple<DatabaseHelperResponse, User> DatabaseResponse) {
            var temp = BuildBaseResponse(DatabaseResponse.Item1);

            if (DatabaseResponse.Item2 != null)
            {
                DatabaseResponse.Item2.HashPassword = null;
            }

            return new BaseResponse<User>
            {
                Status = temp.Status,
                Code = temp.Code,
                Message = temp.Message,
                Payload = DatabaseResponse.Item2
            };
        }

        public static BaseResponse<User> BuildForSignIn(Tuple<DatabaseHelperResponse, User> DatabaseResponse, string AccessToken)
        {
            var temp = BuildBaseResponse(DatabaseResponse.Item1);

            if (DatabaseResponse.Item2 != null)
            {
                DatabaseResponse.Item2.HashPassword = null;
            }

            return new BaseResponse<User>
            {
                Status = temp.Status,
                Code = temp.Code,
                Message = temp.Message,
                Payload = DatabaseResponse.Item2,
                AccessToken = AccessToken
            };
        }

        public static BaseResponse<User> Build(DatabaseHelperResponse databaseHelperResponse, List<User> users)
        {
            var temp = BuildBaseResponse(databaseHelperResponse);

            if (users != null)
            {
                foreach (var user in users)
                {
                    user.HashPassword = null;
                }
            }

            return new BaseResponse<User>
            {
                Status = temp.Status,
                Code = temp.Code,
                Message = temp.Message,
                Payloads = users
            };
        }

        private static BaseResponse<User> BuildBaseResponse(DatabaseHelperResponse databaseHelperResponse)
        {
            int Code = Convert.ToInt32(ResponseCode.BAD_REQUEST);
            bool Status = false;
            string Message = null;

            switch (databaseHelperResponse)
            {
                case DatabaseHelperResponse.SUCCESS:
                    Code = Convert.ToInt32(ResponseCode.CREATED);
                    Status = true;
                    Message = "Successfully";
                    break;
                case DatabaseHelperResponse.HAVE_EXCEPTION:
                    Code = Convert.ToInt32(ResponseCode.BAD_REQUEST);
                    Status = false;
                    break;
                case DatabaseHelperResponse.OBJECT_IS_NULL:
                    Code = Convert.ToInt32(ResponseCode.NOT_ACCEPTABLE);
                    Status = false;
                    Message = null;
                    break;
                case DatabaseHelperResponse.NOT_EQUAL:
                    Code = Convert.ToInt32(ResponseCode.FORBIDDEN);
                    Status = false;
                    Message = null;
                    break;
                case DatabaseHelperResponse.NOT_FOUND:
                    Code = Convert.ToInt32(ResponseCode.NOT_FOUND);
                    Status = false;
                    Message = null;
                    break;
                case DatabaseHelperResponse.VALUE_IS_NULL_OR_EMPTY:
                    Code = Convert.ToInt32(ResponseCode.NOT_ACCEPTABLE);
                    Status = false;
                    Message = null;
                    break;
            }

            return new BaseResponse<User>
            {
                Status = Status,
                Code = Code,
                Message = Message
            };
        }
    }
}
