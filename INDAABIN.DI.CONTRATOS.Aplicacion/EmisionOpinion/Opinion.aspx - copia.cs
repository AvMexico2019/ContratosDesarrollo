using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//
using System.Configuration; //para accear al WebConfig
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
//
using INDAABIN.DI.CONTRATOS.ModeloNegocios; //objetos Entities
using INDAABIN.DI.CONTRATOS.Negocio;//para uso del BUS
using INDAABIN.DI.ModeloNegocio;

namespace INDAABIN.DI.CONTRATOS.Aplicacion.EmisionOpinion
{
    public partial class Opinion : System.Web.UI.Page
    {

       
    
        List<ConceptoOpinion> ListCptosOpinion;
        private static List<ValorRespuestaConcepto> ListValorRespuestaConcepto;
      
        protected void Page_Load(object sender, EventArgs e)
        {
         
            //El postback esta comentado deliveradamente porque la tabla dinamica no puede ligar el evento si no es en la carga de la pagina
            //if (!IsPostBack)
            //{

                if (Session["Contexto"] == null)
                    Response.Redirect(ConfigurationManager.AppSettings.Get("URL_SSO") + ConfigurationManager.AppSettings.Get("TokenApp").Replace("-", ""));

               //cargar la tabla de cptos de emision
                this.ObtenerCptosEmisionOpinion();
                this.LabelInfo.Text = "Proporcione los valores para cada concepto y al final de la captura de clic en Enviar para que el sistema registre la informacion y le proporcione un acuse con un número de Folio de Emisión de Opinión de Arrendamiento";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario    
            
               
           // }
        }

          
        
        private void MostrarMensajeJavaScript(string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert(\"" + mensaje + "\");", true);
        }

           
        private Boolean ObtenerCptosEmisionOpinion()
        {
            Boolean Ok = false;
            try
            {
                //obtener el Id del tema, para : Opinión Nuevo Arrendamiento y despues con el Id Obtener los Conceptos Valor-Resp
                byte IdTema = new NG_Catalogos().ObtenerIdTemaXDesc("Opinión Nuevo Arrendamiento");
                if (IdTema > 0)
                {
                    //ListCptosValorRespuesta = new NGConceptoRespValor().ObtenerCptosRespuestaValor(Convert.ToByte(this.DropDownListTipoArrto.SelectedValue));
                    ListCptosOpinion = new NGConceptoRespValor().ObtenerCptosEmisionOpinion(IdTema);
                    
                    //si existen CptosValorRespuesta, entonces exponerlos en la vista
                    if (ListCptosOpinion.Count > 0)
                     {

                        if (this.CrearTablaCptosEmisionOpinion())
                        {
                           this.CrearTablaPiePagina();
                           this.TableEmisionOpinion.Visible = true;
                           this.ButtonEnviarOpinion.Visible = true;
                        }
                    }
                }
               

            }
            catch (SqlException ex)
            {
                string msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                this.LabelInfo.Text = msj;
            }
            catch (Exception ex)
            {
                string msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                this.LabelInfo.Text = msj;
            }

            return Ok;
        }



