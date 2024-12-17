namespace ClinicWebApi.Models
{
    public class Booking
    {
        public int? Id { get; set; }
        public int? PatientId { get; set; }
        public int? DoctorId { get; set; }
        public int? TimeSlotId { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string? Description { get; set; }



    }
}
 