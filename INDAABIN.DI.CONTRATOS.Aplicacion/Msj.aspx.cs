using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace INDAABIN.DI.CONTRATOS.Aplicacion
{
    public partial class Msj : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Msj"] != null)
            {

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " +  Session["Msj"].ToString() + "</div>";
                Session["Msj"] = null;
            }
                
        }
    }
}