        //se itera por cada objeto de la lista, y para objeto se crea un renglon de la tabla, donde sus propiedades se mapean a valores de celda
        private Boolean CrearTablaCptosEmisionOpinion()
        {
            Boolean Ok = false;
            TableRow Renglon;
            TableCell Columna;
            //recuperar conceptos de BD
            ListValorRespuestaConcepto = new  List<ValorRespuestaConcepto>();
               

            //para cada renglon de la lista agregarlo a la tabla 
            //para cada objeto en la lista, recorrer y accesar a sus propiedades
            foreach (ConceptoOpinion objCptoOpinion in ListCptosOpinion)
            {
                //agregar renglon a la tabla 
                Renglon = new TableRow();

                //Id de Control en la Tabla
                Renglon.ID = objCptoOpinion.NumOrdenVisual; //asiagnar ID a la columna para posteriormente recuperar en la obtencion de las respuestas
                this.TableEmisionOpinion.Rows.Add(Renglon);


                //************************* Columna de # de Orden *****************************
                Columna = new TableCell();
                Columna.BorderStyle = BorderStyle.Solid;
                Columna.BorderWidth = 1;

                if (objCptoOpinion.IdRespuesta != 8) 
                {
                    //Columna.Text = objCptoOpinion.NumOrden.ToString(); 
                    Columna.Text = objCptoOpinion.NumOrdenVisual;
                    //poblar objeto en donde se pondra la respuesta para guardar en la BD
                    ListValorRespuestaConcepto.Add(new ValorRespuestaConcepto { IdConceptoRespValor = objCptoOpinion.IdConceptoRespValor, NumOrden = objCptoOpinion.NumOrden, NumOrdenVisual = objCptoOpinion.NumOrdenVisual });
                }
                   

                else //es encabezado,  8=N/A es Tema
                {
                    Columna.Font.Bold = true;
                    Columna.BorderStyle = BorderStyle.Solid;
                    Columna.BackColor = System.Drawing.Color.LightGray;
                    Columna.BorderColor = System.Drawing.Color.Gray;
                }

                //agregar la columna al renglon
                Renglon.Controls.Add(Columna);


                //************************* Columna de Desc Cpto ***********************************/
                Columna = new TableCell();
                Columna.BorderStyle = BorderStyle.Solid;
                Columna.BorderWidth = 1;
                Columna.Text = objCptoOpinion.DescripcionConcepto; //recuperacion de datos del contrato
                if (objCptoOpinion.IdRespuesta == 8) //formatear la celda
                {
                    Columna.Font.Bold = true;
                    Columna.BorderStyle = BorderStyle.Solid;
                    Columna.BackColor = System.Drawing.Color.LightGray;
                    Columna.BorderColor = System.Drawing.Color.Gray;
                }

                //agregar la columna al renglon
                Renglon.Controls.Add(Columna);


                //****************columna RESPUESTA******************************************/
                Columna = new TableCell();
                Columna.BorderStyle = BorderStyle.Solid;
                Columna.BorderWidth = 1;
                Columna.HorizontalAlign = HorizontalAlign.Center;

              

                //si el cpto tiene valor de opcion de respuesta, agregar la columna al renglon
                if (objCptoOpinion.ValorMinimo != null)
                {

                    //valor
                    DropDownList DropDownListRespuesta = new DropDownList();
                    DropDownListRespuesta.Items.Add(new ListItem("--", "-99"));
                    DropDownListRespuesta.Items.Add(new ListItem(objCptoOpinion.DescValorMinimo, objCptoOpinion.ValorMinimo.ToString()));
                    DropDownListRespuesta.Items.Add(new ListItem(objCptoOpinion.DescValorMaximo, objCptoOpinion.ValorMaximo.ToString()));
                    
                    //Id Ctrl
                    DropDownListRespuesta.ID = Renglon.ID + "DropDownListRespuesta";

                    //DropDownListRespuesta.ID = objCptoOpinion.IdConcepto.ToString(); //identificador de control para buscarlo posteriormente
                    Columna.Controls.Add(DropDownListRespuesta);
                 

                    
                }
                else
                {

                    if (objCptoOpinion.IdRespuesta != 8) // N/A es Tema)
                    {

                        if (objCptoOpinion.ValorMinimo != null)
                        {
                            TextBox TextBoxCtrl = new TextBox();
                            TextBoxCtrl.Width = 100;
                            //TextBoxCtrl.ReadOnly = true;
                            Columna.Controls.Add(TextBoxCtrl);
                        }
                        else //IdRespuesta no es tema
                        {
                            if (objCptoOpinion.IdConcepto == 35) //Cpto: Folio de Opinion
                            {
                                //agregar caja de texto
                                TextBox TextBoxCtrl = new TextBox();
                                TextBoxCtrl.ID = "TextBoxFolioSMOI"; //identificador de control
                                TextBoxCtrl.Width = 100;
                                //TextBoxCtrl.ReadOnly = true;
                                Columna.Controls.Add(TextBoxCtrl);

                                //creacion de un boton
                                Button ButtonCtrl = new Button();
                                ButtonCtrl.Text = "Comprobar";
                                ButtonCtrl.Click += new EventHandler(this.ComprobarFolioSMOI);
                                ButtonCtrl.ControlStyle.CssClass = "";


                                //this.CrearCtrlConEventos(Columna);
                                Columna.Controls.Add(ButtonCtrl);//se instancio en el load de la pagina, aqui solo se asocia y muestra en la tabla

                            }
                            else
                            {
                               //poner etiquetas para cptos de smoi, que se recuperaran cuando se de clic en el boton de obtener el Folio de SMOI
                                if (objCptoOpinion.NumOrden == (decimal)3.02 ) //SupM2 resultado tabla SMOI
                                 {
                                     Label LabelValor = new Label();
                                     LabelValor.ID = "LabelSupM2TablaSMOI"; //darle ID, para localizar el control al proporcionar el usr el Folio de Tabla SMOI
                                     LabelValor.Text = "--";
                                     Columna.Controls.Add(LabelValor);
                                 }
                                else
                                {
                                    if ( objCptoOpinion.NumOrden == (decimal)3.0)
                                    {
                                        //agregar caja de texto
                                        TextBox TextBoxCtrl = new TextBox();
                                        TextBoxCtrl.ID = "TextBoxSupM2xArrendar"; //identificador de control
                                        TextBoxCtrl.Width = 100;
                                        Columna.Controls.Add(TextBoxCtrl);

                                      
                                    }
                                }
                               
                            }

                        }
                        
                    }
                    else //es Tema
                    {

                        Columna.Font.Bold = true;
                        Columna.BackColor = System.Drawing.Color.LightGray;
                        Columna.BorderColor = System.Drawing.Color.Gray;
                        Columna.Text = "Captura";

                    }

                                          

                }
                                   
                //agregar la columna al renglon
                Renglon.Controls.Add(Columna);


                //**** columna de Link de Funadamento Lega ***/
                Columna = new TableCell();
                Columna.BorderStyle = BorderStyle.Solid;
                Columna.BorderWidth = 1;
                if (objCptoOpinion.IdRespuesta != 8) // 8=N/A es Tema, no poner el orden si es un concepto de Tema
                {
                    Columna.BorderStyle = BorderStyle.Solid;
                    Columna.BorderWidth = 1;
                    Columna.HorizontalAlign = HorizontalAlign.Center;
                    LinkButton LinkButtonLegal = new LinkButton();
                    LinkButtonLegal.Text = "Consultar";
                    Columna.Controls.Add(LinkButtonLegal);
                    LinkButtonLegal.ID = objCptoOpinion.NumOrdenVisual + "-LinkButtom"; //darle un identificador
                                     
                    LinkButtonLegal.Click += new EventHandler(this.VerFundamentoLegalCpto);
                }
                else //es encabezado
                {
                    Columna.HorizontalAlign = HorizontalAlign.Center;
                    Columna.Text = "Fundamento Legal";
                    Columna.Font.Bold = true;
                    Columna.BorderStyle = BorderStyle.Solid;
                    Columna.BackColor = System.Drawing.Color.LightGray;
                    Columna.BorderColor = System.Drawing.Color.Gray;
                }

                //agregar la columna al renglon
                Renglon.Controls.Add(Columna);

                
                          

            }//foreach

            Ok = true;
            //guardar en session para cuando el usuario proporcione las respuestas
            Session["ListValorRespuestaConcepto"] = ListValorRespuestaConcepto;

            return Ok;

        }


       

