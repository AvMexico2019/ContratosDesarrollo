using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace INDAABIN.DI.CONTRATOS.Aplicacion.Exportar
{
    public class PaginaBase : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            try
            {
                //if (Session["permisos"] == null)
                    //Response.Redirect("~/Login/Login.aspx");
            }
            catch (Exception)
            {
                ;
            }
        }

        public virtual void CargarLista(ListControl lista, DataTable datos, string valor, string texto)
        {
            lista.DataSource = datos;
            lista.DataValueField = valor;
            lista.DataTextField = texto;
            lista.DataBind();

            if (lista.Items.Count == 0)
            {
                lista.Items.Insert(0, new System.Web.UI.WebControls.ListItem("(Seleccionar)", "-1"));
            }

        }

        public virtual void CargarLista(ListControl lista, DataTable datos, string valor, string texto, string valueIni)
        {
            lista.DataSource = datos;
            lista.DataValueField = valor;
            lista.DataTextField = texto;
            lista.DataBind();

            lista.Items.Insert(0, new ListItem(valueIni, "0"));
        }

        public virtual void CargarLista(ListControl lista, DataTable datos, string valor, string texto, string textIni, string valueIni)
        {
            lista.DataSource = datos;
            lista.DataValueField = valor;
            lista.DataTextField = texto;
            lista.DataBind();

            lista.Items.Insert(0, new ListItem(textIni, valueIni));
        }

        public virtual void ExportarExcel(Control control, string fileName)
        {
            ExportExcel reporteExcel;
            try
            {
                reporteExcel = new ExportExcel();
                reporteExcel.AddSection(control);
                reporteExcel.SendToBrowser(Context, fileName);
            }
            catch (Exception ex)
            {
                string excepcion = ex.Message;
            }
        }
        
        public virtual void ExportarExcel(Control control, string fileName, string sCadenaFiltros)
        {
           ExportExcel reporteExcel;

            try
            {

                reporteExcel = new ExportExcel();

                reporteExcel.AddSection(control);

                reporteExcel.SendToBrowser(Context, fileName, sCadenaFiltros);

            }
            catch (Exception ex)
            {
                string excepcion = ex.Message;
            }

        }



        public virtual void ExportarExcel(string title, Control control, string fileName)
        {
            ExportExcel reporteExcel;

            try
            {

                reporteExcel =

                new ExportExcel(title);

                reporteExcel.AddSection(control);

                reporteExcel.SendToBrowser(Context, fileName);

            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al exportar la información a Excel.", ex);
            }

        }

        public virtual void ClearAndAddItemList(ListControl lista)
        {
            lista.Items.Clear();

            lista.Items.Add(new ListItem("(Selecciona)", "-1"));
        }

        public virtual void ClearAndAddItemList(ListControl lista, string value)
        {
            lista.SelectedIndex = -1;
            //lista.Items.Clear();

            //lista.Items.Add(new ListItem("(Selecciona)", value));
        }

        public virtual void ClearAndAddItemList(ListControl lista, string text, string value)
        {
            lista.Items.Clear();

            lista.Items.Add(new ListItem(text, value));
        }

        public virtual void SetSelectedValueList(ListControl lista)
        {
            lista.SelectedValue = "-1";
        }

        public virtual void SetSelectedValueList(ListControl lista, string value)
        {
            lista.SelectedValue = value;
        }

        public virtual void CargarEtapasUltimus(ListControl lista, string textoInicial, string valorInicial)
        {
            lista.Items.Add(new ListItem(textoInicial, valorInicial));

            lista.Items.Add(new ListItem("Revisor", "Revisor"));
            lista.Items.Add(new ListItem("Presupuesto", "Presupuesto"));
            lista.Items.Add(new ListItem("RecibirSolicitud", "RecibirSolicitud"));
            lista.Items.Add(new ListItem("ODT", "ODT"));
            lista.Items.Add(new ListItem("odtSubdir", "odtSubdir"));
            lista.Items.Add(new ListItem("odtDireccion", "odtDireccion"));
            lista.Items.Add(new ListItem("Agenda", "Agenda"));
            lista.Items.Add(new ListItem("sesión", "sesión"));
            lista.Items.Add(new ListItem("CierreSesión", "CierreSesión"));
            lista.Items.Add(new ListItem("Normatividad", "Normatividad"));
            lista.Items.Add(new ListItem("Revisar Proyecto", "Revisar Proyecto"));
            lista.Items.Add(new ListItem("Gestor", "Gestor"));
            lista.Items.Add(new ListItem("Analizar", "Analizar"));
            lista.Items.Add(new ListItem("Revisar", "Revisar"));
            lista.Items.Add(new ListItem("Corregir", "Corregir"));
            lista.Items.Add(new ListItem("Firmar Perito", "Firmar Perito"));
            lista.Items.Add(new ListItem("Firmar DGA", "Firmar DGA"));
            lista.Items.Add(new ListItem("Gestor", "Gestor"));
            lista.Items.Add(new ListItem("AcuerdoVisita", "AcuerdoVisita"));
            lista.Items.Add(new ListItem("Contacta", "Contacta"));
            lista.Items.Add(new ListItem("PrevioVisita", "PrevioVisita"));
            lista.Items.Add(new ListItem("BaseInformativa", "BaseInformativa"));
            lista.Items.Add(new ListItem("Gestionar", "Gestionar"));
            lista.Items.Add(new ListItem("RecibeRespuesta", "RecibeRespuesta"));
            lista.Items.Add(new ListItem("verificaBase", "verificaBase"));
            lista.Items.Add(new ListItem("TrabajoValuatorio", "TrabajoValuatorio"));
            lista.Items.Add(new ListItem("RevisarTrabajo", "RevisarTrabajo"));
            lista.Items.Add(new ListItem("RecibeObservaciones", "RecibeObservaciones"));
            lista.Items.Add(new ListItem("CorregirTrabajo", "CorregirTrabajo"));
            lista.Items.Add(new ListItem("CuerpoColegiado", "CuerpoColegiado"));
            lista.Items.Add(new ListItem("SecretarioComite", "SecretarioComite"));
            lista.Items.Add(new ListItem("Calificacion", "Calificacion"));
            lista.Items.Add(new ListItem("Perito_Dictamen", "Perito_Dictamen"));
            lista.Items.Add(new ListItem("Director_Region_Firma", "Director_Region_Firma"));
            lista.Items.Add(new ListItem("CC_Firma", "CC_Firma"));
            lista.Items.Add(new ListItem("CC2_Firma", "CC2_Firma"));
            lista.Items.Add(new ListItem("Gestor_Valida", "Gestor_Valida"));
        }
    }
}