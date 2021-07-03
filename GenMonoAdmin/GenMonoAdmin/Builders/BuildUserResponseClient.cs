using System;
using System.Collections.Generic;
using DatabaseHelper.Entities;
using DatabaseHelper.Enums;
using GenMonoAdmin.Models.Response;

namespace GenMonoAdmin.Builders
{
    public class BuildUserResponseClient : BaseResponseClient<User>
    {
        public BaseResponse<User> Build(Tuple<DatabaseResponse, User> result)
        {
            var ConvertObject = ConvertDatabaseResponseToResponse(result.Item1);

            if (result != null)
            {
                ConvertObject.Object = result.Item2;
            }

            return ConvertObject;
        }

        public BaseResponse<User> Build(Tuple<DatabaseResponse, List<User>> result)
        {
            var ConvertObject = ConvertDatabaseResponseToResponse(result.Item1);

            if (result != null)
            {
                ConvertObject.Objects = result.Item2;
            }

            return ConvertObject;
        }
    }
}