        private string ConvirteNumLogicoAFisico(Decimal NumOrden)
        {
                        
            float numDecimal;

            
            //si su parte decimal es 0.0, exponer como entero
             numDecimal = float.Parse("0," + NumOrden.ToString().Split('.')[1]);

            if (numDecimal == 0.0)//no tiene parte decimal, entonces exponer commo un entero
                return int.Parse(NumOrden.ToString().Split('.')[0]).ToString();//obtener parte entera
            else //si tiene parte decimal >0, exponerla
            {
                //para casos de 3.01 exponer como 3.1, se hace en la implementacion, no en la BD, porque si no lo expone como 3.10, pareciendo que hay 10 items
                if (numDecimal > 1 && numDecimal < 10)
                {
                    //si de la 1er parte es cero, mostrar el siguiente
                    byte Num1Decimal = byte.Parse(numDecimal.ToString().Split()[0]);
                    return int.Parse(NumOrden.ToString().Split('.')[0]).ToString() + ".0" + Num1Decimal.ToString();
                }

                else
                    return NumOrden.ToString(); //expoener tal como esta en la BD
            }
        }

        //evento asociado dinamicamente al link de fundamento legal en la tabla
        //nota: se define en la creacion de la tabla, al load de la pagina
        private void VerFundamentoLegalCpto(object sender, EventArgs e)
        {

            LinkButton LinkButtonFundamento = (LinkButton)sender;
            if (LinkButtonFundamento != null)
            {
                ConceptoRespValor objConceptoRespValor= null;


                string NumOrdenCpto = LinkButtonFundamento.ID;
                string[] words = NumOrdenCpto.Split('-');

                NumOrdenCpto.IndexOf("-");
                NumOrdenCpto = words[0];

                string[] nums = NumOrdenCpto.Split('.');
                
                if (nums.Count() > 1)
                        //convertir a numero decimal correcto: 3.2 ---> 3.02
                        NumOrdenCpto = ConvirteNumLogicoAFisico(Convert.ToDecimal(NumOrdenCpto));

                try
                {
                  
                    objConceptoRespValor = new NGConceptoRespValor().ObtenerFundamentoLegalCpto(Convert.ToByte(2), Convert.ToDecimal(NumOrdenCpto));
                }
                catch (SqlException ex)
                {
                    string msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                    this.LabelInfo.Text = msj;
                }
                catch (Exception ex)
                {
                    string msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                    this.LabelInfo.Text = msj;
                }

                if (objConceptoRespValor != null)
                {
                    Session["objConceptoRespValor"] = objConceptoRespValor;
                    //string queryString = "FundamentoLegalCpto.aspx?NumPregunta=" + LinkButtonFundamento.ID;
                    string queryString = "FundamentoLegalCpto.aspx?NumOrdenCpto=" + NumOrdenCpto;
                    string newWin = "window.open('" + queryString + "');";
                    ClientScript.RegisterStartupScript(this.GetType(), "pop", newWin, true);
                }
                else
                    MostrarMensajeJavaScript("No existe fundamento Legal, para este Concepto");

            }//LinkButtonFundamento

         }

        //evento asociado dinamicamente al Button de la tabla
        //nota: se define en la creacion de la tabla, al load de la pagina
        private void ComprobarFolioSMOI(object sender, EventArgs e)
        {

            ValidarFolioSMOI();

        }


