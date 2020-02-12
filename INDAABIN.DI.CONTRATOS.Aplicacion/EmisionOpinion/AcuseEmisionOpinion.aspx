<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AcuseEmisionOpinion.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.EmisionOpinion.AcuseEmisionOpinion1" %>

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
                        $("#LogoHeaderEmision").attr("src", "https://sistemas.indaabin.gob.mx/ImagenesComunes/SHCP-INDAABINREDUCIDO.PNG");
                        $("#background").css("background-image", "url(https://sistemas.indaabin.gob.mx/ImagenesComunes/nuevoescudo.png)");
                        $("#background").css("background-position", "left");
                        $("#form1").css("font-family", "Montserrat");
                        $('#EncabezadoViejo').hide();
                        $('#EncabezadoNuevo').show();
                        $('#PIE').hide();
                    }
                    else {
                        $('#EncabezadoViejo').hide();
                        $('#EncabezadoNuevo').show();
                    }
                }
                else {
                    $('#EncabezadoViejo').hide();
                    $('#EncabezadoNuevo').show();
                }
            }
            else {
                $('#EncabezadoViejo').hide();
                $('#EncabezadoNuevo').show();
            }

        }


    </script>


</head>

<body onload="deshabilitaRetroceso()">

    <form id="form1" runat="server" style="font-size: 14px; ##font##">

        <div id="ZonaNoImrpimible" style="text-align: center">
            <br />
            <button id="ButtonExportarPdf" class="btn btn-default" tooltip="Da clic para exportar a PDF." type="button" runat="server" onserverclick="ButtonExportarPdf_Click"><span class="glyphicon glyphicon-export"></span>Exportar a PDF</button>
            <input type="button" id="ButtonImprimir" value="Imprimir" onclick="javascript: window.print()" class="btn" style="display: none;" />
            <%--<br />--%>
            <br />
            <asp:Label ID="LabelInfo" runat="server"></asp:Label>
            <%--<br />--%>
        </div>

        <table id="SMOI" style="width: 100%">
            <tr id="EncabezadoNuevo" style="##nuevo##">

                <td colspan="2">
                    <table style="width: 100%;">
                        <tr>
                            <td class="auto-style1" style="width: 20%">
                                <img id="LogoHeader" src="https://sistemas.indaabin.gob.mx/ImagenesComunes/Hacienda-Indaabin-Zapata-2019.png" alt="" style="height: 60px; width: 550px" />
                            </td>
                            <td style="text-align: right; width: 80%" class="auto-style1">Dirección General de Política y Gestión  Inmobiliaria
                                            <br />
                                Dirección de Planeación Inmobiliaria
                                              <br />
                            </td>
                            <%--    <td>
                                <img src="https://sistemas.indaabin.gob.mx/ImagenesComunes/SHCP-LOGO-2019.png" alt="" style="height: 70px; width: 300px" />

                            </td>
                            <td style="text-align: center;">Dirección General de Política y Gestión  Inmobiliaria<br />
                                Dirección de Planeación Inmobiliaria

                            </td>
                            <td>
                                <img src="https://sistemas.indaabin.gob.mx/ImagenesComunes/LOGO-INDAABIN-2019.png" alt="" style="height: 70px; width: 250px" />

                            </td>--%>
                        </tr>
                    </table>
                </td>
            </tr>


            <tr id="EncabezadoViejo" style="##Viejo##">
                <td class="auto-style1" style="width: 20%">
                    <img id="LogoHeaderEmision" src="https://sistemas.indaabin.gob.mx/ImagenesComunes/SHCP-INDAABINREDUCIDO.PNG" alt="" style="height: 119px; width: 474px" />
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


        <div class="panel panel-default">
            <div class="panel-heading" style="font-size: 14px;">
                <strong>ACUSE DE REGISTRO DE EMISIÓN DE OPINIÓN PARA ARRENDAMIENTO:</strong>
                <asp:Label ID="LabelTipoContrato" runat="server" Text="" Style="font-weight: 700"></asp:Label>
                <br />
            </div>
            <%--<div id="background">--%>
            <div>

                <div style="text-align: right;">
                    <asp:Label ID="LabelFechaRegistro" runat="server" Text="Ciudad de México, a [01] de [Agosto] de [2016]."></asp:Label>
                    <br />
                </div>

                <div>
                    <div style="text-align: justify;">
                        <br />
                        <asp:Label ID="LabelTextoRespuesta" runat="server" Text="Con fundamento en lo dispuesto en los numerales 165 y 166 de la Sección III del Capítulo IX del “Acuerdo por el que se establecen las disposiciones en Materia de Recursos Materiales y Servicios Generales”, publicado en el Diario Oficial de la Federación el día 5 de abril de 2016; y de acuerdo a la información capturada en el Sistema de Contratos de Arrendamiento y Otras Figuras de Ocupación de este Instituto, se emite la OPINIÓN FAVORABLE [para la continuación, el nuevo o la sustitución] del arrendamiento del inmueble de referencia, de la Institución Pública que usted representa; debiéndose observar la normatividad aplicable."></asp:Label>
                        <br />
                    </div>

                    <div style="text-align: left;">
                        <br />
                        <strong>Institución Pública</strong>:
                        <asp:Label ID="LabelInstitucion" runat="server" Text=""></asp:Label>
                        <br />
                        <br />
                        <strong>Inmueble Ubicado en</strong>:<br />
                        <asp:Label ID="LabelPais" runat="server"></asp:Label>
                        <br />
                        <asp:Label ID="LabelEntFedyCP" runat="server"></asp:Label>
                        <br />
                        <asp:Label ID="LabelMpo" runat="server"></asp:Label>
                        <br />
                        <asp:Label ID="LabelColonia" runat="server"></asp:Label><br />
                        <asp:Label ID="LabelDirVialidadYNums" runat="server"></asp:Label><br />
                        <br />
                        <asp:Label ID="LabelHoraRegistro" runat="server"></asp:Label>
                        <br />
                        <br />
                        El presente registro de Emisión de Opinión de Arrendamiento, en caso de no ser utilizado en un periodo de seis meses, a partir de la fecha de captura, será inhabilitada del Sistema de Contratos de Arrendamiento y Otras Figuras de Ocupación.<br />
                    </div>
                </div>

                <table style="text-align: center; width: 100%">
                    <tr>
                        <td style="word-break: break-all;">
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
                        <td style="word-break: break-all;">
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
                            <%--RCA 10/08/2018--%>
                            <%--FECHA DE AUTORIZACION--%>
                            <asp:Label ID="FechaAutorizacionEmision" runat="server"></asp:Label>
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
                            <asp:Label ID="LeyendaEmision" runat="server"></asp:Label>
                            <br />
                            
                        </td>
                    </tr>

                </table>

                <br />
                            <br />
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
                        
               
        
        <table>
            <tr id="PIE" style="##viejo##">
                <td>
                    <img src="https://sistemas.indaabin.gob.mx/ImagenesComunes/piePagina.png" alt="" style="height: 64px; width: 1000px" />

                </td>
            </tr>
        </table>

    </form>

</body>
</html>
