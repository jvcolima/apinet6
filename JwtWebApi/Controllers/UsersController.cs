using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtWebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public UsersController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }


        //// GET: UsersController
        //[HttpGet]
        //public ActionResult<IEnumerable<User>> Index()
        //{
        //    List<User> users = _userService.GetUsers();
        //    return users;
        //}

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            List<User> users = _userService.GetUsers();
            return users;
        }


    }
}
