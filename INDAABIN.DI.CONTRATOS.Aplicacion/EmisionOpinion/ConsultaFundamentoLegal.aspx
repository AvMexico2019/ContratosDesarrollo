<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConsultaFundamentoLegal.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.EmisionOpinion.ConsultaFundamentoLegal" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta charset="utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <title>Direcciones de Inmuebles</title>

    <!-- CSS -->
    <link href="../css/EstilosEspecificos.css" rel="stylesheet" />
    <link href="https://framework-gb.cdn.gob.mx/assets/styles/main.css" rel="stylesheet"/>
    <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700,600,300' rel='stylesheet' type='text/css'/>
    <link href='https://framework-gb.cdn.gob.mx/favicon.ico' rel='shortcut icon'/>
    <script src="../Scripts/jquery-1.10.2.js"></script>
    <script src="https://framework-gb.cdn.gob.mx/assets/scripts/jquery-ui-datepicker.js"></script>

</head>
<body>
    <form id="form1" runat="server">
         <div class="panel panel-default">
            <div class="panel-heading"><strong>Fundamento legal</strong></div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <span class="control-label"><strong>Aplicable al tipo de solicitud de emisión:</strong></span>
                                <br />
                                <asp:Label ID="LabelTema" runat="server" Text=""></asp:Label> 
                                <br />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <span class="control-label"><strong>Número de concepto:</strong></span>
                                <br />
                                <asp:Label ID="LabelNumCpto" runat="server" Text=""></asp:Label> 
                                <br />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <span class="control-label"><strong>Concepto:</strong></span>
                                <br />
                                <asp:Label ID="LabelDescCpto" runat="server" Text=""></asp:Label>
                                <br />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <span class="control-label"><strong>Fundamento legal:</strong></span>
                                <br />
                                <asp:Label ID="LabelFundamentoLegalCpto" runat="server" Text=""></asp:Label>
                                <br />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <asp:Label ID="LabelInfo" runat="server" Text="" style="font-weight: 700"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
         </div>
    </form>
</body>
</html>
