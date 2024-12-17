using ClinicWebApi.EmailSend;
using ClinicWebApi.Models;
using ClinicWebApi.Packages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Timers;

namespace ClinicWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PatientsController : MainController
    {
        IPKG_TK_PATIENTS pkg;
        IEmailSender emailSender;
        IPKG_TK_PATIENT_ACTIVATION_CODES pkgEmail;
        public PatientsController(IPKG_TK_PATIENTS pkg , IEmailSender emailSender , IPKG_TK_PATIENT_ACTIVATION_CODES pkgEmail)
        {
            this.pkg = pkg;
            this.emailSender = emailSender;
            this.pkgEmail = pkgEmail;

        }

       

        [HttpPost]
        public IActionResult add_patient(UserRegistration newPatient)
        {
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (string.IsNullOrEmpty(newPatient.FirstName) || newPatient.FirstName.Length < 5)
                return BadRequest(new { message = "First name must be at least 5 characters." });

            if (string.IsNullOrEmpty(newPatient.LastName))
                return BadRequest(new { message = "Last name is required." });

            if (newPatient.PersonalNumber.Length != 11)
                return BadRequest(new { message = "Personal number must be 11 characters." });

            if (newPatient.Password.Length < 8)
                return BadRequest(new { message = "Password must be at least 8 characters." });

            if (string.IsNullOrEmpty(newPatient.Email) || !Regex.IsMatch(newPatient.Email, emailPattern))
                return BadRequest(new { message = "Invalid email address." });

            var activationCode = pkgEmail.get_activation_code(newPatient.Email);
            if (string.IsNullOrEmpty(newPatient.ActivationCode) || newPatient.ActivationCode != activationCode?.Code)
            {
                return BadRequest(new { message = "Invalid activation code." });
            }

            pkg.add_patient(newPatient);

            return Ok(new { message = "User registered successfully" });
        }


        [HttpPost]

        public async Task<IActionResult> add_activation_code([FromBody] ActivationCode userActivation)
        {

            if (string.IsNullOrEmpty(userActivation.Email))
            {
                return BadRequest(new { message = "Email is required" });
            }

            var activationCode = Guid.NewGuid().ToString().Substring(0, 8);
            userActivation.Code = activationCode;

            var subject = "Activate Your Registration";
            var message = $"Your activation code is: {activationCode}";

            await emailSender.SendEmailAsync(userActivation.Email, subject, message);
            pkgEmail.add_activation_code(userActivation);

            StartTimerToDeleteActivationCode(userActivation.Email);

            return Ok();
        }

        private void StartTimerToDeleteActivationCode(string email)
        {
           
            System.Timers.Timer timer = new System.Timers.Timer(120000); 
            timer.Elapsed += (sender, e) => DeleteActivationCode(sender, e, email);
            timer.AutoReset = false;
            timer.Start();
        }

        private void DeleteActivationCode(object sender, ElapsedEventArgs e, string email)
        {
          
            var userActivation = new ActivationCode { Email = email };
            pkgEmail.delete_activation_code(userActivation);
        }


    
    }
}
