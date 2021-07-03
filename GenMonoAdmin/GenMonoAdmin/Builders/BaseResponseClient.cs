using System;
using DatabaseHelper.Entities;
using DatabaseHelper.Enums;
using GenMonoAdmin.Models.Response;

namespace GenMonoAdmin.Builders
{
    public class BaseResponseClient<T>
    {
        public virtual BaseResponse<T> ConvertDatabaseResponseToResponse(DatabaseResponse databaseResponse)
        {
            var baseResponse = new BaseResponse<T>
            {
                Status = false,
                Code = 404,
                Message = ""
            };

            switch (databaseResponse)
            {
                case DatabaseResponse.NOT_FOUND:
                    baseResponse.Message = "Can't find your information to handle request";
                    break;
                case DatabaseResponse.NOT_EQUAL:
                    baseResponse.Message = "Object is not equal or value is change while request is being handling";
                    break;
                case DatabaseResponse.HAVE_EXCEPTION:
                    baseResponse.Message = "Have a error while request is being handling";
                    break;
                case DatabaseResponse.OBJECT_NULL:
                    baseResponse.Message = "Can't reference to memory area";
                    break;
                case DatabaseResponse.VALUE_NULL:
                    baseResponse.Message = "Value should not be populated data";
                    break;
                case DatabaseResponse.SUCCESSFULLY:
                    baseResponse.Message = "Successfully";
                    baseResponse.Code = 200;
                    baseResponse.Status = true;
                    break;
            }

            return baseResponse;
        }
    }
}
