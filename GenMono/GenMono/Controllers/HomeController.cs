using Microsoft.AspNetCore.Mvc;

namespace GenMono.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class HomeController : Controller
    {
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
