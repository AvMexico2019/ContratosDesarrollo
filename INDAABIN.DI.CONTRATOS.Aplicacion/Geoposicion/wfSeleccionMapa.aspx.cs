using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace INDAABIN.DI.CONTRATOS.Aplicacion.Geoposicion
{
    public partial class wfSeleccionMapa : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.CacheControl = "no-cache";

            if (!this.IsPostBack)
            {
                if (Request.QueryString["EstadoId"] != null)
                {
                    string EstadoId = Request.QueryString["EstadoId"];
                    if (EstadoId.ToString().Length == 1)
                        this.Edo.Value = "0" + EstadoId.ToString();
                    else
                        this.Edo.Value = EstadoId.ToString();
                }

                if (Request.QueryString["MunicipioId"] != null)
                {
                    string MunicipioId = Request.QueryString["MunicipioId"];
                    if (MunicipioId.ToString().Length == 1)
                        this.Mun.Value = "00" + MunicipioId.ToString();
                    else if (MunicipioId.ToString().Length == 2)
                        this.Mun.Value = "0" + MunicipioId.ToString();
                    else if (MunicipioId.ToString().Length == 3)
                        this.Mun.Value = MunicipioId.ToString();
                }

                if (Request.QueryString["TipoGeometria"] != null)
                    this.tipoGeometria.Value = Request.QueryString["TipoGeometria"];
                else
                    this.tipoGeometria.Value = string.Empty;

                if (Request.QueryString["Wkt"] != null)
                    this.wkt.Value = Request.QueryString["Wkt"];
                else
                    this.wkt.Value = string.Empty;

                if (Request.QueryString["vX"] != null)
                    this.x.Value = Request.QueryString["vX"];
                else
                    this.x.Value = string.Empty;

                if (Request.QueryString["vY"] != null)
                    this.y.Value = Request.QueryString["vY"];
                else
                    this.y.Value = string.Empty;

                if (Request.QueryString["Editar"] != null)
                    this.Editar.Value = Request.QueryString["Editar"];
                else
                    this.Editar.Value = "true";

                if (Request.QueryString["CP"] != null)
                    this.CP.Value = Request.QueryString["CP"];
                else
                    this.CP.Value = string.Empty;
            }
        }
    }
}