using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GenMonoAdmin.Controllers
{
    [ApiController]
    [Route("/api/admin")]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        [Route("index")]
        public dynamic Index()
        {
            return new
            {
                Status = true,
                Code = 200,
                Message = "Ready to connect"
            };
        }
    }
}
