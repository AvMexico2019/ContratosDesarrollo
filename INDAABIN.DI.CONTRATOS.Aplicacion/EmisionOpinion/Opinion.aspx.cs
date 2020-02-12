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
//
using INDAABIN.DI.CONTRATOS.ModeloNegocios; //objetos Entities
using INDAABIN.DI.CONTRATOS.Negocio;//para uso del BUS

namespace INDAABIN.DI.CONTRATOS.Aplicacion.EmisionOpinion
{
    public partial class Opinion : System.Web.UI.Page
    {

        List<ConceptoEmisionOpinionDEMO> listCptosEmisionOpinion; //de prueba
        //List<ConceptoRespValor> ListCptosValorRespuesta;
        List<ConceptoOpinion> ListCptosOpinion;
        Button ButtonCtrl;

      


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
                if (this.PoblarDropDownTemasCptoValorResp())
                {
                    //remover el cpto de tabla SMOI
                    this.DropDownListTipoArrto.Items.Remove(this.DropDownListTipoArrto.Items.FindByText("Concepto SMOI"));
                    TableEmisionOpinion.Visible = false;
                    //inicalizar el ctrl y agregarlo a la PAGE
                    CrearCtrlConEventos();
                    this.LabelInfo.Text = "Seleccione una opcion de tipo de arrendamiento para la que  se desea crear una Emision de Opinion";
                }
            }
        }


        //creacion de ctrl
        private void CrearCtrlConEventos()
        {
            Button ButtonCtrl = new Button();
            ButtonCtrl.Text = "Comprobar";
            ButtonCtrl.Click += new EventHandler(ComprobarFolioSMOI);
            //ButtonCtrl.ControlStyle.CssClass = "";
            //this.Page.Controls.Add(ButtonCtrl);

            ContentPlaceHolder content = (ContentPlaceHolder)this.Master
                   .FindControl("cphBody");

            content.Controls.Add(ButtonCtrl);
        }

        //lledado de Combo Catalogo
        private Boolean PoblarDropDownTemasCptoValorResp()
        {
            Boolean Ok = false;
            List<TemaConcepto> ListTemaCptos = new List<TemaConcepto>();
            try
            {

                ListTemaCptos = new NG_Catalogos().ObtenerTemaCptos();
                this.DropDownListTipoArrto.DataSource = ListTemaCptos;
                this.DropDownListTipoArrto.DataValueField = "IdTema";
                this.DropDownListTipoArrto.DataTextField = "DescripcionTema";
                this.DropDownListTipoArrto.DataBind();

             
                Ok = true;
            }
            catch (Exception ex)
            {
                //msj al usuario
                this.LabelInfo.Text = ex.Message;
                            
            }
            return Ok;
        }
        
        private void MostrarMensajeJavaScript(string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert(\"" + mensaje + "\");", true);
        }

        protected void ButtonAgregar_Click(object sender, EventArgs e)
        {
           try
            {
                     /*
                    this.TableEmisionOpinion.Visible = true;
                    CargarCptosEmisionDemo();
                    CrearTablaCptosEmisionOpinionDemo();
                    CrearTablaPiePagina();
                    */

                    //ListCptosValorRespuesta = new NGConceptoRespValor().ObtenerCptosRespuestaValor(Convert.ToByte(this.DropDownListTipoArrto.SelectedValue));
                    ListCptosOpinion = new NGConceptoRespValor().ObtenerCptosEmisionOpinion(Convert.ToByte(this.DropDownListTipoArrto.SelectedValue));
                    //si existen CptosValorRespuesta, entonces exponerlos en la vista
                    if (ListCptosOpinion.Count > 0)
                    {
              
                       if( CrearTablaCptosEmisionOpinion())
                       {
                           this.CrearTablaPiePagina();
                           this.TableEmisionOpinion.Visible = true;
                           this.ButtonEnviarOpinion.Visible = true;
                       }
                    }
             }
             catch (Exception ex)
             {
                 this.LabelInfo.Text = ex.Message;
             }
        }


        private Boolean CrearTablaCptosEmisionOpinion()
        {

            TableRow Renglon;
            TableCell Columna;
            Boolean Ok = false;

            //para cada renglon de la lista agregarlo a la tabla 
            //para cada objeto en la lista, recorrer y accesar a sus propiedades
            foreach (ConceptoOpinion objCptoOpinion in ListCptosOpinion)
            {
                //agregar renglon a la tabla 
                Renglon = new TableRow();
                this.TableEmisionOpinion.Rows.Add(Renglon);


                //orden
                Columna = new TableCell();
                Columna.BorderStyle = BorderStyle.Solid;
                Columna.BorderWidth = 1;
                if (objCptoOpinion.IdRespuesta != 8) // 8=N/A es Tema, no poner el orden si es un concepto de Tema
                    //Columna.Text = objCptoOpinion.NumOrden.ToString(); 
                    Columna.Text = objCptoOpinion.NumOrdenVisual; 
                    
                else
                {
                    Columna.Font.Bold = true;
                    Columna.BorderStyle = BorderStyle.Solid;
                    Columna.BackColor = System.Drawing.Color.LightGray;
                    Columna.BorderColor = System.Drawing.Color.Gray;
                }

                //agregar la columna al renglon
                Renglon.Controls.Add(Columna);

                //cpto
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


                Columna = new TableCell();
                Columna.BorderStyle = BorderStyle.Solid;
                Columna.BorderWidth = 1;
                Columna.HorizontalAlign = HorizontalAlign.Center;

         //****************RESPUESTA******************************************
                
                //si el cpto tiene valor de opcion de respuesta, agregar la columna al renglon
                if (objCptoOpinion.ValorMinimo != null)
                {
                   
                  
                        //valor
                        DropDownList DropDownListRespuesta = new DropDownList();
                        DropDownListRespuesta.Items.Add(new ListItem("--", "-99"));
                        DropDownListRespuesta.Items.Add(new ListItem(objCptoOpinion.DescValorMinimo, objCptoOpinion.ValorMinimo.ToString()));
                        DropDownListRespuesta.Items.Add(new ListItem(objCptoOpinion.DescValorMaximo, objCptoOpinion.ValorMaximo.ToString()));
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
                            if (objCptoOpinion.IdConcepto == 35) //Folio de Opinion
                            {
                                //agregar caja de texto
                                TextBox TextBoxCtrl = new TextBox();
                                TextBoxCtrl.Width = 100;
                                
                                //Button ButtonCtrl = new Button();
                                //ButtonCtrl.Text = "Comprobar";
                                //ButtonCtrl.Click += new EventHandler(ComprobarFolioSMOI);
                                //ButtonCtrl.ControlStyle.CssClass = "";

                                //TextBoxCtrl.ReadOnly = true;
                                Columna.Controls.Add(TextBoxCtrl);
                                //this.CrearCtrlConEventos(Columna);
                                Columna.Controls.Add( this.ButtonCtrl);//se instancio en el load de la pagina, aqui solo se asocia y muestra en la tabla
                               
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

            }//foreach
            
            Ok = true;
           
            return Ok;

        }

        private void ComprobarFolioSMOI(object sender, EventArgs e)
        {
            this.LabelInfo.Text = "se activo";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "ButtonComprobarSMOI",
                                                "<script type = 'text/javascript'>alert('Button Clicked');</script>");
        }


        //poblado manual de objetos de negocio: demo
        private void CargarCptosEmisionDemo()
        {

            //Foma1: agregar objetos a la lista, en su inicializador
            listCptosEmisionOpinion = new  List<ConceptoEmisionOpinionDEMO>(){
                    //                new ConceptoEmisionOpinion{NumOrden=1, Concepto= "Se cuenta con el dictamen de disponibilidad de inmuebles federales",
                    //DescValorMinimo="No", ValorMinimo=0, DescValorMaximo="Si", ValorMaximo=10, EsDeterminante=true, new Respuesta(0, "No", 1, "10")},

                    //new ConceptoEmisionOpinion{NumOrdenLogico="0", Concepto= "Requerimientos para Arrendamiento Inmobiliario"},


                    new ConceptoEmisionOpinionDEMO{NumOrdenFisico=1, NumOrdenLogico="1", EsTemaCpto=false, Concepto= "Se cuenta con el dictamen de disponibilidad de inmuebles federales",
                    DescValorMinimo="No", ValorMinimo=0, DescValorMaximo="Si", ValorMaximo=10, EsDeterminante=true},
                    
                     new ConceptoEmisionOpinionDEMO{NumOrdenFisico=2, NumOrdenLogico="2",EsTemaCpto=false, Concepto= "Tiempo estimado que permanecerán arrendando el inmueble propuesto:",
                    DescValorMinimo=">2 años, No lo sé", ValorMinimo=0, DescValorMaximo="<=2 años", ValorMaximo=3, EsDeterminante=false, ValorRespuesta=1},
                    
                     new ConceptoEmisionOpinionDEMO{NumOrdenFisico=3,NumOrdenLogico="3.1",EsTemaCpto=false, Concepto= "Superficie de acuerdo a la tabla SMOI",  EsDeterminante=true,ValorRespuesta=1},
                     new ConceptoEmisionOpinionDEMO{NumOrdenFisico=4,NumOrdenLogico="3.2", EsTemaCpto=false,Concepto= "Superficie a arrendar",  EsDeterminante=true,ValorRespuesta=1},
                     new ConceptoEmisionOpinionDEMO{NumOrdenFisico=5,NumOrdenLogico="3.3", EsTemaCpto=false,Concepto= "En caso de incumplimiento, cuenta con el correspondiente Dictamen del INDAABIN, sobre aplicación de la Tabla SMOI:"
                      ,DescValorMinimo="No", ValorMinimo=0, DescValorMaximo="Si", ValorMaximo=10, EsDeterminante=true,ValorRespuesta=1},

                     new ConceptoEmisionOpinionDEMO{NumOrdenFisico=6,NumOrdenLogico="4", EsTemaCpto=false,Concepto= "El monto del arrendamiento propuesto es igual o inferior al establecido en el dictamen de justipreciación de renta tradicional o electrónica:"
                      ,DescValorMinimo="No", ValorMinimo=0, DescValorMaximo="Si",ValorMaximo=10, EsDeterminante=true,ValorRespuesta=1},
                     
                     new ConceptoEmisionOpinionDEMO{NumOrdenFisico=7,NumOrdenLogico="5", EsTemaCpto=false,Concepto= "El importe de renta del inmueble propuesto, rebasa el monto establecido en el Acuerdo por el que se fija el importe máximo de rentas por zonas y tipos de inmuebles:"
                      ,DescValorMinimo="No", ValorMinimo=0, DescValorMaximo="Si", ValorMaximo=10, EsDeterminante=true,ValorRespuesta=1},
                      
                      //Encabezado de preguntas         
                     new ConceptoEmisionOpinionDEMO{NumOrdenFisico=8,NumOrdenLogico="6", EsTemaCpto=false, Concepto= "Qué medidas de optimización, para el aprovechamiento de las actuales áreas, ha realizado:",  EsDeterminante=true},

                     new ConceptoEmisionOpinionDEMO{NumOrdenFisico=9,NumOrdenLogico="6.1",  EsTemaCpto=false,Concepto= "Redistribución de personal"
                      ,DescValorMinimo="No", ValorMinimo=0, DescValorMaximo="Si", ValorMaximo=1, EsDeterminante=true, ValorRespuesta=1},
                     
                        new ConceptoEmisionOpinionDEMO{NumOrdenFisico=10,NumOrdenLogico="6.2",  EsTemaCpto=false, Concepto= "Adecuaciones (rediseño) de espacios"
                      ,DescValorMinimo="No", ValorMinimo=0, DescValorMaximo="Si", ValorMaximo=1, EsDeterminante=true,ValorRespuesta=1},

                       new ConceptoEmisionOpinionDEMO{NumOrdenFisico=11,NumOrdenLogico="6.3",  EsTemaCpto=false, Concepto= "Cambio en el tipo de mobiliario (mobiliario modular, ligero, de utilización flexible y dimensiones no muy grandes)"
                      ,DescValorMinimo="No", ValorMinimo=0, DescValorMaximo="Si", ValorMaximo=1, EsDeterminante=true, ValorRespuesta=1},

                        new ConceptoEmisionOpinionDEMO{NumOrdenFisico=12,NumOrdenLogico="6.4",  EsTemaCpto=false, Concepto= "Optimización de espacios donde se almacenan los bienes"
                      ,DescValorMinimo="No", ValorMinimo=0, DescValorMaximo="Si", ValorMaximo=1, EsDeterminante=true,ValorRespuesta=1},

                       new ConceptoEmisionOpinionDEMO{NumOrdenFisico=13,NumOrdenLogico="6.5",  EsTemaCpto=false, Concepto= "Depuración de archivo"
                      ,DescValorMinimo="No", ValorMinimo=0, DescValorMaximo="Si", ValorMaximo=1, EsDeterminante=true,ValorRespuesta=1},
                      

                        new ConceptoEmisionOpinionDEMO{NumOrdenFisico=14,NumOrdenLogico="6.6",  EsTemaCpto=false, Concepto= "Disposición de bienes no útiles y/o de desecho (enajenación, donación, otros), para liberación de espacios"
                      ,DescValorMinimo="No", ValorMinimo=0, DescValorMaximo="Si", ValorMaximo=1, EsDeterminante=true,ValorRespuesta=1},

                        new ConceptoEmisionOpinionDEMO{NumOrdenFisico=15,NumOrdenLogico="6.7", EsTemaCpto=false,  Concepto= "Estrategias de uso del espacio, como son, puestos de trabajo no asignados y de ocupación rotativa"
                      ,DescValorMinimo="No", ValorMinimo=0, DescValorMaximo="Si", ValorMaximo=1, EsDeterminante=true,ValorRespuesta=1},
                      
                        new ConceptoEmisionOpinionDEMO{NumOrdenFisico=16,NumOrdenLogico="7",  EsTemaCpto=false, Concepto= "Se realizó el Análisis Costo-Beneficio, que manifieste ahorros económicos y en espacios físicos:"
                      ,DescValorMinimo="No", ValorMinimo=0, DescValorMaximo="Si", ValorMaximo=1, EsDeterminante=true,ValorRespuesta=1},

                      
                        new ConceptoEmisionOpinionDEMO{NumOrdenFisico=17,NumOrdenLogico="8", EsTemaCpto=false,  Concepto= "El inmueble propuesto para arrendamiento, cubre las necesidades de seguridad, higiene y funcionalidad para el público, los usuarios y el personal:"
                      ,DescValorMinimo="No", ValorMinimo=0, DescValorMaximo="Si", ValorMaximo=1, EsDeterminante=true,ValorRespuesta=1},

                         new ConceptoEmisionOpinionDEMO{NumOrdenFisico=18,NumOrdenLogico="9", EsTemaCpto=false,  Concepto= "El dictamen de justipreciación de renta o certificación electrónica se encuentra vigente:"
                      ,DescValorMinimo="No", ValorMinimo=0, DescValorMaximo="Si", ValorMaximo=1, EsDeterminante=true,ValorRespuesta=1},

                        new ConceptoEmisionOpinionDEMO{NumOrdenFisico=19,NumOrdenLogico="10",  EsTemaCpto=false, Concepto= "Se cuenta con la autorización del oficial mayor u homólogo"
                      ,DescValorMinimo="No", ValorMinimo=0, DescValorMaximo="Si", ValorMaximo=1, EsDeterminante=true,ValorRespuesta=1},
                    
                      new ConceptoEmisionOpinionDEMO{NumOrdenFisico=20,NumOrdenLogico="",EsTemaCpto=true, Concepto= "Implementación de buenas prácticas"},
                     
              //Encabezado de preguntas         
                     new ConceptoEmisionOpinionDEMO{NumOrdenFisico=21,NumOrdenLogico="1", EsTemaCpto=false, Concepto= "Qué medidas de optimización, para el aprovechamiento de las actuales áreas, ha realizado:",  EsDeterminante=true,ValorRespuesta=1},

                    new ConceptoEmisionOpinionDEMO{NumOrdenFisico=9,NumOrdenLogico="1.1",  EsTemaCpto=false,Concepto= "Norma Oficial Mexicana NOM-007-ENER-2014, Eficiencia energética para sistemas de alumbrado en edificios no residenciales."
                      ,DescValorMinimo="No", ValorMinimo=0, DescValorMaximo="Si", ValorMaximo=1, EsDeterminante=true,ValorRespuesta=1},
                    
                      new ConceptoEmisionOpinionDEMO{NumOrdenFisico=9,NumOrdenLogico="1.2",  EsTemaCpto=false,Concepto= "Norma Oficial Mexicana  NOM-008-ENER-2001, Eficiencia energética en edificaciones, envolvente de edificios no residenciales"
                      ,DescValorMinimo="No", ValorMinimo=0, DescValorMaximo="Si", ValorMaximo=1, EsDeterminante=true,ValorRespuesta=1},
                    
                    new ConceptoEmisionOpinionDEMO{NumOrdenFisico=9,NumOrdenLogico="1.3",  EsTemaCpto=false,Concepto= "Norma Oficial Mexicana NOM-025-STPS-2008, Condiciones de iluminación en los centros de trabajo."
                      ,DescValorMinimo="No", ValorMinimo=0, DescValorMaximo="Si", ValorMaximo=1, EsDeterminante=true,ValorRespuesta=1},


             };
        }


        private void CrearTablaCptosEmisionOpinionDemo()
        {

            TableRow Renglon;
            TableCell Columna;

            
            //agregar el renglon de encabezado
            Renglon = new TableRow();
            this.TableEmisionOpinion.Rows.Add(Renglon);

            //agregar columnas al renglon

            //col1: Orden
            Columna = new TableCell();
            Columna.Font.Bold = true;
            Columna.BorderStyle = BorderStyle.Solid;
            Columna.BorderWidth = 1;
            Columna.Width = 50;
            Columna.HorizontalAlign = HorizontalAlign.Center; 
            Columna.BackColor = System.Drawing.Color.LightGray;
            Columna.BorderColor = System.Drawing.Color.Gray;
            Columna.Text = "";
            Renglon.Controls.Add(Columna);

            //col2: Cpto
            Columna = new TableCell();
            Columna.Font.Bold = true;
            Columna.BorderStyle = BorderStyle.Solid;
            Columna.BorderWidth = 1;
            Columna.BackColor = System.Drawing.Color.LightGray;
            Columna.BorderColor = System.Drawing.Color.Gray;
            Columna.Text = "Requerimientos para Arrendamientos Inmobiliario";
            Renglon.Controls.Add(Columna);


            //col3: Valor
            Columna = new TableCell();
            Columna.Font.Bold = true;
            Columna.BorderStyle = BorderStyle.Solid;
            Columna.BackColor = System.Drawing.Color.LightGray;
            Columna.BorderColor = System.Drawing.Color.Gray;
            Columna.BorderWidth = 1;
            Columna.Text = "Captura";
            Renglon.Controls.Add(Columna);
                     
                      

            //para cada renglon de la lista agregarlo a la tabla 
            //para cada objeto en la lista, recorrer y accesar a sus propiedades
            foreach (ConceptoEmisionOpinionDEMO CptoOpinion in listCptosEmisionOpinion)
             {
                //agregar renglon a la tabla 
                 Renglon = new TableRow();   
                 this.TableEmisionOpinion.Rows.Add(Renglon);


                 //orden
                 Columna = new TableCell();
                 Columna.BorderStyle = BorderStyle.Solid;
                 Columna.BorderWidth = 1;
                 if (CptoOpinion.EsTemaCpto == false)
                    Columna.Text = CptoOpinion.NumOrdenLogico.ToString(); //recuperacion de datos del contrato
                 else
                 {
                     Columna.Font.Bold = true;
                     Columna.BorderStyle = BorderStyle.Solid;
                     Columna.BackColor = System.Drawing.Color.LightGray;
                     Columna.BorderColor = System.Drawing.Color.Gray;
                 }

                 //agregar la columna al renglon
                 Renglon.Controls.Add(Columna);

                 //cpto
                 Columna = new TableCell();
                 Columna.BorderStyle = BorderStyle.Solid;
                 Columna.BorderWidth = 1;
                 Columna.Text = CptoOpinion.Concepto; //recuperacion de datos del contrato
                 if (CptoOpinion.EsTemaCpto == true) //formatear la celda
                 {
                     Columna.Font.Bold = true;
                     Columna.BorderStyle = BorderStyle.Solid;
                     Columna.BackColor = System.Drawing.Color.LightGray;
                     Columna.BorderColor = System.Drawing.Color.Gray;
                 }
               
                //agregar la columna al renglon
                 Renglon.Controls.Add(Columna);


                 Columna = new TableCell();
                 Columna.BorderStyle = BorderStyle.Solid;
                 Columna.BorderWidth = 1;
                 Columna.HorizontalAlign = HorizontalAlign.Center;

                //si el cpto tiene valor de opcion de respuesta, agregar la columna al renglon
                 if (CptoOpinion.ValorMinimo != null)
                 {
                     //valor
                     DropDownList DropDownListRespuesta = new DropDownList();
                     DropDownListRespuesta.Items.Add(new ListItem("--", "-99"));
                     DropDownListRespuesta.Items.Add(new ListItem(CptoOpinion.DescValorMinimo, CptoOpinion.ValorMinimo.ToString()));
                     DropDownListRespuesta.Items.Add(new ListItem(CptoOpinion.DescValorMaximo, CptoOpinion.ValorMaximo.ToString()));
                     Columna.Controls.Add(DropDownListRespuesta);

                 }
                 else
                 {
                      if (CptoOpinion.EsTemaCpto == false)
                      {

                          if (CptoOpinion.ValorRespuesta!= null)
                          {
                                TextBox TextBoxCtrl = new TextBox();
                                TextBoxCtrl.Width = 100;
                                //TextBoxCtrl.ReadOnly = true;
                                Columna.Controls.Add(TextBoxCtrl);
                          }
                        

                        
                      }
                      else
                      {
                          Columna.Font.Bold = true;
                          Columna.BackColor = System.Drawing.Color.LightGray;
                          Columna.BorderColor = System.Drawing.Color.Gray;
                          Columna.Text = "Captura";

                      }
                     
                    
                 }
                      
                                  

                 //agregar la columna al renglon
                 Renglon.Controls.Add(Columna);

             }//foreach


            //Renglon de pie de pagina



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

        protected void DropDownListTipoArrto_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

      
    }
}