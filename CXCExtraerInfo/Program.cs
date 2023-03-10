using CXCExtraerInfo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CXCExtraerInfo
{
    public class Program
    {
        public string Total;
        public static FacLabControler facLabControler = new FacLabControler();
        static void Main(string[] args)
        {
            
            Program obj = new Program();
            obj.ProcesarInfo();
        }
        public void ProcesarInfo()
        {
            string[] values;
            DirectoryInfo di24a = new DirectoryInfo(@"\\10.223.208.41\Users\Administrator\Documents\FILESCXC");
            //DirectoryInfo di24a = new DirectoryInfo(@"C:\Administración\Proyecto CXC\Complemento");

            FileInfo[] files24a = di24a.GetFiles("*.tsv");


            int cantidad24a = files24a.Length;
            if (cantidad24a > 0)
            {
                foreach (var itema in files24a)
                {
                    //string sourceFile = @"C:\Administración\Proyecto CXC\Complemento\" + itema.Name;
                    string sourceFile = @"\\10.223.208.41\Users\Administrator\Documents\FILESCXC\" + itema.Name;

                    string lna = itema.Name.Replace("CB", "");
                    string NameFile = lna.Replace(".tsv", "");
                    DataTable cres = facLabControler.VerificarCP(NameFile);
                    if (cres.Rows.Count == 0)
                    {
                        facLabControler.InsertCp(NameFile);
                        int counter = 1;
                        foreach (string line in File.ReadLines(sourceFile, Encoding.UTF8))
                        {
                            if (counter > 1)
                            {
                                values = line.Split('\t');
                                string col1 = values[0];
                                string col2 = values[1];
                                string col3 = values[2];

                                if (col1 != "")
                                {
                                    try
                                    {
                                        var request2819 = (HttpWebRequest)WebRequest.Create("https://canal1.xsa.com.mx:9050/bf2e1036-ba47-49a0-8cd9-e04b36d5afd4/cfdis?uuid=" + col1);
                                        var response2819 = (HttpWebResponse)request2819.GetResponse();
                                        var responseString2819 = new StreamReader(response2819.GetResponseStream()).ReadToEnd();
                                        List<ModelFact> separados819 = JsonConvert.DeserializeObject<List<ModelFact>>(responseString2819);
                                        //PASO 2 - SI EXISTE LE ACTUALIZA EL ESTATUS A 9
                                        if (separados819 != null)
                                        {
                                            foreach (var rlist in separados819)
                                            {
                                                string serie = rlist.serie;
                                                string folio = rlist.folio;

                                                DataTable resVcp = facLabControler.GetTotalVcp(folio);
                                                if (resVcp.Rows.Count > 0)
                                                {
                                                    foreach (DataRow itemP in resVcp.Rows)
                                                    {
                                                        //ESTE ES EL TOTAL DE  VISTACARTAPORTE
                                                        decimal totalfaltante = (decimal)(Convert.ToDouble(itemP["Total"]));
                                                        Total = totalfaltante.ToString("F");
                                                    }
                                                }

                                                DataTable res = facLabControler.GetOrder(folio);
                                                if (res.Rows.Count > 0)
                                                {
                                                    foreach (DataRow item in res.Rows)
                                                    {
                                                        string Rorder = item["ord_hdrnumber"].ToString();
                                                        DataTable resu = facLabControler.GetInvoice(Rorder);
                                                        if (resu.Rows.Count > 0 && resu.Rows.Count < 2)
                                                        {
                                                            foreach (DataRow itemt in resu.Rows)
                                                            {
                                                                string ivh_invoicenumber = itemt["ivh_invoicenumber"].ToString();
                                                                string ivh_billto = itemt["ivh_billto"].ToString();
                                                                decimal totalfaltantec = (decimal)(Convert.ToDouble(itemt["ivh_totalcharge"]));
                                                                string ivh_totalcharge = totalfaltantec.ToString("F");
                                                                string estatus = "";

                                                                //AQUI LLENO LA TABLA DEL REPORTE
                                                                facLabControler.InsertRegCp(NameFile, col1, folio, serie, Rorder, ivh_invoicenumber, ivh_billto, ivh_totalcharge, Total, estatus);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //AQUI REGISTRO LOS DEMAS REGISTROS Y ACTUALIZO EL CAMPO DE REVISAR
                                                            foreach (DataRow itemt in resu.Rows)
                                                            {
                                                                string ivh_invoicenumber = itemt["ivh_invoicenumber"].ToString();
                                                                string ivh_billto = itemt["ivh_billto"].ToString();
                                                                decimal totalfaltantec = (decimal)(Convert.ToDouble(itemt["ivh_totalcharge"]));
                                                                string ivh_totalcharge = totalfaltantec.ToString("F");
                                                                string estatus = "Revisar";

                                                                //AQUI LLENO LA TABLA DEL REPORTE
                                                                facLabControler.InsertRegCp(NameFile, col1, folio, serie, Rorder, ivh_invoicenumber, ivh_billto, ivh_totalcharge, Total, estatus);
                                                            }
                                                        }

                                                    }

                                                }
                                                else
                                                {
                                                    //string folio = "1389465";
                                                    DataTable resul = facLabControler.GetInvoiceHeader(folio);
                                                    if (resul.Rows.Count > 0)
                                                    {
                                                        foreach (DataRow itemtr in resul.Rows)
                                                        {
                                                            string ord_hdrnumber = itemtr["ord_hdrnumber"].ToString();
                                                            string ivh_invoicenumber = itemtr["ivh_invoicenumber"].ToString();
                                                            string ivh_billto = itemtr["ivh_billto"].ToString();
                                                            decimal totalfaltantec = (decimal)(Convert.ToDouble(itemtr["ivh_totalcharge"]));
                                                            string ivh_totalcharge = totalfaltantec.ToString("F");
                                                            string estatus = "";
                                                            string vtotal = "Esta es una factura";
                                                            facLabControler.InsertRegCp(NameFile, col1, folio, serie, ord_hdrnumber, ivh_invoicenumber, ivh_billto, ivh_totalcharge, vtotal, estatus);
                                                        }
                                                    }else
                                                    {
                                                        DataTable resulta = facLabControler.GetInvoiceHeaderRemark(folio);
                                                        if (resulta.Rows.Count > 0)
                                                        {
                                                            foreach (DataRow itemir in resulta.Rows)
                                                            {
                                                                string rorderi = itemir["ord_number"].ToString();
                                                                DataTable resu = facLabControler.GetInvoice(rorderi);
                                                                if (resu.Rows.Count > 0 && resu.Rows.Count < 2)
                                                                {
                                                                    foreach (DataRow itemt in resu.Rows)
                                                                    {
                                                                        string ivh_invoicenumber = itemt["ivh_invoicenumber"].ToString();
                                                                        string ivh_billto = itemt["ivh_billto"].ToString();
                                                                        decimal totalfaltantec = (decimal)(Convert.ToDouble(itemt["ivh_totalcharge"]));
                                                                        string ivh_totalcharge = totalfaltantec.ToString("F");
                                                                        string estatus = "";

                                                                        //AQUI LLENO LA TABLA DEL REPORTE
                                                                        facLabControler.InsertRegCp(NameFile, col1, folio, serie, rorderi, ivh_invoicenumber, ivh_billto, ivh_totalcharge, Total, estatus);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    //AQUI REGISTRO LOS DEMAS REGISTROS Y ACTUALIZO EL CAMPO DE REVISAR
                                                                    foreach (DataRow itemt in resu.Rows)
                                                                    {
                                                                        string ivh_invoicenumber = itemt["ivh_invoicenumber"].ToString();
                                                                        string ivh_billto = itemt["ivh_billto"].ToString();
                                                                        decimal totalfaltantec = (decimal)(Convert.ToDouble(itemt["ivh_totalcharge"]));
                                                                        string ivh_totalcharge = totalfaltantec.ToString("F");
                                                                        string estatus = "Revisar";

                                                                        //AQUI LLENO LA TABLA DEL REPORTE
                                                                        facLabControler.InsertRegCp(NameFile, col1, folio, serie, rorderi, ivh_invoicenumber, ivh_billto, ivh_totalcharge, Total, estatus);
                                                                    }
                                                                }
                                                            }
                                                        }else
                                                        {
                                                            string mensaje = "No existe este folio con serie TDRZP";
                                                            facLabControler.NoregCp(NameFile, col1, mensaje);
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                        else
                                        {
                                            CanalTralix(col1, NameFile);
                                            //AQUI VUELVO A BUSCAR EN EL CANAL
                                        }
                                    }
                                    catch (Exception e)
                                    {


                                        CanalTralix(col1, NameFile);
                                    }

                                }
                            }
                            counter++;
                        }
                        string destinationFile = @"\\10.223.208.41\Users\Administrator\Documents\FILESCXCPROCESADAS\" + itema.Name;
                        //string destinationFile = @"C:\Administración\Proyecto CXC\CProcesadas\" + itema.Name;
                        System.IO.File.Move(sourceFile, destinationFile);
                    }
                    else
                    {
                        string mensaje = "Ya se proceso este complemento";
                        string uuidv = "--";
                        facLabControler.NoregCp(NameFile, uuidv, mensaje);
                        //ENVIAR UN CORREO CON EL FOLIO QUE YA ESTA REGISTRADO
                    }
                        

                    

                }
            }
        }

        public void CanalTralix(string col1, string NameFile)
        {
            try
            {
                var request2819 = (HttpWebRequest)WebRequest.Create("https://canal1.xsa.com.mx:9050/bf2e1036-ba47-49a0-8cd9-e04b36d5afd4/cfdis?uuid=" + col1);
                var response2819 = (HttpWebResponse)request2819.GetResponse();
                var responseString2819 = new StreamReader(response2819.GetResponseStream()).ReadToEnd();
                List<ModelFact> separados819 = JsonConvert.DeserializeObject<List<ModelFact>>(responseString2819);
                //PASO 2 - SI EXISTE LE ACTUALIZA EL ESTATUS A 9
                if (separados819 != null)
                {
                    foreach (var rlist in separados819)
                    {
                        string serie = rlist.serie;
                        string folio = rlist.folio;

                        DataTable resVcp = facLabControler.GetTotalVcp(folio);
                        if (resVcp.Rows.Count > 0)
                        {
                            foreach (DataRow itemP in resVcp.Rows)
                            {
                                //ESTE ES EL TOTAL DE  VISTACARTAPORTE
                                decimal totalfaltante = (decimal)(Convert.ToDouble(itemP["Total"]));
                                Total = totalfaltante.ToString("F");
                            }
                        }

                        DataTable res = facLabControler.GetOrder(folio);
                        if (res.Rows.Count > 0)
                        {
                            foreach (DataRow item in res.Rows)
                            {
                                string Rorder = item["ord_hdrnumber"].ToString();
                                DataTable resu = facLabControler.GetInvoice(Rorder);
                                if (resu.Rows.Count > 0 && resu.Rows.Count < 2)
                                {
                                    foreach (DataRow itemt in resu.Rows)
                                    {
                                        string ivh_invoicenumber = itemt["ivh_invoicenumber"].ToString();
                                        string ivh_billto = itemt["ivh_billto"].ToString();
                                        decimal totalfaltantec = (decimal)(Convert.ToDouble(itemt["ivh_totalcharge"]));
                                        string ivh_totalcharge = totalfaltantec.ToString("F");
                                        string estatus = "";

                                        //AQUI LLENO LA TABLA DEL REPORTE
                                        facLabControler.InsertRegCp(NameFile, col1, folio, serie, Rorder, ivh_invoicenumber, ivh_billto, ivh_totalcharge, Total, estatus);
                                    }
                                }
                                else
                                {
                                    //AQUI REGISTRO LOS DEMAS REGISTROS Y ACTUALIZO EL CAMPO DE REVISAR
                                    foreach (DataRow itemt in resu.Rows)
                                    {
                                        string ivh_invoicenumber = itemt["ivh_invoicenumber"].ToString();
                                        string ivh_billto = itemt["ivh_billto"].ToString();
                                        decimal totalfaltantec = (decimal)(Convert.ToDouble(itemt["ivh_totalcharge"]));
                                        string ivh_totalcharge = totalfaltantec.ToString("F");
                                        string estatus = "Revisar";

                                        //AQUI LLENO LA TABLA DEL REPORTE
                                        facLabControler.InsertRegCp(NameFile, col1, folio, serie, Rorder, ivh_invoicenumber, ivh_billto, ivh_totalcharge, Total, estatus);
                                    }
                                }

                            }

                        }
                        else
                        {
                            //string folio = "1389465";
                            DataTable resul = facLabControler.GetInvoiceHeader(folio);
                            if (resul.Rows.Count > 0)
                            {
                                foreach (DataRow itemtr in resul.Rows)
                                {
                                    string ord_hdrnumber = itemtr["ord_hdrnumber"].ToString();
                                    string ivh_invoicenumber = itemtr["ivh_invoicenumber"].ToString();
                                    string ivh_billto = itemtr["ivh_billto"].ToString();
                                    decimal totalfaltantec = (decimal)(Convert.ToDouble(itemtr["ivh_totalcharge"]));
                                    string ivh_totalcharge = totalfaltantec.ToString("F");
                                    string estatus = "";
                                    string vtotal = "Esta es una factura";
                                    facLabControler.InsertRegCp(NameFile, col1, folio, serie, ord_hdrnumber, ivh_invoicenumber, ivh_billto, ivh_totalcharge, vtotal, estatus);
                                }
                            }
                            else
                            {
                                DataTable resulta = facLabControler.GetInvoiceHeaderRemark(folio);
                                if (resulta.Rows.Count > 0)
                                {
                                    foreach (DataRow itemir in resulta.Rows)
                                    {
                                        string rorderi = itemir["ord_number"].ToString();
                                        DataTable resu = facLabControler.GetInvoice(rorderi);
                                        if (resu.Rows.Count > 0 && resu.Rows.Count < 2)
                                        {
                                            foreach (DataRow itemt in resu.Rows)
                                            {
                                                string ivh_invoicenumber = itemt["ivh_invoicenumber"].ToString();
                                                string ivh_billto = itemt["ivh_billto"].ToString();
                                                decimal totalfaltantec = (decimal)(Convert.ToDouble(itemt["ivh_totalcharge"]));
                                                string ivh_totalcharge = totalfaltantec.ToString("F");
                                                string estatus = "";

                                                //AQUI LLENO LA TABLA DEL REPORTE
                                                facLabControler.InsertRegCp(NameFile, col1, folio, serie, rorderi, ivh_invoicenumber, ivh_billto, ivh_totalcharge, Total, estatus);
                                            }
                                        }
                                        else
                                        {
                                            //AQUI REGISTRO LOS DEMAS REGISTROS Y ACTUALIZO EL CAMPO DE REVISAR
                                            foreach (DataRow itemt in resu.Rows)
                                            {
                                                string ivh_invoicenumber = itemt["ivh_invoicenumber"].ToString();
                                                string ivh_billto = itemt["ivh_billto"].ToString();
                                                decimal totalfaltantec = (decimal)(Convert.ToDouble(itemt["ivh_totalcharge"]));
                                                string ivh_totalcharge = totalfaltantec.ToString("F");
                                                string estatus = "Revisar";

                                                //AQUI LLENO LA TABLA DEL REPORTE
                                                facLabControler.InsertRegCp(NameFile, col1, folio, serie, rorderi, ivh_invoicenumber, ivh_billto, ivh_totalcharge, Total, estatus);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    string mensaje = "No existe este folio con serie TDRZP";
                                    facLabControler.NoregCp(NameFile, col1, mensaje);
                                }
                            }

                        }
                    }

                }
                else
                {
                    string mensaje = "No se puedo procesar";
                    facLabControler.NoregCp(NameFile, col1, mensaje);
                }
            }
            catch (Exception e)
            {

                
                    string mensaje = "No se puedo procesar";
                    facLabControler.NoregCp(NameFile, col1, mensaje);
                
                //Console.WriteLine("No se puedo procesar");
            }
        }
    }
}
