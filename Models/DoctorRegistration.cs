namespace ClinicWebApi.Models
{
    public class DoctorRegistration:UserRegistration
    {
        public string SpecializationId { get; set; }
        public IFormFile Cv { get; set; }
        public IFormFile Photo { get; set; }


    }
}
