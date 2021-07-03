using GenMonoAdmin.Models.Response;
 
namespace GenMonoAdmin.Builders
{
    public static class CommonResponse
    {
        public static BaseResponse<object> BuildExceptionRespone()
        {
            return new BaseResponse<object>()
            {
                Status = false,
                Code = 404,
                Message = "Error: Have a exception while handling"
            };
        }

        public static BaseResponse<object> BuildFinishRespone()
        {
            return new BaseResponse<object>()
            {
                Status = true,
                Code = 200,
                Message = "Completed your request but not have a response from system"
            };
        }
    }
}
