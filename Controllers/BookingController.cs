using ClinicWebApi.Models;
using ClinicWebApi.Packages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BookingController : MainController
    {
        IPKG_TK_BOOKING pkg_booking;

        public BookingController(IPKG_TK_BOOKING pkg_booking) {
            this.pkg_booking = pkg_booking;
        }


        [HttpPost]
        [Authorize]

        public IActionResult add_booking(Booking booking)
        {
            if(Auth.Role == 1)
            {
                this.pkg_booking.add_booking(booking);
            }
            else
            {
                return BadRequest(new { message = "You do not have permission to  add booking" });
            }


            return Ok();
        }


        [HttpPut]
        [Authorize]

        public IActionResult update_booking(Booking booking)
        {
            if (booking.AppointmentDate.HasValue)
            {
                if (booking.AppointmentDate.Value.DayOfWeek == DayOfWeek.Saturday || booking.AppointmentDate.Value.DayOfWeek == DayOfWeek.Sunday)
                {
                    return BadRequest(new { message = "Bookings cannot be updated for Saturday or Sunday" });
                }
            }

            if (Auth.Role == 2 || Auth.Role == 0)
            {
               this.pkg_booking.update_booking(booking);
            }
            else
            {
                return BadRequest(new { message = "You do not have permission to update booking" });
            }
          
            return Ok();
        }

        [HttpPost]

        public IActionResult get_doctor_bookings(Booking booking)
        {
            List<Booking> bookings = new List<Booking>();
            bookings = this.pkg_booking.get_doctor_bookings(booking);

            return Ok(bookings);
        }

        [HttpPost]
        [Authorize]

        public IActionResult get_patient_bookings(Booking booking)
        {
            List<Booking> bookings = new List<Booking>();
                  
            bookings = this.pkg_booking.get_patient_bookings(booking);
           
            return Ok(bookings);
        }


        [HttpPost]
        [Authorize]
        public IActionResult delete_booking(Booking booking)
        {
            if (Auth.Role == 1 && Auth.Id != booking.PatientId)
            {
                return BadRequest(new { message = "You do not have permission to delete booking that is not yours" });
            }

            if (Auth.Role == 2 && Auth.Id != booking.DoctorId)
            {
                return BadRequest(new { message = "You do not have permission to delete booking of other doctor" });
            }

            this.pkg_booking.delete_booking(booking);
            return Ok();
        }


        [HttpPut]
        [Authorize]

        public IActionResult update_booking_description(Booking booking)
        {
            this.pkg_booking.update_booking_description(booking);
            return Ok();
        }

        [HttpGet]

        public IActionResult get_time_slots()
        {
            List<TimeSlot> timeslots = new List<TimeSlot>();
            timeslots = this.pkg_booking.get_time_slots();

            return Ok(timeslots);
        }
    }
}