        /// <summary>
        /// Valida que la exista la entrada de un FolioSMOI vs la BD, a traves del llamado al metodo: RecuperaValoresFolioSMOI()
        /// </summary>
        /// <returns></returns>
        private Boolean ValidarFolioSMOI()
        {

            //recorrer los renglones-celdas y controles en celda hasta encontrar en ctrl por determinado ID
            foreach (TableRow row in TableEmisionOpinion.Rows)
            {
                foreach (TableCell cell in row.Cells)
                {
                    foreach (Control ctrl in cell.Controls)
                    {
                        //si el control que itera es 
                    
                        if (ctrl.ID == "TextBoxFolioSMOI")
                        {
                            //reseteo de etiquetas resultado de identificacion de Folio SMOI
                            this.AsignarValorIdCtrlRowCellCtrl("LabelSupM2TablaSMOI", "--"); //limpiar valores de etiquetas resultado de localizar el FolioSMOI
                          
                            //validar que existan datos en el textobox de supM2 x arrendar


                            this.AsignarValorIdCtrlRowCellCtrl("3.3DropDownListRespuesta", "--", true); //habilitarlo y resetear seleccion de valor

                            if (((TextBox)ctrl).Text.Length > 0)
                            {
                                if (Util.IsEnteroNatural(((TextBox)ctrl).Text))
                                {
                                 
                                    //TODO: buscar el FolioSMOI para obtener los 2 valores: superficieM2 de acuerdo a tabla SMOI y M2 por arrrendar
                             
                                    AplicacionConcepto objAplicacionConcepto = RecuperaValoresFolioSMOI(Convert.ToInt32(((TextBox)ctrl).Text));


                                    if (objAplicacionConcepto != null)//sustituir con la llamada al metodo que devuelve 2 valores, quizas sera un objeto, que  se poblaran las 2 propiedades
                                    {

                                          this.LabelInfo.Text = "Se identificó el registro del Folio de Tabla SMOI proporcionado";
                                          MostrarMensajeJavaScript(this.LabelInfo.Text);
                                          this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                          return true; //romper todos los ciclos, se asignaron correctamente los valores buscasdos
                                    }
                                    else
                                    {
                                        this.LabelInfo.Text = "No existe registro del Folio de Tabla SMOI proporcionado";
                                        this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                          MostrarMensajeJavaScript(this.LabelInfo.Text);
                                          return false; //romper todos los ciclos, se asignaron correctamente los valores buscasdos
                                    }

                                                                        

                                }
                                else
                                {
                                    this.LabelInfo.Text = "Debe proporcionar el Folio de tabla SMOI, en el concepto: [" + row.ID.ToString() + "] como un numero entero";
                                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                    ((TextBox)ctrl).Focus();
                                    MostrarMensajeJavaScript(this.LabelInfo.Text);
                                    return false; //romper todos los ciclos
                                }
                            }
                            else
                            {
                                this.LabelInfo.Text = "Debe proporcionar el Folio de tabla SMOI, en el concepto:  [" + row.ID.ToString() + "]";
                                this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                ((TextBox)ctrl).Focus();
                                MostrarMensajeJavaScript(this.LabelInfo.Text);
                                return false; //romper todos los ciclos
                            }


                        }//if
                           
                    }// foreach
                }// foreach
            }// foreach

            return false; //si llega aqui es que no se pudo reccorer la tabla y localizar valores, por ello se devuele falso
        }//funcion



        /// <summary>
        /// Busca un FolioSMOI en la BD y recupera 2 valores: (SupM2XArrendar y SupM2XSMOI) que coloca en un objeto de negocio: [AplicacionConcepto]
        /// </summary>
        /// <param name="FolioTablaSMOI"></param>
        /// <returns></returns>
        private AplicacionConcepto RecuperaValoresFolioSMOI(int FolioTablaSMOI )
        {
            if (FolioTablaSMOI > 0)
            {
                //TODO: Recuperar el FolioSMOI de la BD y vaciar los 2 valores a las propiedades del obj de negocio, para exponerlas al usuario
                AplicacionConcepto objAplicacionConcepto = new AplicacionConcepto { SupM2XArrendar = 1200.00m, SupM2XSMOI = 1500.00m };

                if (objAplicacionConcepto != null)//sustituir con la llamada al metodo que devuelve 2 valores, quizas sera un objeto, que  se poblaran las 2 propiedades
                {

                    //if (this.AsignarValorIdCtrlRowCellCtrl("TextBoxSupM2xArrendar", objAplicacionConcepto.SupM2XArrendar.ToString()))
                    //{

                         //colocar los valores recuperados del FolioSMOI en los controles de la vista
                        if (this.AsignarValorIdCtrlRowCellCtrl("LabelSupM2TablaSMOI", objAplicacionConcepto.SupM2XSMOI.ToString()))
                        {
                       

                            //autoseleccionar "--" de lista de Existe Dictamen cuando no se Excede Resultado de SMOI vs SupM2Arrendados
                            if (objAplicacionConcepto.SupM2XArrendar <  objAplicacionConcepto.SupM2XSMOI) //no se excede la norma recomendada
                            {
                                //buscar control en la tabla y asignar el valor especificado
                                this.AsignarValorIdCtrlRowCellCtrl("3.3DropDownListRespuesta", "--", false); //deshabilitar, no aplica este concepto

                            }


                        }
                         else
                        {
                            this.LabelInfo.Text = "Debe proporcionar un Folio SMOI valido para obtener los m2 resultado";
                            this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                            SetFocoIdCtrlRowCellCtrl("TextBoxFolioSMOI");
                            MostrarMensajeJavaScript(this.LabelInfo.Text);
                      
                           
                        }


                    //}
                    //else
                    //{
                    //    this.LabelInfo.Text = "Debe proporcionar la Superficie de m2 por arrendar";
                    //    SetFocoIdCtrlRowCellCtrl("TextBoxSupM2xArrendar");
                    //    MostrarMensajeJavaScript(this.LabelInfo.Text);
                        
                    //}

                }   
            
                return objAplicacionConcepto;
              }
            throw (new Exception("Debe propocionar un numero de folio smoi como un entero"));
        }



