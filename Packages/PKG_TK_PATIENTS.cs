using ClinicWebApi.Models;
using Oracle.ManagedDataAccess.Client;

namespace ClinicWebApi.Packages
{

    public interface IPKG_TK_PATIENTS
    {
        public void add_patient(UserRegistration newPatient);
        public User get_patient_by_id(User user);
    }

    public class PKG_TK_PATIENTS:PKG_BASE , IPKG_TK_PATIENTS
    {
        IConfiguration configuration;

        public PKG_TK_PATIENTS(IConfiguration configuration) : base(configuration) { }

        public void add_patient (UserRegistration newPatient)
        {
            try
            {
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = Connstr;
                conn.Open();

                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "olerning.pkg_tk_patients.add_patient";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add("v_first_name", OracleDbType.Varchar2).Value = newPatient.FirstName;
                cmd.Parameters.Add("v_last_name", OracleDbType.Varchar2).Value = newPatient.LastName;
                cmd.Parameters.Add("v_email", OracleDbType.Varchar2).Value = newPatient.Email;
                cmd.Parameters.Add("v_password", OracleDbType.Varchar2).Value = newPatient.Password;
                cmd.Parameters.Add("v_personal_number", OracleDbType.Varchar2).Value = newPatient.PersonalNumber;

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

        public User get_patient_by_id(User user)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = Connstr;
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.pkg_tk_patients.get_patient_by_id";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("v_id", OracleDbType.Int32).Value = user.Id;
            cmd.Parameters.Add("v_result", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();

            User selectedUser = new User();

            if (reader.Read())
            {
                User userInfo = new User();
                userInfo.Id = int.Parse(reader["id"].ToString());
                userInfo.FirstName = reader["first_name"].ToString();
                userInfo.LastName = reader["last_name"].ToString();
                userInfo.Email = reader["email"].ToString();
                userInfo.PersonalNumber = reader["personal_number"].ToString();
                //user.Photo;
                userInfo.BookingQuantity = int.Parse(reader["booking_quantity"].ToString());
                userInfo.Role = int.Parse(reader["role"].ToString());





                selectedUser=userInfo;
            }
            reader.Close();
            conn.Close();
            return selectedUser;
        }


       
    }

}
