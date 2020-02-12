<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AcuseOtraFigura.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.Contrato.AcuseOtraFigura" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="https://framework-gb.cdn.gob.mx/assets/styles/main.css" rel="stylesheet"/>
    <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700,600,300' rel='stylesheet' type='text/css'/>
    <link href='https://framework-gb.cdn.gob.mx/favicon.ico' rel='shortcut icon'/>
    <style>
        /*zonas que no se imprimen*/
        @media print {
                    #ZonaNoImrpimible {display:none;}
                    }
           /*zonas que no se imprimen*/
            nav,aside  {
                        display: none;
                        }
        .auto-style1 {
            height: 119px;
        }
    </style>
   
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.4/jquery.min.js"></script>
    <script type="text/javascript">

        //para deshabilitar la opcion de volver a atras, del navegador, y no vuelva a dar guardar
        function deshabilitaRetroceso() {
            window.location.hash = "no-back-button";
            window.location.hash = "Again-No-back-button" //chrome
            window.onhashchange = function () { window.location.hash = "no-back-button"; }
        }        

        //funcion para cambiar el backgroud
        function CambiarLogo(dia, mes, ano) {
            debugger;
            var d = parseInt(dia);
            var m = parseInt(mes);
            var a = parseInt(ano);

            if (a >= 2018) {
                if (m >= 12) {
                    if (a >= 1) {
                        $("#LogoHeaderContrato").attr("src", "https://sistemas.indaabin.gob.mx/ImagenesComunes/SHCP-INDAABINREDUCIDO.PNG");
                        $("#background").css("background-image", "url(https://sistemas.indaabin.gob.mx/ImagenesComunes/nuevoescudo.png)");
                        $("#background").css("background-position", "left");
                        $("#form1").css("font-family", "Montserrat");
                        $('#EncabezadoViejo').hide();
                        $('#PIE').hide();
                    }
                    else {
                        $('#EncabezadoNuevo').hide();
                    }
                }
                else {
                    $('#EncabezadoNuevo').hide();
                }
            }
            else {
                $('#EncabezadoNuevo').hide();
            }

        }

        
      </script>
 

 </head>

