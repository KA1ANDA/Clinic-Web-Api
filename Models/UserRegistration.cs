namespace ClinicWebApi.Models
{
    public class UserRegistration
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PersonalNumber { get; set; }
        public string? ActivationCode { get; set; }


    }
}
