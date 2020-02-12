using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//
using System.Configuration;
//
using INDAABIN.DI.CONTRATOS.ModeloNegocios; //objetos Entities
using INDAABIN.DI.CONTRATOS.Negocio;//para uso del BUS

namespace INDAABIN.DI.CONTRATOS.Aplicacion.TablaSMOI
{
    public partial class BusqTablaSMOI : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Contexto"] == null)
                Response.Redirect(ConfigurationManager.AppSettings.Get("URL_SSO") + ConfigurationManager.AppSettings.Get("TokenApp").Replace("-", ""));

            
        }

        protected void ButtonBuscar_Click(object sender, EventArgs e)
        {
            Session["intFolioConceptoResp"] = this.TextBoxFolioSMOI.Text;
            
            Session["URLQueLllama"] = "~/TablaSMOI/BusqTablaSMOI.aspx";
            //Response.Redirect("../EmisionOpinion/AcuseEmisionOpinion.aspx");
            Response.Redirect("../TablaSMOI/AcuseSMOI.aspx");
        }
    }
}