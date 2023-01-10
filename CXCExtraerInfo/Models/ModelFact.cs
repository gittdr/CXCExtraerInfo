using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CXCExtraerInfo.Models
{
    public class ModelFact
    {
        public string uuid { get; set; }
        public string motivo { get; set; }
        public string status { get; set; }

        public string folio { get; set; }
        public string fecha { get; set; }
        public string serie { get; set; }
        public string rfc { get; set; }
        public string pdfAndXmlDownload { get; set; }
        public string pdfDownload { get; set; }
        public string xmlDownload { get; set; }
        public string monto { get; set; }
        public string tipoMoneda { get; set; }

        public string ord_hdrnumber { get; set; }
        public string tcfix { get; set; }

        public DataTable GetOrder(string segmento)
        {
            string cadena2 = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(cadena2))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand("sp_get_order", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@segmento", (object)segmento);
                    selectCommand.ExecuteNonQuery();
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            //selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            connection.Close();
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataTable GetTotalVcp(string folio)
        {
            string cadena2 = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(cadena2))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand("sp_get_monto_vistacartaporte", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@folio", (object)folio);
                    selectCommand.ExecuteNonQuery();
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            //selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            connection.Close();
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataTable GetInvoice(string rorder)
        {
            string cadena2 = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(cadena2))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand("sp_get_invoice", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@rorder", (object)rorder);
                    selectCommand.ExecuteNonQuery();
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            //selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            connection.Close();
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public void InsertCp(string NameFile)
        {
            string cadena2 = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
            //DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(cadena2))
            {

                using (SqlCommand selectCommand = new SqlCommand("usp_registarCp", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@NameFile", (object)NameFile);
                    //selectCommand.Parameters.AddWithValue("@mensaje", (object)mensaje);
                    try
                    {
                        connection.Open();
                        selectCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

        }
        public void InsertRegCp(string NameFile, string col1, string folio, string serie, string Rorder, string ivh_invoicenumber, string ivh_billto, string ivh_totalcharge, string Total, string estatus)
        {
            string cadena2 = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
            //DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(cadena2))
            {

                using (SqlCommand selectCommand = new SqlCommand("usp_registarRegCp", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@NameFile", (object)NameFile);
                    selectCommand.Parameters.AddWithValue("@col1", (object)col1);
                    selectCommand.Parameters.AddWithValue("@folio", (object)folio);
                    selectCommand.Parameters.AddWithValue("@serie", (object)serie);
                    selectCommand.Parameters.AddWithValue("@Rorder", (object)Rorder);
                    selectCommand.Parameters.AddWithValue("@ivh_invoicenumber", (object)ivh_invoicenumber);
                    selectCommand.Parameters.AddWithValue("@ivh_billto", (object)ivh_billto);
                    selectCommand.Parameters.AddWithValue("@ivh_totalcharge", (object)ivh_totalcharge);
                    selectCommand.Parameters.AddWithValue("@Total", (object)Total);
                    selectCommand.Parameters.AddWithValue("@estatus", (object)estatus);
                    try
                    {
                        connection.Open();
                        selectCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

        }

        public void NoregCp(string NameFile, string col1, string mensaje)
        {
            string cadena2 = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
            //DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(cadena2))
            {

                using (SqlCommand selectCommand = new SqlCommand("usp_NoregistarRegCp", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@NameFile", (object)NameFile);
                    selectCommand.Parameters.AddWithValue("@col1", (object)col1);
                    selectCommand.Parameters.AddWithValue("@mensaje", (object)mensaje);
                    try
                    {
                        connection.Open();
                        selectCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

        }

    }
}
