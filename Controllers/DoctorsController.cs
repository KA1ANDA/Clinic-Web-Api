using ClinicWebApi.Models;
using ClinicWebApi.Packages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace ClinicWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DoctorsController : MainController
    {
        IPKG_TK_DOCTORS pkg;
        public DoctorsController (IPKG_TK_DOCTORS PKG_TK_DOCTORS)
        {
            this.pkg = PKG_TK_DOCTORS;
        }

        [HttpPost]
        [Authorize]

        public IActionResult add_doctor([FromForm] DoctorRegistration newDoctor)
        {
            //if (newDoctor.FirstName.Length < 5 || newDoctor.PersonalNumber.Length != 11 || newDoctor.Password.Length < 8)
            //{
            //    return BadRequest("Field Validation Error");
            //}

            if (Auth.Role==null || Auth.Role != 0)
            {
                return BadRequest(new { message = "You do not have permission to add doctor" });
            }

            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (string.IsNullOrEmpty(newDoctor.FirstName) || newDoctor.FirstName.Length < 5)
                return BadRequest(new { message = "First name must be at least 5 characters." });

            if (string.IsNullOrEmpty(newDoctor.LastName))
                return BadRequest(new { message = "Last name is required." });

            if (newDoctor.PersonalNumber.Length != 11)
                return BadRequest(new { message = "Personal number must be 11 characters." });

            if (newDoctor.Password.Length < 8)
                return BadRequest(new { message = "Password must be at least 8 characters." });

            if (string.IsNullOrEmpty(newDoctor.Email) || !Regex.IsMatch(newDoctor.Email, emailPattern))
                return BadRequest(new { message = "Invalid email address." });

            byte[] cvData = null;
            byte[] photoData = null;


            if (newDoctor.Cv != null)
            {
                using (var ms = new MemoryStream())
                {
                    newDoctor.Cv.CopyTo(ms);
                    cvData = ms.ToArray(); 
                }
            }



            if (newDoctor.Photo != null)
            {
                using (var ms = new MemoryStream())
                {
                    newDoctor.Photo.CopyTo(ms);
                    photoData = ms.ToArray();
                }
            }

            pkg.add_doctor(newDoctor , cvData , photoData);

            return Ok();
           
        }


        [HttpGet]
        public IActionResult get_doctors()
        {
            List<Doctor> doctors = new List<Doctor>();
            doctors = pkg.get_doctors();
            return Ok(doctors);
        }


        [HttpPost]
        public IActionResult get_doctors_by_category_id(Specialization specialization)
        {
            List<Doctor> doctors = new List<Doctor>();
            doctors = pkg.get_doctors_by_category_id(specialization);
            return Ok(doctors);
        }


        [HttpPost]

        public IActionResult search_doctor(SearchRequest request)
        {

            List<Doctor> doctors = new List<Doctor>();
            doctors = this.pkg.search_doctor(request);
            return Ok(doctors);
        }


        [HttpPost]

        public IActionResult get_doctor_by_id(GetDoctorById doctorId)
        {
            Doctor doctorById = new Doctor();


            doctorById = this.pkg.get_doctor_by_id(doctorId);
            return Ok(doctorById);
        }


        [HttpPost]
        [Authorize]

        public IActionResult add_day_off(DayOff day)
        {

            if (day.DayOffDate.HasValue)
            {
                if (day.DayOffDate.Value.DayOfWeek == DayOfWeek.Saturday || day.DayOffDate.Value.DayOfWeek == DayOfWeek.Sunday)
                {
                    return BadRequest(new { message = "You can not set Day Off for Saturday or Sunday" });
                }
            }


            if ((Auth.Role == 0 || Auth.Role == 2) && (day.DoctorId == Auth.Id || Auth.Role == 0))
            {
                this.pkg.add_day_off(day);

            }
            else
            {
                return BadRequest(new { message = "You do not have permission to set Day Off" });
            }

            
            return Ok();
        }

        [HttpPost]

        public IActionResult get_doctor_days_off (DayOff day)
        {
            List<DayOff> daysOff = new List<DayOff>();
            daysOff = this.pkg.get_doctor_days_off(day);
            return Ok(daysOff);
        }
    }
}
