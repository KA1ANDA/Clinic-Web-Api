namespace ClinicWebApi.Models
{
    public class Doctor : User
    {
        public int SpecializationId { get; set; }
        public int Rating { get; set; }
        //public byte[] Cv { get; set; }
        public List<ExperienceEntry> Experience { get; set; }
    }
}
