using ClinicWebApi.CvParse;
using ClinicWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Xml.Linq;

namespace ClinicWebApi.Packages
{
    public interface IPKG_TK_DOCTORS
    {
        public void add_doctor(DoctorRegistration newDoctor, byte[] cvData, byte[] photoData);
        public List<Doctor> get_doctors();
        public Doctor get_loged_doctor(Doctor doctor);
        public List<Doctor> get_doctors_by_category_id(Specialization specialization);
        public List<Doctor> search_doctor(SearchRequest request);
        public Doctor get_doctor_by_id(GetDoctorById doctorId);
        public void add_day_off(DayOff day);

        public List<DayOff> get_doctor_days_off(DayOff day);
    }
    public class PKG_TK_DOCTORS:PKG_BASE, IPKG_TK_DOCTORS
    {
        IConfiguration  configuration;

        public PKG_TK_DOCTORS(IConfiguration configuration) : base(configuration) { }
        
        


        public void add_doctor (DoctorRegistration newDoctor , byte[] cvData , byte[] photoData)
        {
            try
            {
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = Connstr;
                conn.Open();

                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "olerning.pkg_tk_doctors.add_doctor";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add("v_first_name", OracleDbType.Varchar2).Value = newDoctor.FirstName;
                cmd.Parameters.Add("v_last_name", OracleDbType.Varchar2).Value = newDoctor.LastName;
                cmd.Parameters.Add("v_email", OracleDbType.Varchar2).Value = newDoctor.Email;
                cmd.Parameters.Add("v_password", OracleDbType.Varchar2).Value = newDoctor.Password;
                cmd.Parameters.Add("v_personal_number", OracleDbType.Varchar2).Value = newDoctor.PersonalNumber;
                cmd.Parameters.Add("v_specialization_id", OracleDbType.Int32).Value = int.Parse(newDoctor.SpecializationId);


                var blobParam = new OracleParameter("v_cv", OracleDbType.Blob);
                blobParam.Value = (cvData != null) ? cvData : DBNull.Value;
                cmd.Parameters.Add(blobParam);

                var blobPhotoParam = new OracleParameter("v_photo", OracleDbType.Blob);
                blobPhotoParam.Value = (photoData != null) ? photoData : DBNull.Value;
                cmd.Parameters.Add(blobPhotoParam);

                cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (OracleException ex)
    {
                // Handle Oracle-specific exceptions
                if (ex.Number == 20001) // Match the error code for custom raise_application_error
                {
                    throw new Exception("Custom error: " + ex.Message);
                }
                else
                {
                    throw new Exception("Oracle database error occurred: " + ex.Message);
                }
            }


        }

        public List<Doctor> get_doctors()
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = Connstr;
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.pkg_tk_doctors.get_doctors";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("v_result", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();

            List<Doctor> doctors = new List<Doctor>();

            while (reader.Read())
            {
                Doctor doctor = new Doctor();
                doctor.Id = int.Parse(reader["id"].ToString());
                doctor.FirstName = reader["first_name"].ToString();
                doctor.LastName = reader["last_name"].ToString();
                doctor.Email = reader["email"].ToString();
                doctor.SpecializationId = int.Parse(reader["specialization_id"].ToString());
                if (!reader.IsDBNull(reader.GetOrdinal("photo")))
                {
                    byte[] photoData = (byte[])reader["photo"];
                    doctor.Photo = photoData;
                }
                else
                {
                    doctor.Photo = null; 
                }

                doctor.Rating = int.Parse(reader["rating"].ToString());

                doctors.Add(doctor);
            }
            reader.Close();
            conn.Close();
            return doctors;

        }


        public List<Doctor> get_doctors_by_category_id(Specialization specialization)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = Connstr;
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.pkg_tk_doctors.get_doctors_by_category_id";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("v_id", OracleDbType.Int32).Value = specialization.Id;
            cmd.Parameters.Add("v_result", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;


            OracleDataReader reader = cmd.ExecuteReader();

            List<Doctor> doctors = new List<Doctor>();

            while (reader.Read())
            {
                Doctor doctor = new Doctor();
                doctor.Id = int.Parse(reader["id"].ToString());
                doctor.FirstName = reader["first_name"].ToString();
                doctor.LastName = reader["last_name"].ToString();
                doctor.Email = reader["email"].ToString();
                doctor.SpecializationId = int.Parse(reader["specialization_id"].ToString());
                if (!reader.IsDBNull(reader.GetOrdinal("photo")))
                {
                    byte[] photoData = (byte[])reader["photo"];
                    doctor.Photo = photoData;
                }
                else
                {
                    doctor.Photo = null;
                }

                doctor.Rating = int.Parse(reader["rating"].ToString());

                doctors.Add(doctor);
            }
            reader.Close();
            conn.Close();
            return doctors;

        }