        //Proposito: Identificar el control solicitado por su Id en un determinado renglon-celda-control y "Asignar" el valor especificado en el parametro de entrada
        //parametros de entrada: el IdControl para el que se desea asignar un valor
        //parametros de salida: un true/false si se logro encontrar el control y hacer la asignacion del valor
        private Boolean AsignarValorIdCtrlRowCellCtrl(String IdCtrl, String Valor, Boolean? pEnabled=null)
        {
                //iterar por cada renglon de la tabla = concepto
            foreach (TableRow row in TableEmisionOpinion.Rows)
            {
                //iterar por cada columna/celda del renglon
                foreach (TableCell cell in row.Cells)
                {
                    //iterar por cada control dentro de la celda y validar contenido de acuerdo al tipo de ctrl.
                    foreach (Control ctrl in cell.Controls)
                    {

                        //la respuesta al concepto puede encontrarse en un control de tipo: TextBox, DropDownList o Label
                        switch (ctrl.GetType().ToString())
                        {
                            case "System.Web.UI.WebControls.Label":
                                if (((Label)ctrl).ID == IdCtrl)
                                {
                                    ((Label)ctrl).Text = Valor; //asignar valor al control solicitado
                                    return true; //romper todos los ciclos, porque ya se encontro el ctrl.
                                }

                                break;


                            case "System.Web.UI.WebControls.DropDownList":
                           
                                if (((DropDownList)ctrl).ID == IdCtrl)
                                {
                                    if (pEnabled != null)
                                          ((DropDownList)ctrl).Enabled = pEnabled.Value; //la opcion especificada                                   
                                    else //default
                                        ((DropDownList)ctrl).Enabled = true; //habilitar 

                                    if (Valor == "No")
                                    {
                                         ((DropDownList)ctrl).SelectedIndex = 1; //si
                                         //((DropDownList)ctrl).Items.FindByText(Valor).Selected = true; //autoseleccionar  valor al control solicitado
                                        return true; //romper todos los ciclos, porque ya se encontro el ctrl.
                                    }
                                    else
                                    {
                                         if (Valor == "Si")
                                         {
                                             ((DropDownList)ctrl).SelectedIndex = 2; //no
                                             //((DropDownList)ctrl).Items.FindByText(Valor).Selected = true; //autoseleccionar  valor al control solicitado
                                             return true; //romper todos los ciclos, porque ya se encontro el ctrl.
                                         }
                                         else
                                         {
                                             ((DropDownList)ctrl).SelectedIndex = 0; //"--"
                                             return true; //romper todos los ciclos, porque ya se encontro el ctrl.
                                         }
                                            
                                    }

                                   
                                }

                                break;
                        }
                    } //foreach
                }//foreach
            }//foreach

            return false; //si llego a este punto es que no se encontro el control por el Id solicitado en los paramteros de entrada
        }

        //Proposito: Identificar el control solicitado por su Id en un determinado renglon-celda-control y "Devolver" el valor especificado en el parametro de entrada
        //parametros de entrada: el IdControl para el que se desea asignar un valor
        //parametros de salida: un true/false si se logro encontrar el control y hacer la asignacion del valor
        private string EncontrarValorIdCtrlRowCellCtrl(String IdCtrl)
        {
            
            //iterar por cada renglon de la tabla = concepto
            foreach (TableRow row in TableEmisionOpinion.Rows)
            {
                //iterar por cada columna/celda del renglon
                foreach (TableCell cell in row.Cells)
                {
                    //iterar por cada control dentro de la celda y validar contenido de acuerdo al tipo de ctrl.
                    foreach (Control ctrl in cell.Controls)
                    {

                        //la respuesta al concepto puede encontrarse en un control de tipo: TextBox, DropDownList o Label
                        switch (ctrl.GetType().ToString())
                        {
                            case "System.Web.UI.WebControls.Label":
                                if (((Label)ctrl).ID == IdCtrl)                   
                                    return ((Label)ctrl).Text; //romper todos los ciclos, porque ya se encontro el ctrl. y se devuelve el valor
                                
                                break;


                            case "System.Web.UI.WebControls.DropDownList":

                                if (((DropDownList)ctrl).ID == IdCtrl)             
                                    return ((DropDownList)ctrl).SelectedItem.Text;  //romper todos los ciclos, porque ya se encontro el ctrl. y se devuelve el valor

                                break;

                            case "System.Web.UI.WebControls.TextBox":
                                if (((TextBox)ctrl).ID == IdCtrl)
                                    return ((TextBox)ctrl).Text; //romper todos los ciclos, porque ya se encontro el ctrl. y se devuelve el valor

                                break;

                        }
                    } //foreach
                }//foreach
            }//foreach

            return "Valor no Encontrado"; //si llego a este punto es que no se encontro el control por el Id solicitado en los paramteros de entrada
        }


        //Asigna el Foco a un control especificado, en la Tabla
        private Boolean SetFocoIdCtrlRowCellCtrl(String IdCtrl)
        {
            //iterar por cada renglon de la tabla = concepto
            foreach (TableRow row in TableEmisionOpinion.Rows)
            {
                //iterar por cada columna/celda del renglon
                foreach (TableCell cell in row.Cells)
                {
                    //iterar por cada control dentro de la celda y validar contenido de acuerdo al tipo de ctrl.
                    foreach (Control ctrl in cell.Controls)
                    {

                        //la respuesta al concepto puede encontrarse en un control de tipo: TextBox, DropDownList o Label
                        switch (ctrl.GetType().ToString())
                        {

                            case "System.Web.UI.WebControls.TextBox":
                                if (((TextBox)ctrl).ID == IdCtrl)
                                {
                                    ((TextBox)ctrl).Focus();
                                    return true; //romper todos los ciclos, porque ya se encontro el ctrl.
                                }

                                break;

                            case "System.Web.UI.WebControls.Label":
                                if (((Label)ctrl).ID == IdCtrl)
                                {
                                    ((Label)ctrl).Focus();
                                    return true; //romper todos los ciclos, porque ya se encontro el ctrl.
                                }

                                break;


                            case "System.Web.UI.WebControls.DropDownList":

                                if (((DropDownList)ctrl).ID == IdCtrl)
                                {
                                    ((DropDownList)ctrl).Focus(); 

                                }

                                break;
                        }
                    } //foreach
                }//foreach
            }//foreach

            return false; //si llego a este punto es que no se encontro el control por el Id solicitado en los paramteros de entrada
        }

