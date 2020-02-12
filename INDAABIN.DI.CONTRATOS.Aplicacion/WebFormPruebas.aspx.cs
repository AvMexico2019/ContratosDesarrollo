using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
//
using System.Text.RegularExpressions;
//
using INDAABIN.DI.CONTRATOS.ModeloNegocios; //objetos Entities
using INDAABIN.DI.CONTRATOS.Negocio;
using INDAABIN.DI.ModeloNegocio;//interconexion con el BUS

namespace INDAABIN.DI.CONTRATOS.Aplicacion
{
    public partial class WebFormPruebas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Build_Table();
        }

        protected void Build_Table()
        {
            //create table header row and cells
            TableHeaderRow hr_header = new TableHeaderRow();
            TableHeaderCell hc_cell = new TableHeaderCell();
            hc_cell.Text = "This column contains a button";
            hr_header.Cells.Add(hc_cell);
            tblData.Rows.Add(hr_header);

            //create the cell to contain our button
            TableRow row = new TableRow();
            TableCell cell_with_button = new TableCell();

            //create the button
            Button btn1 = new Button();
            btn1.Text = "Holaaa";
            btn1.Click += new EventHandler(this.btn1_Click);

            //add button to cell, cell to row, and row to table
            cell_with_button.Controls.Add(btn1);
            row.Cells.Add(cell_with_button);
            tblData.Rows.Add(row);
        }

        protected void btn1_Click(Object sender, EventArgs e)
        {
            LabelInfo.Text = "HOLAAAAAA";
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("WebFormPruebasConMasterPage.aspx");
            return;
           // string strConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["cnArrendamientoInmueble"].ConnectionString;

           // SqlConnection SqlConnectionBD = new System.Data.SqlClient.SqlConnection(strConnectionString);
           //try
           //{
           //      SqlConnectionBD.Open();
           //      this.LabelInfo.Text = "Conexion OK";
           //}
              
           // catch (Exception ex)
           //{
           //    this.LabelInfo.Text = ex.Message;
           //}


        }

        protected void Button2_Click(object sender, EventArgs e)
        {

            
            //quitar espacios, acentos, Ñ y en mayusculas
          
           this.TextBoxAcentos.Text =  QuitarAcentosTexto(this.TextBoxAcentos.Text.Replace(" ", "").ToUpper());
           

         
        }

        private string QuitarAcentosTexto(string Texto)
        {
            string textoNormalizado = Texto.Normalize(System.Text.NormalizationForm.FormD);
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9 ]");
            string textoSinAcentos = reg.Replace(textoNormalizado, "");
            return textoSinAcentos;
        }



        // /*******CREAR UN DROPDONWLIST DINAMICAMENTE***************
        // http://www.aspsnippets.com/Articles/Creating-Dynamic-DropDownList-Controls-in-ASP.Net.aspx

        Panel pnlDropDownList;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            //Create a Dynamic Panel
            pnlDropDownList = new Panel();
            pnlDropDownList.ID = "pnlDropDownList";
            pnlDropDownList.BorderWidth = 1;
            pnlDropDownList.Width = 300;
            this.form1.Controls.Add(pnlDropDownList);

            //Create a LinkDynamic Button to Add TextBoxes
            LinkButton btnAddDdl = new LinkButton();
            btnAddDdl.ID = "btnAddDdl";
            btnAddDdl.Text = "Add DropDownList";
            btnAddDdl.Click += new System.EventHandler(btnAdd_Click);
            this.form1.Controls.Add(btnAddDdl);

            //Recreate Controls
            RecreateControls("ddlDynamic", "DropDownList");
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            int cnt = FindOccurence("ddlDynamic");
            CreateDropDownList("ddlDynamic-" + Convert.ToString(cnt + 1));
        }

        private void RecreateControls(string ctrlPrefix, string ctrlType)
        {
            string[] ctrls = Request.Form.ToString().Split('&');
            int cnt = FindOccurence(ctrlPrefix);
            if (cnt > 0)
            {
                for (int k = 1; k <= cnt; k++)
                {
                    for (int i = 0; i < ctrls.Length; i++)
                    {
                        if (ctrls[i].Contains(ctrlPrefix + "-" + k.ToString())
                            && !ctrls[i].Contains("EVENTTARGET"))
                        {
                            string ctrlID = ctrls[i].Split('=')[0];

                            if (ctrlType == "DropDownList")
                            {
                                CreateDropDownList(ctrlID);
                            }
                            break;
                        }
                    }
                }
            }
        }

        private int FindOccurence(string substr)
        {
            string reqstr = Request.Form.ToString();
            return ((reqstr.Length - reqstr.Replace(substr, "").Length)
                              / substr.Length);
        }

        private void CreateDropDownList(string ID)
        {
            DropDownList ddl = new DropDownList();
            ddl.ID = ID;
            ddl.Items.Add(new ListItem("--Select--", ""));
            ddl.Items.Add(new ListItem("One", "1"));
            ddl.Items.Add(new ListItem("Two", "2"));
            ddl.Items.Add(new ListItem("Three", "3"));
            ddl.AutoPostBack = true;
            ddl.SelectedIndexChanged += new EventHandler(OnSelectedIndexChanged);
            pnlDropDownList.Controls.Add(ddl);

            Literal lt = new Literal();
            lt.Text = "<br />";
            pnlDropDownList.Controls.Add(lt);
        }

        protected void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            string ID = ddl.ID;
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert",
                         "<script type = 'text/javascript'>alert('" + ID +
                          " fired SelectedIndexChanged event');</script>");

            //Place the functionality here
        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            Int32 valor;
          //  valor = Convert.ToInt32(string.Format("{0}", TextBox1.Text));
            valor = Convert.ToInt32(Convert.ToDecimal(this.TextBox1.Text));
        }

        protected void ButtonEnviarEmail_Click(object sender, EventArgs e)
        {
            EnviarCorreo objEnviaEmail = new EnviarCorreo();
            try
            {
                string fileContent;
                using (System.IO.StreamReader Reader = new System.IO.StreamReader(@"C:\Users\desa10\Documents\CodifgoFuente\Net\Contratos\INDAABIN.DI.CONTRATOS.VER1.0\INDAABIN.DI.CONTRATOS.Aplicacion\HtmlPrueba.html"))
                {
                    StringBuilder Sb = new StringBuilder();
                    fileContent = Reader.ReadToEnd();
                }

                //objEnviaEmail.EnviaCorreo(this.TextBoxEmail.Text.Trim(), "Prueba!", fileContent);
                //if (objEnviaEmail.Exito)
                //{
                //    LabelInfoEmail.Text = "OK envio Email";
                //}

            }
            catch(Exception ex)
            {
                LabelInfoEmail.Text = ex.Message;
            }

        }

        protected void ButtonSecuencial_Click(object sender, EventArgs e)
        {
            try
            {
                SolicitudAvaluos objSolicitudAvaluos = null;
                objSolicitudAvaluos = new NG().ObtenerJustipreciacionAvaluos(this.TextBoxSecuencial.Text.Trim());

                if (objSolicitudAvaluos == null)
                {
                    this.LabelInfoSec.Text = "No existe";

                }
                else
                {
                    string SupRentable = null;
                    //existen justipreciaciones de renta que no tienen SuperficieRentableDictaminado, entonces se tomara la capturada
                    if (objSolicitudAvaluos.SuperficieRentableDictaminado.Value > 0 && String.IsNullOrEmpty(objSolicitudAvaluos.SuperficieRentableDictaminado.ToString()) == false)
                        //se toma como 1ra opcion, siempre la Sup. rentable dictaminada
                        SupRentable = objSolicitudAvaluos.SuperficieRentableDictaminado.ToString();
                    else//la sup. rentable que se toma, es la captura por el promovente en la solicitud
                        SupRentable = objSolicitudAvaluos.SuperficieRentable.ToString();

                    this.LabelInfoSec.Text = "Estatus: " + objSolicitudAvaluos.Estatus +
                      "</br>Estado: " + objSolicitudAvaluos.EstadoDescripcion +
                       "</br>Mpo: " + objSolicitudAvaluos.MunicipioDescripcion +
                       "</br>CP: " + objSolicitudAvaluos.CP +
                        "</br># ext: " + objSolicitudAvaluos.NoExterior +
                       "</br>Fecha Dictamen: " + objSolicitudAvaluos.FechaDictamen +
                       "</br>Sup. m2 : " + SupRentable +
                        "</br>Unidad de medida rentable: " + objSolicitudAvaluos.UnidadMedidaRentable +
                       "</br>Monto Dictaminado $: " + objSolicitudAvaluos.MontoDictaminado;
                }
            }
            catch (Exception ex)
            {
                this.LabelInfoSec.Text = ex.Message;
            }
        }




        protected void DV_Click(object sender, EventArgs e)
        {
            int DV;
            DV = UtilContratosArrto.Digitoverificador(Convert.ToInt32(this.TextBoxIdEdo.Text), Convert.ToInt32(TextBoxUltConsecutivo.Text));
            this.LabelDV.Text = "Digito verificador: "+  DV.ToString();
        }

        protected void ButtonGeneraExcepcion_Click(object sender, EventArgs e)
        {
            String Msj;
            try
            {
                throw new Exception("prueba!");
            }
            catch(Exception ex)
            {
                //msj al usuario
                Msj = "Ocurrió una excepción al procesar el registro de la información, por favor vuelva a intentar ó reporte a Sistemas.";
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                this.LabelInfoEx.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                
      

                //registra en una tabla o archivo informacion acerca de una excepcion
                BitacoraExcepcion BitacoraExcepcionAplictivo = new BitacoraExcepcion
                {
                    CadenaconexionBD = System.Configuration.ConfigurationManager.ConnectionStrings["cnArrendamientoInmueble"].ConnectionString,
                    //valores de contexto  de Excepcion a guardar
                    Aplicacion = "ContratosArrto",
                    Modulo = MethodInfo.GetCurrentMethod().DeclaringType.ToString() + ".aspx",
                    Funcion = MethodBase.GetCurrentMethod().Name + "()",
                    DescExcepcion = ex.InnerException == null ? ex.Message : ex.InnerException.Message,
                    Usr = Session["Contexto"] != null ? ((SSO)Session["Contexto"]).UserName.ToString(): "--", //el usuario del SSO
                };
                //persistir la informacion de la Excepcion
                BitacoraExcepcionAplictivo.RegistrarBitacoraExcepcion();
                BitacoraExcepcionAplictivo = null; //desocupar
            }
        }


    }
}