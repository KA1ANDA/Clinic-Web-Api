using ClinicWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        protected User? Auth
        {
            get
            {
                User? user = null ;

                var currentUser = HttpContext.User;

                if (currentUser != null && currentUser.HasClaim(c => c.Type == "UserID"))
                {
                    user = new User();
                    user.Id = int.Parse(currentUser.Claims.FirstOrDefault(c => c.Type == "UserID").Value);
                    user.Role = int.Parse(currentUser.Claims.FirstOrDefault(c => c.Type == "Role").Value);
                }
                return user;
            }
        }
    }
}