        private void CrearTablaPiePagina()
        {

            TableRow Renglon;
            TableCell Columna;

            //renglon1
            Renglon = new TableRow();
            this.TablePiePagina.Rows.Add(Renglon);

            //agregar columnas al renglon1
            Columna = new TableCell();
            Columna.ColumnSpan = 4;
            Columna.Font.Bold = true;
            Columna.BorderStyle = BorderStyle.Solid;
            Columna.BorderWidth = 1;
            Columna.Width = 800;
            Columna.HorizontalAlign = HorizontalAlign.Center;
            Columna.BackColor = System.Drawing.Color.LightGray;
            Columna.BorderColor = System.Drawing.Color.Gray;
            Columna.Text = "La información capturada es responsabilidad del servidor público y  la Institución que la envía; el INDAABIN se reserva el derecho a solicitar información adicional y/o probatoria.";
            Renglon.Controls.Add(Columna);



            //renglon2
            Renglon = new TableRow();
            this.TablePiePagina.Rows.Add(Renglon);

            //agregar columnas al renglon1
            Columna = new TableCell();
            Columna.ColumnSpan = 4;
            Columna.BorderStyle = BorderStyle.Solid;
            Columna.BorderWidth = 1;
            Columna.Width = 800;
            Columna.HorizontalAlign = HorizontalAlign.Left;
            Columna.Text = "Nota: La presente evaluación no exime del cumplimiento de toda la normatividad que al respecto se publica en el Capítulo IX del MANUAL DE RECURSOS MATERIALES Y SERVICIOS GENERALES.";
            Renglon.Controls.Add(Columna);
        }

        
        
   
        protected void ButtonEnviarOpinion_Click(object sender, EventArgs e)
        {
          
         
            if (this.InsertRespuestaCptosEmisionOpinion() == false)
                this.LabelInfo.Text = "No fue posible registrar la solicitud de emision de opinion, vuelva a intentar o reporte al área correspondiente";


        }