<body onload="deshabilitaRetroceso()">
   
    <form id="form1" runat="server" style="font-size: small;##font##">

    <div id="ZonaNoImrpimible" style="text-align:center">
        <br />
        <button id="ButtonExportarPdf" class="btn btn-default" ToolTip="Da clic para exportar a PDF." type="button" runat="server" onserverclick="ButtonExportarPdf_Click" ><span class="glyphicon glyphicon-export"></span>  Exportar a PDF</button>
        <input type="button" id="ButtonImprimir" value="Imprimir" onclick="javascript: window.print()" class="btn" style="display:none;" />
        <br />
        <br />
        <asp:Label ID="LabelInfo" runat="server"></asp:Label>
        <br />
    </div>

    <table style="width: 100%">

         <tr id="EncabezadoNuevo" style="##Nuevo##">
                <td colspan="2">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <img src="https://sistemas.indaabin.gob.mx/ImagenesComunes/SHCP-LOGO-2018.png" alt="" style="height: 70px; width: 300px" /></td>
                            <td style="text-align: center;">Dirección General de Política y Gestión  Inmobiliaria<br />
                                Dirección de Planeación Inmobiliaria</td>
                            <td>
                                <img src="https://sistemas.indaabin.gob.mx/ImagenesComunes/LOGO-INDAABIN-2018.png" alt="" style="height: 70px; width: 250px" /></td>
                        </tr>
                    </table>
                </td>
            </tr>

            <tr id="EncabezadoViejo"  style="##Viejo##">
                <td class="auto-style1" style="width: 20%">
                    <img id="LogoHeaderContrato" src="http://sistemas.indaabin.gob.mx/ImagenesComunes/INDAABIN_01.jpg" alt="" style="height: 119px; width: 474px" /></td>
                <td style="text-align: right; width: 80%" class="auto-style1">Dirección General de Política y Gestión  Inmobiliaria <br />
                Dirección de Planeación Inmobiliaria
                <br />
                <asp:Label ID="LabelDeclaracionAnio" runat="server" Style="font-family:Arial;">2017, "Año del Centenario de la Promulgación de la Constitución Política de los Estados Unidos Mexicanos"</asp:Label>
            </td>
        </tr>
         <tr>
            <td></td>
            <td style="text-align: right">
                Folio:
                <asp:Label ID="LabelNoFolio" runat="server" Text="" style="font-weight: 700"></asp:Label></td>
        </tr>
    </table>  

   
        <div class="panel panel-default">
            <div class="panel-heading">
                <strong>ACUSE DE REGISTRO DE OTRA FIGURA DE OCUPACION</strong><br />
            </div>

            <div id="background" style="background-image: url(http://sistemas.indaabin.gob.mx/ImagenesComunes/aguila.png); background-repeat: no-repeat; background-size: contain; background-position: center; width: inherit; height: inherit;">
                <div style="text-align: right;">
                    <asp:Label ID="LabelFechaRegistro" runat="server" Text="Ciudad de México, a [01] de [Agosto] de [2016]."></asp:Label>
                    <br />
                    <br />
                </div>

                <div>
                    <br />
                    <div style="text-align: justify;">
                        <p style="text-align: justify">
                            <span>Con fundamento en lo dispuesto en los numerales 110 Bis, 144 y 146 del 'Acuerdo por el que se establecen las disposiciones en Materia de Recursos Materiales y Servicios Generales', publicado en el Diario Oficial de la Federación el día 5 de abril de 2016; y de acuerdo a la información capturada en el Sistema de Contratos de Arrendamiento y Otras Figuras de Ocupación de este Instituto, se emite el acuse de captura de Contrato de Otras figuras de ocupación, de la Institución Pública que usted representa; debiéndose observar la normatividad aplicable. </span>
                        </p>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <asp:Label ID="LabelInstitucion" runat="server" Text=""></asp:Label>
                                    <br />
                                    <asp:Label ID="LabelTipoContrato" runat="server"></asp:Label>
                                    <asp:Label ID="LabelTipoOcupacion" runat="server" Text="" Visible="false"></asp:Label>
                                    <br />
                                    <asp:Label ID="LabelTipoArrendamiento" runat="server"></asp:Label>
                                    <br />
                                    <asp:Label ID="LabelTipoContratacion" runat="server"> </asp:Label><br />
                                    <asp:Label ID="LabelFechaInicioOcupacion" runat="server" Text=""></asp:Label><br />
                                    <asp:Label ID="LabelFechaFinOcupacion" runat="server" Text=""></asp:Label>
                                    <br />
                                    <asp:Label ID="labelAreaOcupadaM2" runat="server" Text=""></asp:Label>
                                    <br />
                                    <asp:Label ID="LabelMontoPagoMensual" runat="server" Text=""></asp:Label>
                                    <br />
                                    <asp:Label ID="LabelFolioOpinion" runat="server"></asp:Label><br />
                                    <asp:Label ID="LabelFolioSAEF" runat="server"></asp:Label><br />
                                    <asp:Label ID="LabelSecuencialJust" runat="server"></asp:Label><br />
                                    <asp:Label ID="LabelPropietarioInmueble" runat="server" Text=""></asp:Label>
                                    <br />
                                    <asp:Label ID="LabelFuncionarioResponsable" runat="server" Text=""></asp:Label>
                                    <br />
                                    <asp:Label ID="LabelDirInmueble" runat="server" Text=""></asp:Label>
                                    <br />
                                    <asp:Label ID="LabelRIUF" runat="server" Text=""></asp:Label>
                                    <br />
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <br />
                    <br />
                </div>

                <table style="text-align: center; width: 100%; page-break-before: always;">
                    <tr>
                        <td style="word-break: break-all; text-align: left">
                            <asp:Label ID="LabelHoraRegistro" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="word-break: break-all;">
                            <strong>Cadena Original:</strong><br />
                            <asp:Label ID="LabelCadenaOriginal" runat="server"></asp:Label>
                            <br />
                            <br />
                        </td>

                         <td id="QRContrato" runat="server" style="width:30%; text-align:center;" rowspan="2">
                            <asp:Label ID="LabelQR" runat="server"></asp:Label>
                        </td>

                    </tr>
                    <tr>
                        <td style="word-break: break-all;">
                            <strong>Sello Digital:<br />
                            </strong>
                            <asp:Label ID="LabelSelloDigital" runat="server"></asp:Label>
                            <br />
                            <br />
                        </td>
                    </tr>

                      <tr>
                        <td style="text-align:left;" colspan="2">
                            <%--RCA 10/08/2018--%>
                            <%--FECHA DE AUTORIZACION--%>
                            <asp:Label ID="FechaAutorizacionContrato" runat="server"></asp:Label>
                            <br />
                            <br />
                            <br />
                        </td>
                    </tr>

                     <tr>
                        <td style="text-align: justify;" colspan="2">
                            <%--  RCA 10/08/2018
                                 LEYENDA DE QR--%>
                            <br />
                            <asp:Label ID="LeyendaContrato" runat="server"></asp:Label>
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                        </td>
                    </tr>

                    <tr id="PIE" style="##Viejo##">
                        <td colspan="2">
                            <span class="control-label">Avenida México Número 151, Colonia Del Carmen, C.P. 04100, Coyoacán, Ciudad de México Tel.: (55) 5563-2699  www.gob.mx/indaabin
                            </span>
                        </td>
                    </tr>
                </table>
            </div>
        </div>      
    </form>

</body>
</html>

