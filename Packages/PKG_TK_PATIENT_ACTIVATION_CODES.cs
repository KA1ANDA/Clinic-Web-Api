using ClinicWebApi.Models;
using Oracle.ManagedDataAccess.Client;

namespace ClinicWebApi.Packages
{

    public interface IPKG_TK_PATIENT_ACTIVATION_CODES
    {
        public void add_activation_code(ActivationCode userActivation);
        public void delete_activation_code(ActivationCode userActivation);
        public ActivationCode get_activation_code(string email);
    }
    public class PKG_TK_PATIENT_ACTIVATION_CODES:PKG_BASE , IPKG_TK_PATIENT_ACTIVATION_CODES
    {

        IConfiguration configuration;

        public PKG_TK_PATIENT_ACTIVATION_CODES(IConfiguration configuration):base(configuration) { }
        public void add_activation_code(ActivationCode userActivation)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = Connstr;
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.pkg_tk_patient_activation_code.add_activation_code";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("v_email", OracleDbType.Varchar2).Value = userActivation.Email;
            cmd.Parameters.Add("v_activation_code", OracleDbType.Varchar2).Value = userActivation.Code;

            cmd.ExecuteNonQuery();

            conn.Close();
       
        }

        public void delete_activation_code(ActivationCode userActivation)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = Connstr;
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.pkg_tk_patient_activation_code.delete_activation_code";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("v_email", OracleDbType.Varchar2).Value = userActivation.Email;

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public ActivationCode get_activation_code(string email)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = Connstr;
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.pkg_tk_patient_activation_code.get_activation_code";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("v_email", OracleDbType.Varchar2).Value = email;      
            cmd.Parameters.Add("v_result", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();

            ActivationCode userActivationData = null;
            if (reader.Read())
            {
                userActivationData = new ActivationCode();
                userActivationData.Code = reader["activation_code"].ToString();
            }


            conn.Close();
            return userActivationData;
        }
    }
}