        public List<Doctor> search_doctor(SearchRequest request)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = Connstr;
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.pkg_tk_doctors.search_doctor";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("v_char", OracleDbType.Varchar2).Value = request.searchString;
            cmd.Parameters.Add("v_result", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();

            List<Doctor> doctors = new List<Doctor>();

            while (reader.Read())
            {
                Doctor doctor = new Doctor();
                doctor.Id = int.Parse(reader["id"].ToString());
                doctor.FirstName = reader["first_name"].ToString();
                doctor.LastName = reader["last_name"].ToString();
                doctor.SpecializationId = int.Parse(reader["specialization_id"].ToString());
                if (!reader.IsDBNull(reader.GetOrdinal("photo")))
                {
                    byte[] photoData = (byte[])reader["photo"];
                    doctor.Photo = photoData;
                }
                else
                {
                    doctor.Photo = null;
                }

                doctor.Rating = int.Parse(reader["rating"].ToString());

                doctors.Add(doctor);
            }
            reader.Close();
            conn.Close();
            return doctors;
        }

        public Doctor get_loged_doctor(Doctor doctor)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = Connstr;
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.pkg_tk_doctors.get_doctor_by_id";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("v_id", OracleDbType.Int32).Value = doctor.Id;

            cmd.Parameters.Add("v_result", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();

            Doctor logedDoctor = new Doctor();
            
            if (reader.Read())
            {
                Doctor doctorData = new Doctor();
                doctorData.Id = int.Parse(reader["id"].ToString());
                doctorData.FirstName = reader["first_name"].ToString();
                doctorData.LastName = reader["last_name"].ToString();
                doctorData.Email = reader["email"].ToString();
                doctorData.SpecializationId = int.Parse(reader["specialization_id"].ToString());
                doctorData.PersonalNumber = reader["personal_number"].ToString();

               
                if (!reader.IsDBNull(reader.GetOrdinal("photo")))
                {
                    byte[] photoData = (byte[])reader["photo"];
                    doctorData.Photo = photoData;
                }
                else
                {
                    doctorData.Photo = null; 
                }

                doctorData.Rating = int.Parse(reader["rating"].ToString());
                doctorData.BookingQuantity = int.Parse(reader["booking_quantity"].ToString());
                doctorData.Role = int.Parse(reader["role"].ToString());



                logedDoctor=doctorData;
            }
            reader.Close();
            conn.Close();
            return logedDoctor;
        }



        public Doctor get_doctor_by_id(GetDoctorById doctorId)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = Connstr;
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.pkg_tk_doctors.get_doctor_by_id";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("v_id", OracleDbType.Int32).Value = doctorId.Id;

            cmd.Parameters.Add("v_result", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();

            Doctor logedDoctor = new Doctor();

            if (reader.Read())
            {
                Doctor doctorData = new Doctor();
                doctorData.Id = int.Parse(reader["id"].ToString());
                doctorData.FirstName = reader["first_name"].ToString();
                doctorData.LastName = reader["last_name"].ToString();
                doctorData.Email = reader["email"].ToString();
                doctorData.SpecializationId = int.Parse(reader["specialization_id"].ToString());
                doctorData.PersonalNumber = reader["personal_number"].ToString();


                if (!reader.IsDBNull(reader.GetOrdinal("photo")))
                {
                    byte[] photoData = (byte[])reader["photo"];
                    doctorData.Photo = photoData;
                }
                else
                {
                    doctorData.Photo = null;
                }

                //if (!reader.IsDBNull(reader.GetOrdinal("cv")))
                //{
                //    byte[] cvData = (byte[])reader["cv"];
                //    doctorData.Cv = cvData;
                //}
                //else
                //{
                //    doctorData.Cv = null;
                //}

                if (!reader.IsDBNull(reader.GetOrdinal("cv")))
                {
                    byte[] cvData = (byte[])reader["cv"];
                    var cvParser = new CvParser();
     
                    doctorData.Experience = cvParser.ExtractExperience(cvData);
                }
                else
                {
                    doctorData.Experience = null;
                }

                doctorData.Rating = int.Parse(reader["rating"].ToString());
                doctorData.BookingQuantity = int.Parse(reader["booking_quantity"].ToString());
                doctorData.Role = int.Parse(reader["role"].ToString());



                logedDoctor = doctorData;
            }
            reader.Close();
            conn.Close();
            return logedDoctor;
        }


        public void add_day_off(DayOff day)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = Connstr;
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.pkg_doctor_days_off.add_day_off";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("v_doctor_id", OracleDbType.Int32).Value = day.DoctorId;
            cmd.Parameters.Add("v_day_off_date", OracleDbType.Date).Value = day.DayOffDate;


            cmd.ExecuteNonQuery();

            conn.Close();
        }



        public List<DayOff> get_doctor_days_off(DayOff day)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = Connstr;
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.pkg_doctor_days_off.get_doctor_days_off";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("v_doctor_id", OracleDbType.Int32).Value = day.DoctorId;

            cmd.Parameters.Add("v_result", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();

            List<DayOff> daysOff = new List<DayOff>();

            while (reader.Read())
            {
                DayOff dayOff = new DayOff();
                dayOff.Id = int.Parse(reader["id"].ToString());
                dayOff.DayOffDate = Convert.ToDateTime(reader["day_off_date"]);
                daysOff.Add(dayOff);
            }
            reader.Close();
            conn.Close();
            return daysOff;
        }
    }
}