        private Boolean InsertRespuestaCptosEmisionOpinion()
        {
            Boolean Ok = false;

            if (this.ValidarRespuestasCptos()) //validar entrada de datos requerida
            {
                
               // objCptoOpinion = new ConceptoOpinion(); //objeto de negocio para recolectar respuestas

                //lista de respuestas, pendientes de valor, recuperar cptos de la session, guardada en la presentacion de los cptos en la vista
                List<ValorRespuestaConcepto> ListValorRespuestaConcepto = (List<ValorRespuestaConcepto>)Session["ListValorRespuestaConcepto"];


                //Recolectar datos de respuesta  del usuario para cada cpto de acuerdo a su Id
                //Para cada respuesta colocarlo en el objeto de negocio
                //iterar por cada renglon de la tabla = concepto
                foreach (TableRow row in TableEmisionOpinion.Rows)
                {
                    if (Util.IsNumeric(row.ID))
                    {
                        //iterar por cada columna/celda del renglon
                        foreach (TableCell cell in row.Cells)
                        {

                            //iterar por cada control dentro de la celda y validar contenido de acuerdo al tipo de ctrl.
                            foreach (Control crtl in cell.Controls)
                            {


                                //la respuesta al concepto puede encontrarse en un control de tipo: TextBox, DropDownList o Label
                                switch (crtl.GetType().ToString())
                                {

                                    case "System.Web.UI.WebControls.DropDownList":

                                        //iterar por la lista de espera de Cpto-Respuesta para encontrar el Id y asignar respuesta desde la tabla de la vista
                                        foreach (ValorRespuestaConcepto objResp in ListValorRespuestaConcepto)
                                        {
                                            if (objResp.NumOrdenVisual == row.ID)
                                            {
                                                objResp.ValorResp =  Convert.ToInt32((((DropDownList)crtl).SelectedValue));
                                            }
                                        }
              
                                       break;


                                    case "System.Web.UI.WebControls.TextBox":
                                       foreach (ValorRespuestaConcepto objResp in ListValorRespuestaConcepto)
                                       {
                                           if (objResp.NumOrdenVisual == row.ID)
                                           {
                                               objResp.ValorResp = Convert.ToInt32((((TextBox)crtl).Text));
                                           }
                                       }
                                        
                                        break;

                                    case "System.Web.UI.WebControls.Label":
                                        foreach (ValorRespuestaConcepto objResp in ListValorRespuestaConcepto)
                                        {
                                            if (objResp.NumOrdenVisual == row.ID)
                                            {
                                                //se guardo a proposito primero en una variable de tipo Decimal, porque marcaba excepcion, quizas por algun formato interior en la cadena
                                                Decimal valor = Util.ToDecimal((((Label)crtl).Text));
                                                objResp.ValorResp = Convert.ToInt32(valor);

                                                //  objResp.ValorResp = Convert.ToInt32((((Label)crtl).Text));
                                            }
                                        }
                                        break;

                                }//switch

                            }//foreach
                        } //foreach
                    }//if ctrl Id

                }//foreach

          

                //llamar el objeto de capa de negocio para pasar los datos a la BD
              
                //Obtener valores: IdInstitucion y IdUsuarioRegistro del contexto del SSO de la cuenta del autenticado
                int IdInstitucionUsr = ((SSO)Session["Contexto"]).IdInstitucion.Value; //sso
                int IdUsuarioRegistro = ((SSO)Session["Contexto"]).IdUsuario.Value; //sso
                String IdCargoUsr = ((SSO)Session["Contexto"]).Cargo; //SSO
                

                string strConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["cnArrendamientoInmueble"].ConnectionString;

                //Armar DataTable con valores Cpto-Respuesta
                int IdConceptoRespValor, ValorResp;

                //definicion de la estructura de la tabla parametro
                System.Data.DataTable DataTableRespuestaCptoList = new System.Data.DataTable();
                DataTableRespuestaCptoList.Columns.Add("IdConceptoRespValor", typeof(int));
                DataTableRespuestaCptoList.Columns.Add("ValorResp", typeof(int));
               
                //pasar los datos de lista de negocio para vaciar al DataTable requerido por el SP
                foreach (ValorRespuestaConcepto objRespCpto in ListValorRespuestaConcepto)
                {

                    if (objRespCpto.ValorResp.HasValue) //solo cptos con respuesta, se guardan
                    {
                        IdConceptoRespValor = objRespCpto.IdConceptoRespValor;
                        ValorResp = objRespCpto.ValorResp.Value;
                        //agregar el renglon
                        DataTableRespuestaCptoList.Rows.Add(IdConceptoRespValor, ValorResp);
                    }
                  

                }

                ListValorRespuestaConcepto = null; // desocupar
                int FolioEmisionOpinion= 0; //parametro de salida

               //TODO: obtener del control de encabezado: el CP del inmueble y el pais, para pasar al sello digital

                //obtener controles del UserControl
                Control LabelDireccion = this.ctrlDireccionLectura.FindControl("LabelDireccion");
                string DireccionInmueble = ((Label)LabelDireccion).Text;

                //idInmueble para el que se realiza la emision de opinion
                Control LabelIdInmuebleArrto = this.ctrlDireccionLectura.FindControl("LabelIdInmuebleArrto");
                int IdInmuebleArrendamiento = Convert.ToInt32(((Label)LabelIdInmuebleArrto).Text);
                               
                Control LabelInstitucion = this.ctrlUsuarioInfo.FindControl("LabelInstitucion");
                string InstitucionUsr = ((Label)LabelInstitucion).Text;
                
                //crear sello digital
                string CadenaOriginal = "||Invocante:[" + InstitucionUsr + "] || Inmueble:[" + DireccionInmueble + "]||Fecha:[" + DateTime.Today.ToLongDateString() + "]||" + Guid.NewGuid().ToString();
                string SelloDigital = UtilContratosArrto.Encrypt(CadenaOriginal, true, "EmisionOpinion");



                try
                {
                    FolioEmisionOpinion = new NGConceptoRespValor().InsertEmisionOpinion("Nuevo", IdInstitucionUsr, IdCargoUsr, IdUsuarioRegistro, "Opinión Nuevo Arrendamiento", CadenaOriginal, SelloDigital, DataTableRespuestaCptoList, strConnectionString, IdInmuebleArrendamiento);
                    
                    if (FolioEmisionOpinion > 0)
                    {
                        Session["intFolioEmisionOpinion"] = FolioEmisionOpinion.ToString();
                        this.LabelInfo.Text = "Se registraron las respuestas de la Emisión de Opinión para el Nuevo Arrendamiento, su folio es: [" + FolioEmisionOpinion.ToString() + "]";
                        this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                        DataTableRespuestaCptoList = null; //desocupar
                        Response.Redirect("../EmisionOpinion/AcuseEmisionOpinion.aspx", false); //ojo: debe de ser false, si no truena
                    

                    }
                    else
                    {
                        this.LabelInfo.Text = "No fue posible registrar las respuestas de la Emisión de Opinión para el Nuevo Arrendamiento, vuelva a intentar el envío ó reporte al área de Sistemas";
                        this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                        this.MostrarMensajeJavaScript(this.LabelInfo.Text);
                        DataTableRespuestaCptoList = null; //desocupar
                    }
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    string msj = ex.InnerException == null ? "" : ex.InnerException.Message;
                    this.LabelInfo.Text = msj;
                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                    this.MostrarMensajeJavaScript(this.LabelInfo.Text);

                    //TODO: implementar registro de Bitacora de Excepciones
                }
                catch(Exception ex)
                {
                    string msj = ex.InnerException == null ? "" : ex.InnerException.Message;
                    this.LabelInfo.Text = msj;
                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                    this.MostrarMensajeJavaScript(this.LabelInfo.Text);
                }
                
                



            }//if validacion de ctrl de entrada

            return Ok;
           
        }



        //Validar  datos de respuesta  del usuario para cada cpto de acuerdo a su Id
      
