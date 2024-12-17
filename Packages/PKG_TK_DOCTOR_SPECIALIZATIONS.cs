using ClinicWebApi.Models;
using Oracle.ManagedDataAccess.Client;

namespace ClinicWebApi.Packages
{

    public interface IPKG_TK_DOCTOR_SPECIALIZATIONS
    {
        public List<Specialization> get_specializations();
    }

    public class PKG_TK_DOCTOR_SPECIALIZATIONS:PKG_BASE , IPKG_TK_DOCTOR_SPECIALIZATIONS
    {
        IConfiguration configuration;

        public PKG_TK_DOCTOR_SPECIALIZATIONS(IConfiguration configuration):base(configuration) { }


        public List<Specialization> get_specializations()
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = Connstr;
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.pkg_tk_doctor_specializations.get_specializations";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("v_result" , OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();

            List<Specialization> specializations = new List<Specialization>();

            while (reader.Read())
            {
                Specialization specialization = new Specialization();
                specialization.Id = int.Parse(reader["specialization_id"].ToString());
                specialization.SpecializationName = reader["specialization"].ToString();
                specialization.Quantity = int.Parse(reader["quantity"].ToString());
                specializations.Add(specialization);
            }

            reader.Close();
            conn.Close();

            return specializations;

        }

    }
}
