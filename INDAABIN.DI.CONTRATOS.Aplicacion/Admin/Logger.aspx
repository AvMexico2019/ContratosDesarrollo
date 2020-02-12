<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Logger.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.Admin.Logger" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src='<%: ResolveClientUrl("~/Scripts/jquery-1.6.4.min.js") %>'></script>
    <script src='<%: ResolveClientUrl("~/Scripts/jquery.signalR-2.2.2.min.js") %>'></script>
    <script src='<%: ResolveClientUrl("~/signalr/hubs") %>'></script>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/knockout/3.4.2/knockout-min.js"></script>

    <script type="text/javascript">

        $(document).ready(function () {

            $.connection.hub.logging = true;
            $.connection.hub.start()
            .done(function () {
                console.log('connection.hub.started...');
            })
            .fail(function () {
                console.log("Could not Connect!");
            });

            function AppViewModel() {
                var self = this;

                self.issues = ko.observableArray([]);

                self.removeIssue = function () {
                    self.issues.remove(this);
                }

                $.connection.broadcasterHub.client.broadCastEvent = function (issue) {

                    if (issue) {
                        console.log("broadCastEvent : " + JSON.stringify(issue));
                        self.issues.push(issue);

                        if (self.issues.length > 20) {
                            self.issues.slice(0, 1);
                        }
                    }
                };
            }

            ko.applyBindings(new AppViewModel());



        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <div class="row">
        <div class="col-md-4">
            <div class="form-group">
                <h1>Monitor de eventos SAU</h1>

            </div>
        </div>
        <div class="col-md-8"></div>
    </div>

    <div class="row">
        <h2>Traza de eventos</h2>
        <div class="col-md-12">
            <table class="table table-bordered">
                <thead>
                    <tr>
                        <td>Fecha/Hora</td>
                        <td>Tipo evento</td>
                        <td>Mensaje</td>
                        <td></td>
                    </tr>

                </thead>
                <tbody data-bind="foreach: issues">
                    <tr>
                        <td data-bind="text: TimeStamp"></td>
                        <td data-bind="text: Event"></td>
                        <td data-bind="text: Message"></td>
                        <td>
                            <a href="#" data-bind="click: $parent.removeIssue">Eliminar</a>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>

</asp:Content>
