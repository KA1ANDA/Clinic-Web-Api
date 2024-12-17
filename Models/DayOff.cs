namespace ClinicWebApi.Models
{
    public class DayOff
    {
        public int? Id { get; set; }
        public int? DoctorId { get; set; }
        public DateTime? DayOffDate { get; set; }

    }
}
