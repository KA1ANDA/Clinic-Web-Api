using ClinicWebApi.Auth;
using ClinicWebApi.EmailSend;
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
    public class AuthenticationController : MainController
    {
        IPKG_TK_AUTHENTICATION pkg_auth;
        IPKG_TK_PATIENTS pkg_patient;
        IPKG_TK_DOCTORS pkg_doctor;
        IJwtManager jwt;
        IEmailSender emailSender;
        
        public AuthenticationController(IPKG_TK_AUTHENTICATION PKG_TK_AUTHENTICATION , IJwtManager JwtManager , IEmailSender emailSender, IPKG_TK_PATIENTS pkg_patient , IPKG_TK_DOCTORS pkg_doctor)
        {
            this.pkg_auth = PKG_TK_AUTHENTICATION;
            this.jwt = JwtManager;
            this.emailSender = emailSender;
            this.pkg_patient = pkg_patient;
            this.pkg_doctor = pkg_doctor;
        }

        [HttpPost]

        public IActionResult authenticate (Login loginData)
        {

            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";


            if (string.IsNullOrEmpty(loginData.Email))
            {
                return BadRequest(new { message = "email address is required !" });
            }

            if (!Regex.IsMatch(loginData.Email, emailPattern))
            {
                return BadRequest(new { message = "Invalid email address." });
            }


            if (string.IsNullOrEmpty(loginData.Password))
            {
                return BadRequest(new { message = "password is required !" });
            }

            User? user = null;
            Token? token = null;

            user = pkg_auth.authenticate(loginData);
            if (user == null) return Unauthorized(new { message = "Email or Password is Incorrect" });

            token = jwt.GetToken(user);

            return Ok(token);
        }

        [HttpPut]
        public async Task<IActionResult> recoverPassword ([FromBody] RecoverPasswordRequest request)
        {
            var newPassword = Guid.NewGuid().ToString().Substring(0, 8);

            var subject = "Your new Generated Password";
            var message = $"Your Password is: {newPassword}";


            await emailSender.SendEmailAsync(request.Email, subject, message);
            pkg_auth.recoverPassword(request.Email, newPassword);
            return Ok();
        }



        [HttpGet] 
        [Authorize]
        public IActionResult get_loged_user()
        
        
        {

            if (Auth?.Role == 0)
            {
                User logedUser = new User();

                User user = new User { Id = Auth.Id };
                logedUser = pkg_patient.get_patient_by_id(user);
                return Ok(logedUser);
            }
            else if (Auth?.Role == 1)
            {
                User logedUser = new User();

                User user = new User { Id = Auth.Id };
                logedUser = pkg_patient.get_patient_by_id(user);
                return Ok(logedUser);

            }
            else if (Auth?.Role == 2)
            {
                Doctor logedDoctor = new Doctor();

                Doctor doctor = new Doctor { Id = Auth.Id };
                logedDoctor = pkg_doctor.get_loged_doctor(doctor);
                return Ok(logedDoctor);


             
            }
            else
            {
                return BadRequest(new { message = "Invalid role" });
            }

        }
    }
}
