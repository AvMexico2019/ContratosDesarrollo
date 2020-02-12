using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//
using System.Configuration;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
//
using INDAABIN.DI.CONTRATOS.ModeloNegocios; //objetos Entities
using INDAABIN.DI.CONTRATOS.Negocio;//para uso del BUS
using INDAABIN.DI.ModeloNegocio;


namespace INDAABIN.DI.CONTRATOS.Aplicacion.EmisionOpinion
{   
    public partial class FundamentoLegalCpto : System.Web.UI.Page
    {

        String Msj;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LabelInfo.Text = String.Empty;
            if (!IsPostBack)
            {
                this.MostrarFundamento();
            }
        }

        private void MostrarFundamento()
        {
            string NumOrdenConcepto = Request.QueryString["NumOrdenCpto"].ToString();
            ConceptoRespValor objConceptoRespValor; // = (ConceptoRespValor)Session["objConceptoRespValor"];
            objConceptoRespValor = new NGConceptoRespValor().ObtenerFundamentoLegalCpto(Convert.ToByte(2), Convert.ToDecimal(NumOrdenConcepto));

            if (objConceptoRespValor != null)
            {
                this.LabelTema.Text = objConceptoRespValor.DescripcionTema;
                this.LabelNumCpto.Text = NumOrdenConcepto;
                this.LabelDescCpto.Text = objConceptoRespValor.DescripcionConcepto;
                this.LabelFundamentoLegalCpto.Text = objConceptoRespValor.FundamentoLegal;
            }
        }

        private void MostrarFundamento2()
        {
            string NumOrdenCpto = Request.QueryString["NumPregunta"];
            string[] words = NumOrdenCpto.Split('-');

            NumOrdenCpto.IndexOf("-");
            NumOrdenCpto = words[0];
            try
            {
                this.LabelNumCpto.Text = NumOrdenCpto;
                ConceptoRespValor objConceptoRespValor = new NGConceptoRespValor().ObtenerFundamentoLegalCpto(Convert.ToByte(2), Convert.ToDecimal(NumOrdenCpto));

                if (objConceptoRespValor != null)
                {
                    this.LabelTema.Text = objConceptoRespValor.DescripcionTema;
                    this.LabelNumCpto.Text = objConceptoRespValor.NumOrden.ToString();
                    this.LabelDescCpto.Text = objConceptoRespValor.DescripcionConcepto;
                    this.LabelFundamentoLegalCpto.Text = objConceptoRespValor.FundamentoLegal;


                }
            }
            catch (SqlException ex)
            {
                Msj = ex.InnerException == null ? "" : ex.InnerException.Message;
                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                
            }
            catch (Exception ex)
            {
                Msj = ex.InnerException == null ? "" : ex.InnerException.Message;
                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
            }


        }
    }
}