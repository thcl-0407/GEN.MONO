using System;
using DatabaseHelper.Enums;
using DatabaseHelper.Entities;
using GenMonoAdmin.Models.Response;

namespace GenMonoAdmin.Builders
{
    public class BuildAccountResponseClient : BaseResponseClient<Admin>
    {
        public BaseResponse<Admin> Build(Tuple<DatabaseResponse, Admin> result)
        {
            var ConvertObject = ConvertDatabaseResponseToResponse(result.Item1);

            if (result != null)
            {
                ConvertObject.Object = result.Item2;
            }

            return ConvertObject;
        }
    }
}
