using ClinicWebApi.Models;
using Oracle.ManagedDataAccess.Client;

namespace ClinicWebApi.Packages
{

    public interface IPKG_TK_AUTHENTICATION
    {
        public User authenticate(Login loginData);
        public void recoverPassword(string email, string newPassword);
    }

    public class PKG_TK_AUTHENTICATION:PKG_BASE , IPKG_TK_AUTHENTICATION
    {
        IConfiguration configuration;

        public PKG_TK_AUTHENTICATION (IConfiguration configuration) : base(configuration) { }

        public User authenticate(Login loginData)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = Connstr;
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.pkg_tk_authentication.authenticate";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("v_email" , OracleDbType.Varchar2).Value = loginData.Email;
            cmd.Parameters.Add("v_password", OracleDbType.Varchar2).Value = loginData.Password;
            cmd.Parameters.Add("v_result", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();

            User? user = null;

            if (reader.Read())
            {
                user = new User();
                user.Id = int.Parse(reader["id"].ToString());
                user.Role = int.Parse(reader["role"].ToString());
                


            }

            conn.Close();
            return user;

        }

        public void recoverPassword(string email , string newPassword)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = Connstr;
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.pkg_tk_authentication.recoverPassword";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("v_email", OracleDbType.Varchar2).Value = email;
            cmd.Parameters.Add("v_new_password", OracleDbType.Varchar2).Value = newPassword;

            cmd.ExecuteNonQuery();

            conn.Close();

        }
    }
}