        private Boolean ValidarRespuestasCptos()
        {
            Boolean Ok = true;

            if (TableEmisionOpinion.Rows.Count == 0)
                Ok = false;

           //iterar por cada renglon de la tabla = concepto
            foreach (TableRow row in TableEmisionOpinion.Rows)
            {
                if (Util.IsNumeric(row.ID))
                {
                    //iterar por cada columna/celda del renglon
                    foreach (TableCell cell in row.Cells)
                    {
                    
                        //iterar por cada control dentro de la celda y validar contenido de acuerdo al tipo de ctrl.
                        foreach (Control ctrl in cell.Controls)
                        {


                                //la respuesta al concepto puede encontrarse en un control de tipo: TextBox, DropDownList o Label
                                switch (ctrl.GetType().ToString())
                                {
                                
                                    case "System.Web.UI.WebControls.DropDownList":
                                        //validar que si el cpto: 3.3 esta como "--" y esta habilitado, se seleccione una valor Si ó No, necesariamente
                                        //el concepto 3.3 se permite la entrada "--" cuando el valor de Supm2SMOI <= SupRentada
                                        if (ctrl.ID == "3.3DropDownListRespuesta")
                                        {
                                            ////obtener el valor del coontrol: TextBoxFolioSMOI, para pasarlo a la funcion de recuperacion de FolioSMOI (sig. instruccion)
                                            //String sFolioSMOI = EncontrarValorIdCtrlRowCellCtrl("TextBoxFolioSMOI");

                                            ////independientemente que el usuario explicitamente dio clic, volver a ejecutar
                                            //this.RecuperaValoresFolioSMOI(Convert.ToInt32(sFolioSMOI));

                                            if (((DropDownList)ctrl).Enabled) //si esta habilitado, se debe seleccionar un valor
                                            {
                                                if (((DropDownList)ctrl).SelectedItem.Text == "--")
                                                {
                                                    this.LabelInfo.Text = "Debe seleccionar una opcion en la lista del concepto: [" + row.ID.ToString() + "]";
                                                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                                    ((DropDownList)ctrl).Focus();
                                                    this.MostrarMensajeJavaScript(this.LabelInfo.Text);
                                                    return false; //romper todos los ciclos
                                                }
                                                
                                            }

                                        }
                                        else //es otro dropdownlist
                                        {

                                            if (((DropDownList)ctrl).SelectedItem.Text == "--")
                                            {
                                                this.LabelInfo.Text = "Debe seleccionar un valor de respuesta para el concepto: [" + row.ID.ToString() + "]";
                                                this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                                ((DropDownList)ctrl).Focus();
                                                this.MostrarMensajeJavaScript(this.LabelInfo.Text);
                                                return false; //romper todos los ciclos

                                            }
                                        }
                                        break;


                                    case "System.Web.UI.WebControls.TextBox":
                                        if (ctrl.ID == "TextBoxFolioSMOI")
                                        {
                                            if (((TextBox)ctrl).Text.Length > 0)
                                            {
                                                if ((Util.IsEnteroNatural(((TextBox)ctrl).Text)) == false)
                                                {
                                                    this.LabelInfo.Text = "Debe proporcionar el Folio de tabla SMOI [" + row.ID.ToString() + "]";
                                                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                                    ((TextBox)ctrl).Focus();

                                                    this.MostrarMensajeJavaScript(this.LabelInfo.Text);
                                                    return false; //romper todos los ciclos
                                                }
                                                else //si se proporciono el folio de smoi, pero volver a validar por si el usuario cambia el texto y no le da comprobar
                                                   this.RecuperaValoresFolioSMOI(Convert.ToInt32(((TextBox)ctrl).Text));
                                                
                                            }
                                            else //no hay contenido en el ctrl
                                            {
                                                this.LabelInfo.Text = "Debe proporcionar el Folio de tabla SMOI [" + row.ID.ToString() + "]";
                                                this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                                ((TextBox)ctrl).Focus();
                                                this.MostrarMensajeJavaScript(this.LabelInfo.Text);
                                                return false; //romper todos los ciclos
                                            }

                                         }
                                        else
                                        {
                                            if (ctrl.ID == "TextBoxSupM2xArrendar")
                                            {
                                                if (((TextBox)ctrl).Text.Length > 0)
                                                {
                                                    if ((Util.IsEnteroNatural(((TextBox)ctrl).Text)) == false)
                                                    {
                                                        this.LabelInfo.Text = "Debe proporcionar los metros cuadrados que se arrendan [" + row.ID.ToString() + "]";
                                                        this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                                        ((TextBox)ctrl).Focus();

                                                        this.MostrarMensajeJavaScript(this.LabelInfo.Text);
                                                        return false; //romper todos los ciclos
                                                    }
                                                    else //si se proporciono el folio de smoi, pero volver a validar por si el usuario cambia el texto y no le da comprobar
                                                    {
                                                        this.RecuperaValoresFolioSMOI(Convert.ToInt32(((TextBox)ctrl).Text));
                                                    }
                                                }
                                                else //no hay contenido en el ctrl
                                                {
                                                    this.LabelInfo.Text = "Debe proporcionar los metros cuadrados que se arrendan [" + row.ID.ToString() + "]";
                                                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                                    ((TextBox)ctrl).Focus();
                                                    this.MostrarMensajeJavaScript(this.LabelInfo.Text);
                                                    return false; //romper todos los ciclos
                                                }
                                            }
                                        }
                                   
                                        break;

                                    case "System.Web.UI.WebControls.Label":
                                        if (((Label)ctrl).Text == "--" )
                                        {
                                           
                                            if (ctrl.ID == "LabelSupM2TablaSMOI") //personalizar msj de excepcion al usuario para cuando  se trate de estas ctrol de etiquetas
                                            
                                                this.LabelInfo.Text = "Debe proporcionar un numero de Folio de Tabla SMOI y hacer clic en Comprobar ";
                                                                                                
                                            else
                                            
                                                this.LabelInfo.Text = "Debe seleccionar un valor de respuesta para el concepto: " + row.ID.ToString();


                                            this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                            this.MostrarMensajeJavaScript(this.LabelInfo.Text);
                                            return false; //romper todos los ciclos
                                        }
                                  
                                        break;

                                }//switch

                            }//foreach
                         } //foreach
                    }//if ctrl Id
              
            }//foreach

            return Ok;

        }//funcion

  

    }
}