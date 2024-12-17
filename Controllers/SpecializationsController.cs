using ClinicWebApi.Models;
using ClinicWebApi.Packages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SpecializationsController : MainController
    {
        IPKG_TK_DOCTOR_SPECIALIZATIONS pkg;
        public SpecializationsController(IPKG_TK_DOCTOR_SPECIALIZATIONS pkg)
        {
            this.pkg = pkg;
        }

        [HttpGet]

        public IActionResult get_specializations()
        {
            List<Specialization> specializations = new List<Specialization>();
            specializations = pkg.get_specializations();

            return Ok(specializations);
        }
    }
}
