using ClinicWebApi.Models;
using Oracle.ManagedDataAccess.Client;
using System.Security.AccessControl;

namespace ClinicWebApi.Packages
{

    public interface IPKG_TK_BOOKING
    {
        public void add_booking(Booking booking);
        public List<TimeSlot> get_time_slots();
        public List<Booking> get_doctor_bookings(Booking booking);
        public void delete_booking(Booking booking);
        public void update_booking_description(Booking booking);
        public List<Booking> get_patient_bookings(Booking booking);
        public void update_booking(Booking booking);
    }
    public class PKG_TK_BOOKING:PKG_BASE , IPKG_TK_BOOKING
    {
        IConfiguration configuration;

        public PKG_TK_BOOKING(IConfiguration configuration):base(configuration) {}

        public void add_booking(Booking booking)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = Connstr;
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.pkg_tk_booking.add_booking";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("v_patient_id", OracleDbType.Int32).Value = booking.PatientId;
            cmd.Parameters.Add("v_doctor_id", OracleDbType.Int32).Value = booking.DoctorId;
            cmd.Parameters.Add("v_time_slot_id", OracleDbType.Int32).Value = booking.TimeSlotId;
            cmd.Parameters.Add("v_appointment_date", OracleDbType.Date).Value = booking.AppointmentDate;
            cmd.Parameters.Add("v_description", OracleDbType.Varchar2).Value = booking.Description;


            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public void update_booking(Booking booking)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = Connstr;
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.pkg_tk_booking.update_booking";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("v_id", OracleDbType.Int32).Value = booking.Id;
            cmd.Parameters.Add("v_time_slot_id", OracleDbType.Int32).Value = booking.TimeSlotId;
            cmd.Parameters.Add("v_appointment_date", OracleDbType.Date).Value = booking.AppointmentDate;


            cmd.ExecuteNonQuery();

            conn.Close();
        }


        public List<Booking> get_doctor_bookings(Booking booking)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = Connstr;
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.pkg_tk_booking.get_doctor_bookings";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("v_id", OracleDbType.Int32).Value = booking.DoctorId;
            cmd.Parameters.Add("v_result", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();

            List<Booking> bookings = new List<Booking>();

            while (reader.Read())
            {
                Booking doctorBooking = new Booking();
                doctorBooking.Id = int.Parse(reader["id"].ToString());
                doctorBooking.PatientId = int.Parse(reader["patient_id"].ToString());
                doctorBooking.DoctorId = int.Parse(reader["doctor_id"].ToString());
                doctorBooking.TimeSlotId = int.Parse(reader["time_slot_id"].ToString());
                doctorBooking.AppointmentDate = Convert.ToDateTime(reader["appointment_date"]);
                doctorBooking.Description = reader["description"].ToString();


                bookings.Add(doctorBooking);
            }

            reader.Close();
            conn.Close();
            return bookings;
        }

        public List<Booking> get_patient_bookings(Booking booking)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = Connstr;
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.pkg_tk_booking.get_patient_bookings";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("v_id", OracleDbType.Int32).Value = booking.PatientId;
            cmd.Parameters.Add("v_result", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();

            List<Booking> bookings = new List<Booking>();

            while (reader.Read())
            {
                Booking patientBooking = new Booking();
                patientBooking.Id = int.Parse(reader["id"].ToString());
                patientBooking.PatientId = int.Parse(reader["patient_id"].ToString());
                patientBooking.DoctorId = int.Parse(reader["doctor_id"].ToString());
                patientBooking.TimeSlotId = int.Parse(reader["time_slot_id"].ToString());
                patientBooking.AppointmentDate = Convert.ToDateTime(reader["appointment_date"]);
                patientBooking.Description = reader["description"].ToString();

                bookings.Add(patientBooking);
            }

            reader.Close();
            conn.Close();
            return bookings;
        }


        public void delete_booking(Booking booking)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = Connstr;
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.pkg_tk_booking.delete_booking";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("v_id", OracleDbType.Int32).Value = booking.Id;
         
   

            cmd.ExecuteNonQuery();

            conn.Close();
        }


        public void update_booking_description(Booking booking)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = Connstr;
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.pkg_tk_booking.update_booking_description";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("v_id", OracleDbType.Int32).Value = booking.Id;
            cmd.Parameters.Add("v_description", OracleDbType.Varchar2).Value = booking.Description;

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public List<TimeSlot> get_time_slots()
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = Connstr;
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.pkg_tk_booking.get_timeslots";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("v_result", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();

            List<TimeSlot> timeslots = new List<TimeSlot>();

            while (reader.Read())
            {
             TimeSlot time = new TimeSlot();
                time.Id = int.Parse(reader["id"].ToString());
                time.StartTime = Convert.ToDateTime(reader["start_time"]);
                time.EndTime = Convert.ToDateTime(reader["end_time"]);

                timeslots.Add(time);
            }

            reader.Close();
            conn.Close();
            return timeslots;

        }
    }
}
