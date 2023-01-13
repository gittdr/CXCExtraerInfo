using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CXCExtraerInfo.Models
{
    public class FacLabControler
    {
        public ModelFact modelFact = new ModelFact();
        public DataTable GetOrder(string segmento)
        {
            return this.modelFact.GetOrder(segmento);
        }
        public DataTable GetTotalVcp(string folio)
        {
            return this.modelFact.GetTotalVcp(folio);
        }
        public DataTable GetInvoiceHeader(string folio)
        {
            return this.modelFact.GetInvoiceHeader(folio);
        }
        public DataTable GetInvoiceHeaderRemark(string folio)
        {
            return this.modelFact.GetInvoiceHeaderRemark(folio);
        }
        public DataTable VerificarCP(string NameFile)
        {
            return this.modelFact.VerificarCP(NameFile);
        }
        public void InsertCp(string NameFile)
        {
            this.modelFact.InsertCp(NameFile);
        }
        public void InsertRegCp(string NameFile, string col1, string folio, string serie, string Rorder, string ivh_invoicenumber, string ivh_billto, string ivh_totalcharge, string Total, string estatus)
        {
            this.modelFact.InsertRegCp(NameFile,col1,folio,serie,Rorder,ivh_invoicenumber,ivh_billto,ivh_totalcharge,Total,estatus);
        }
        public void NoregCp(string NameFile, string col1, string mensaje)
        {
            this.modelFact.NoregCp(NameFile, col1, mensaje);
        }
        public DataTable GetInvoice(string rorder)
        {
            return this.modelFact.GetInvoice(rorder);
        }
    }
}
