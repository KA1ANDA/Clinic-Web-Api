namespace ClinicWebApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PersonalNumber { get; set; }
        public byte[] Photo { get; set; }
        public int BookingQuantity { get; set; }
        public int Role { get; set; }


    }
}
