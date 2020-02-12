<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AcuseSMOI.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.TablaSMOI.AcuseSMOI" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="https://framework-gb.cdn.gob.mx/assets/styles/main.css" rel="stylesheet" />
    <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700,600,300' rel='stylesheet' type='text/css' />
    <link href='https://framework-gb.cdn.gob.mx/favicon.ico' rel='shortcut icon' />
    <style>
        @media print {
            #ZonaNoImrpimible {
                display: none;
            }
        }

        nav, aside {
            display: none;
        }

        .auto-style1 {
            height: 119px;
        }

        .auto-style2 {
            height: 55px;
        }

        .auto-style3 {
            height: 52px;
        }
    </style>

    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.4.4/jquery.min.js"></script>
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
                        $("#LogoHeader").attr("src", "https://sistemas.indaabin.gob.mx/ImagenesComunes/logozapata.jpg");
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

    <form id="form1" runat="server" style="font-size: 16px; ##font##">

        <div id="ZonaNoImrpimible" style="text-align: center">
            <br />
            <button id="ButtonExportarPdf" class="btn btn-default" tooltip="Da clic para exportar a PDF." type="button" runat="server" onserverclick="ButtonExportarPdf_Click"><span class="glyphicon glyphicon-export"></span>Exportar a PDF</button>
            <input type="button" id="ButtonImprimir" value="Imprimir" onclick="javascript: window.print()" class="btn" style="display: none;" />
            <br />
            <asp:Label ID="LabelInfo" runat="server"></asp:Label>
        </div>

        <table id="SMOI" style="width: 100%">

            <tr id="EncabezadoNuevo" style="##nuevo##">
                <td class="auto-style1" style="width: 20%">
                    <img src="https://sistemas.indaabin.gob.mx/ImagenesComunes/logozapata.jpg" alt="" style="height: 60px; width: 550px" />
                </td>
                <td style="text-align: right; width: 80%" class="auto-style1">Dirección General de Política y Gestión  Inmobiliaria
                    <br />
                    Dirección de Planeación Inmobiliaria
                    <br />
                </td>
                <%-- <td colspan="2">
                    <table style="width: 100%;">                     
                         <tr>
                            <td>
                                <img src="https://sistemas.indaabin.gob.mx/ImagenesComunes/SHCP-LOGO-2019.png" alt="" style="height: 70px; width: 300px" /></td>
                            <td style="text-align: center;">Dirección General de Política y Gestión  Inmobiliaria<br />
                                Dirección de Planeación Inmobiliaria</td>
                            <td>
                                <img src="https://sistemas.indaabin.gob.mx/ImagenesComunes/LOGO-INDAABIN-2019.png" alt="" style="height: 70px; width: 250px" /></td>
                        </tr>
                    </table>
                </td>--%>
            </tr>



            <tr id="EncabezadoViejo" style="##Viejo##">
                <td class="auto-style1" style="width: 20%">
                    <img id="LogoHeader" src="https://sistemas.indaabin.gob.mx/ImagenesComunes/Hacienda-Indaabin-Zapata-2019.png" alt="" style="height: 80px; width: 550px" />
                </td>
                <td style="text-align: right; width: 80%" class="auto-style1">Dirección General de Política y Gestión  Inmobiliaria
                    <br />
                    Dirección de Planeación Inmobiliaria
                    <br />
                    <asp:Label ID="LabelDeclaracionAnio" runat="server" Style="font-family: Arial;">2017, "Año del Centenario de la Promulgación de la Constitución Política de los Estados Unidos Mexicanos"</asp:Label>
                </td>
            </tr>
            <tr>
                <td></td>
                <td style="text-align: right">Folio:
                <asp:Label ID="LabelNoFolio" runat="server" Text="" Style="font-weight: 700"></asp:Label></td>
            </tr>
        </table>
        <div style="font-size: 16px">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>ACUSE DE REGISTRO DE TABLA DE SUPERFICIE MÁXIMA A OCUPAR POR INSTITUCIÓN (SMOI)</strong><br />
                </div>

                <div style="text-align: right;">
                    <asp:Label ID="LabelFechaRegistro" runat="server" Text="Ciudad de México, a [01] de [Agosto] de [2016]."></asp:Label>
                    <br />
                </div>

                <div style="text-align: justify;">

                    <br />
                    <br />
                    <br />
                    <p style="text-align: justify">
                        <span>Con fundamento en lo dispuesto en los numerales 110 Bis, 144 y 146 del 'Acuerdo por el que se establecen las disposiciones en Materia de Recursos Materiales y Servicios Generales', publicado en el Diario Oficial de la Federación el día 5 de abril de 2016; y de acuerdo a la información capturada en el Sistema de Contratos de Arrendamiento y Otras Figuras de Ocupación de este Instituto, se emite el acuse de captura de la Tabla SMOI, de la Institución Pública que usted representa; debiéndose observar la normatividad aplicable.
                        </span>
                    </p>
                    <strong>Institución Pública</strong>: 
                        <asp:Label ID="LabelInstitucion" runat="server" Text=""></asp:Label>
                    <br />
                    <br />

                    <p>
                        Factor X = Superficie Máxima a Ocupar por Todos los Niveles: <strong>
                            <asp:Label runat="server" ID="factxLbl"></asp:Label>
                            m2</strong><br />
                        Factor Y = Áreas de Uso Común y Áreas de Circulación (FactorX*0.44): <strong>
                            <asp:Label runat="server" ID="factyLbl"></asp:Label>
                            m2</strong><br />
                        Factor Z = Áreas Complementarias: <strong>
                            <asp:Label runat="server" ID="factzLbl"></asp:Label>
                            m2</strong><br />
                        SOMI = Superficie Máxima a Ocupar por la Institucion: <strong>
                            <asp:Label runat="server" ID="lblSmoi"></asp:Label>
                            m2</strong><br />
                    </p>

                    <asp:Label ID="LabelIncumplimiento" runat="server"></asp:Label>
                    <br />
                    <br />
                    <br />
                    Nota: El presente registro de Tabla SMOI, en caso de no ser utilizado en un periodo de seis meses, a partir de la fecha de captura, será inhabilitada del Sistema de Contratos de Arrendamiento y Otras Figuras de Ocupación.
                        <br />
                    <br />
                    <asp:Label ID="LabelHoraRegistro" runat="server"></asp:Label>

                    <table style="text-align: center; width: 100%;">
                        <tr>
                            <td style="word-break: break-all;" class="auto-style2">
                                <strong>Cadena Original:</strong><br />
                                <asp:Label ID="LabelCadenaOriginal" runat="server"></asp:Label>
                                <br />
                                <br />
                            </td>

                            <td id="QRSMOI" runat="server" style="width: 30%; text-align: center;" rowspan="2">
                                <asp:Label ID="LabelQR" runat="server"></asp:Label>
                            </td>

                        </tr>
                        <tr>
                            <td style="word-break: break-all;" class="auto-style3">
                                <strong>Sello Digital:<br />
                                </strong>
                                <asp:Label ID="LabelSelloDigital" runat="server"></asp:Label>
                                <br />
                                <br />
                                <br />
                            </td>

                        </tr>

                        <tr>
                            <td style="text-align: left;" colspan="2">

                                <asp:Label ID="FechaAutorizacionSMOI" runat="server"></asp:Label>
                                <br />
                                <br />
                                <br />
                            </td>
                        </tr>

                        <tr>
                            <td style="text-align: justify;" colspan="2">

                                <br />
                                <asp:Label ID="LeyendaSMOI" runat="server"></asp:Label>
                                <br />

                            </td>
                        </tr>

                    </table>                                                                       
                </div>                
            </div>

             <br />
                    
                    
                    <br />
                    <br />
            <br />
                    <br />
                    <br />
                    <br />

            <table>
                        <tr style="##viejo##">
                            <td style="width: 100%">
                                <img src="https://sistemas.indaabin.gob.mx/ImagenesComunes/piePagina.png" alt="" style="height: 64px; width: 1000px" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />

                    <br />
                    <br />
                    <br />

            <%--<div id="background" style="background-image: url(https://sistemas.indaabin.gob.mx/ImagenesComunes/nuevoescudo.png); background-repeat: no-repeat; background-size: contain; background-position: center; width: inherit; height: inherit;">--%>
            <div id="background">

                <%--<div>--%>

                <div style="text-align: justify;">                

                    <table style="width: 100%">
                        <tr id="EncabezadoNuevo2" style="##nuevo##">
                            <td colspan="2">
                                <table style="width: 100%;">
                                    <tr>
                                        <td class="auto-style1" style="width: 20%">
                                            <img src="https://sistemas.indaabin.gob.mx/ImagenesComunes/logozapata.jpg" alt="" style="height: 60px; width: 550px" />
                                        </td>
                                        <td style="text-align: right; width: 80%" class="auto-style1">Dirección General de Política y Gestión  Inmobiliaria
                                            <br />
                                            Dirección de Planeación Inmobiliaria
                                              <br />
                                            <%-- <asp:Label ID="Label1" runat="server" Style="font-family: Arial;">2017, "Año del Centenario de la Promulgación de la Constitución Política de los Estados Unidos Mexicanos"</asp:Label>--%>
                                        </td>
                                    </tr>
                                    <%--   <tr>
                                        <td>
                                            <img src="https://sistemas.indaabin.gob.mx/ImagenesComunes/SHCP-LOGO-2019.png" alt="" style="height: 70px; width: 300px" /></td>
                                        <td style="text-align: center;">Dirección General de Política y Gestión  Inmobiliaria<br />
                                            Dirección de Planeación Inmobiliaria</td>
                                        <td>
                                            <img src="https://sistemas.indaabin.gob.mx/ImagenesComunes/LOGO-INDAABIN-2019.png" alt="" style="height: 70px; width: 250px" /></td>
                                    </tr>--%>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <br />

                    <%--<div style="background-image: url(https://sistemas.indaabin.gob.mx/ImagenesComunes/nuevoescudo.png); background-repeat: no-repeat; background-size: contain; background-position: center; width: inherit; height: inherit;">--%>
                    <div style="font-size: 16px">

                        <asp:Table runat="server" ID="tblX" Width="100%" class="table table-bordered">
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell ColumnSpan="5" Font-Bold="true" BorderStyle="Solid" BorderWidth="1" HorizontalAlign="Left" BackColor="LightGray" BorderColor="Gray">Superficie Máxima a Ocupar por Todos los Niveles [Factor X]</asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell BackColor="LightGray" BorderColor="Gray">&nbsp;</asp:TableHeaderCell>
                                <asp:TableHeaderCell Font-Bold="true" BorderStyle="Solid" BorderWidth="1" HorizontalAlign="Left" BackColor="LightGray" BorderColor="Gray">Grupo Jerárquico (Clave)</asp:TableHeaderCell>
                                <asp:TableHeaderCell Font-Bold="true" BorderStyle="Solid" BorderWidth="1" HorizontalAlign="Left" BackColor="LightGray" BorderColor="Gray">Superficie Unitaria Máxima por Servidor Público (m2) [A]</asp:TableHeaderCell>
                                <asp:TableHeaderCell Font-Bold="true" BorderStyle="Solid" BorderWidth="1" HorizontalAlign="Left" BackColor="LightGray" BorderColor="Gray">Número de Servidores Públicos [B]</asp:TableHeaderCell>
                                <asp:TableHeaderCell Font-Bold="true" BorderStyle="Solid" BorderWidth="1" HorizontalAlign="Left" BackColor="LightGray" BorderColor="Gray">Superficie Máxima de Ocupación por Nivel (m2) [C]=[AxB]</asp:TableHeaderCell>
                            </asp:TableHeaderRow>

                        </asp:Table>

                        <asp:Table runat="server" ID="tblZ" Width="100%" class="table table-bordered">
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell ColumnSpan="5" Font-Bold="true" BorderStyle="Solid" BorderWidth="1" HorizontalAlign="Left" BackColor="LightGray" BorderColor="Gray">Áreas Complementarias [Factor Z]</asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell BackColor="LightGray" BorderColor="Gray">&nbsp;</asp:TableHeaderCell>
                                <asp:TableHeaderCell Font-Bold="true" BorderStyle="Solid" BorderWidth="1" HorizontalAlign="Left" BackColor="LightGray" BorderColor="Gray">Tipo de Espacio</asp:TableHeaderCell>
                                <asp:TableHeaderCell Font-Bold="true" BorderStyle="Solid" BorderWidth="1" HorizontalAlign="Left" BackColor="LightGray" BorderColor="Gray">Factor de m2 x usuario</asp:TableHeaderCell>
                                <asp:TableHeaderCell Font-Bold="true" BorderStyle="Solid" BorderWidth="1" HorizontalAlign="Left" BackColor="LightGray" BorderColor="Gray">Número estimado de usuarios</asp:TableHeaderCell>
                                <asp:TableHeaderCell Font-Bold="true" BorderStyle="Solid" BorderWidth="1" HorizontalAlign="Left" BackColor="LightGray" BorderColor="Gray">Total en m2</asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                        </asp:Table>
                         <br />
                    <br />
                    <br />
                    <table>
                        <tr style="##viejo##">
                            <td style="width: 100%">
                                <img src="https://sistemas.indaabin.gob.mx/ImagenesComunes/piePagina.png" alt="" style="height: 64px; width: 1000px" />
                            </td>
                        </tr>
                    </table>

                         <br />
                    <br />
                    <br />
                         <br />
                    <br />
                    <br />

                        <table style="width: 100%">
                        <tr id="EncabezadoNuevo3" style="##nuevo##">
                            <td colspan="2">
                                <table style="width: 100%;">
                                    <tr>
                                        <td class="auto-style1" style="width: 20%">
                                            <img src="https://sistemas.indaabin.gob.mx/ImagenesComunes/logozapata.jpg" alt="" style="height: 60px; width: 550px" />
                                        </td>
                                        <td style="text-align: right; width: 80%" class="auto-style1">Dirección General de Política y Gestión  Inmobiliaria
                                            <br />
                                            Dirección de Planeación Inmobiliaria
                                              <br />
                                            <%-- <asp:Label ID="Label1" runat="server" Style="font-family: Arial;">2017, "Año del Centenario de la Promulgación de la Constitución Política de los Estados Unidos Mexicanos"</asp:Label>--%>
                                        </td>
                                    </tr>
                                    <%--   <tr>
                                        <td>
                                            <img src="https://sistemas.indaabin.gob.mx/ImagenesComunes/SHCP-LOGO-2019.png" alt="" style="height: 70px; width: 300px" /></td>
                                        <td style="text-align: center;">Dirección General de Política y Gestión  Inmobiliaria<br />
                                            Dirección de Planeación Inmobiliaria</td>
                                        <td>
                                            <img src="https://sistemas.indaabin.gob.mx/ImagenesComunes/LOGO-INDAABIN-2019.png" alt="" style="height: 70px; width: 250px" /></td>
                                    </tr>--%>
                                </table>
                            </td>
                        </tr>
                    </table>

                        <asp:Table runat="server" Width="100%" class="table table-bordered">
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell ColumnSpan="4" Font-Bold="true" BorderStyle="Solid" BorderWidth="1" HorizontalAlign="Center" BackColor="LightGray" BorderColor="Gray">Resultados por factor</asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell ColumnSpan="4" Font-Bold="true" BorderStyle="Solid" BorderWidth="1" HorizontalAlign="Left">La Superficie Máxima a Ocupar por Institución (SMOI), es la sumatoria de la superficie total de todos los espacios para el personal (X), las áreas de uso común (Y) y áreas complementarias.</asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                            <asp:TableRow>
                                <asp:TableCell Font-Bold="true">Nombre del factor</asp:TableCell>
                                <asp:TableCell Font-Bold="true" HorizontalAlign="Center">Factor</asp:TableCell>
                                <asp:TableCell Font-Bold="true" HorizontalAlign="Center">M2 por Factor</asp:TableCell>
                                <asp:TableCell Font-Bold="true" HorizontalAlign="Center">Personas</asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>Superficie Máxima a Ocupar por Todos los Niveles (m2)</asp:TableCell>
                                <asp:TableCell HorizontalAlign="Center">X</asp:TableCell>
                                <asp:TableCell HorizontalAlign="Center">
                                    <asp:Label ID="LabelTotalSMOIm2FactorX" runat="server"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="Center">
                                    <asp:Label runat="server" ID="lblPerX"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>Áreas de Uso Común y Áreas de Circulación por 0.44 (m2)</asp:TableCell>
                                <asp:TableCell HorizontalAlign="Center">Y</asp:TableCell>
                                <asp:TableCell HorizontalAlign="Center">
                                    <asp:Label ID="LabelTotalSMOIm2FactorY" runat="server"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="Center"></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>Áreas Complementarias (m2)</asp:TableCell>
                                <asp:TableCell HorizontalAlign="Center">Z</asp:TableCell>
                                <asp:TableCell HorizontalAlign="Center">
                                    <asp:Label ID="LabelTotalSMOIm2FactorZ" runat="server"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="Center">
                                    <asp:Label runat="server" ID="lblPerZ"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>Superficie Máxima a Ocupar por la Institución (m2)</asp:TableCell>
                                <asp:TableCell HorizontalAlign="Center">SMOI</asp:TableCell>
                                <asp:TableCell HorizontalAlign="Center">
                                    <asp:Label ID="LabelTotalSMOIm2" runat="server"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="Center"></asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>

                        <asp:Table runat="server" Width="100%" class="table table-bordered">
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell Font-Bold="true" BorderStyle="Solid" BorderWidth="1" HorizontalAlign="Center" BackColor="LightGray" BorderColor="Gray">Descripción de Operaciones</asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    I. Para los grupos jerárquicos del 1 al 7, la superficie unitaria máxima por servidor (A) incluye los siguientes espacios: área de trabajo, mesa de juntas, zona de espera y baño privado.
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    II. El producto de la columna C es el resultado de multiplicar las columnas A y B.
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    III. Las áreas de uso común y de circulación (Y) incluyen vestíbulos, pasillos, baños comunes, cuartos de máquinas, cuartos de aseo, bodegas, cubos de elevadores, escaleras, entre otros.
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    IV. El porcentaje de espacios complementarios no podrá exceder en un 50% al valor X, en caso de ser así será meritorio de un análisis particular por parte del INDAABIN.
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    V. Las áreas complementarias (Z) constituyen aquellos espacios adicionales requeridos para el funcionamiento de la entidad/dependencia, tales como aulas de capacitación, comedor para servidores públicos, auditorio, áreas para archivo muerto y salones de usos múltiples. Para su cálculo se deberá estimar el número de usuarios y multiplicarlos por el factor de m2 por usuario con base en la siguiente tabla:
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <label style="font-weight:bold">Aquellos espacios no considerados en la tabla, se tendrán que justificar por la dependencia solicitante ante el INDAABIN.</label>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </div>





                </div>
            </div>
            <br />
            <br />

            <br />
            <br />

            <br />
            <br />
            <br />

            <br />
            <br />

            <br />
            <br />
            <br />

            <br />
            <br />

            <br />
            
            <br />

            <br />
            <br />

            <br />
        </div>
        <table>
            <tr id="PIE" style="##viejo##">
                <td style="width: 100%">
                    <img src="https://sistemas.indaabin.gob.mx/ImagenesComunes/piePagina.png" alt="" style="height: 64px; width: 1000px" />
                </td>
            </tr>
        </table>
    </form>

</body>
</html>

