<%@ Page Title="Operaciones" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="fVenta.aspx.cs" Inherits="TPC_equipo_9A.fVenta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>

    <style>
        .centered-gridview {
            text-align: center;
        }

        h1 {
            font-family: 'Arial', sans-serif;
            text-align: center;
            font-size: 2em;
            color: #333;
        }

        .table {
            width: 50%;
            max-width: 900px;
            margin: 20px auto !important;
            border-collapse: collapse;
            font-size: 18px;
            background-color: #f9f9f9;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            border-radius: 8px;
            overflow: hidden;
        }


            .table th, .table td {
                padding: 12px 15px;
                border: 1px solid #dddddd;
            }


            .table th {
                background-color: #4CAF50;
                color: white;
                font-weight: bold;
                text-align: center;
            }


            .table tr:nth-child(even) {
                background-color: #f2f2f2;
            }


            .table tr:hover {
                background-color: #f1f1f1;
            }


            .table td {
                text-align: center;
            }
    </style>

    <script type="text/javascript">
        function toggleSwitchLabel(checkbox) {
            var label = checkbox.nextElementSibling;
            if (checkbox.checked) {
                label.textContent = "Confirmada";
            } else {
                label.textContent = "Anulada";
            }
        }

        document.addEventListener("DOMContentLoaded", function () {
            const errorMessageLabel = document.getElementById("<%= lblErrorMessage.ClientID %>");
            const modal = new bootstrap.Modal(document.getElementById('staticBackdrop'));

            if (errorMessageLabel && errorMessageLabel.innerHTML.trim() !== "") {
                modal.show();
            }
        });
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <br />
    <h1 style="font-family: Rockwell, sans-serif; text-align: center; font-size: 3em; color: #333;">Ventas</h1>

    <div style="width: 100%; display: flex; justify-content: center;">
        <div class="col-md-6">
            <div class="input-group">
                <asp:TextBox ID="txtBuscar" CssClass="form-control form-control-lg" runat="server" Placeholder="Buscar..."></asp:TextBox>
                <asp:Button ID="btnBuscar" CssClass="btn btn-primary" Text="Buscar" OnClick="btnBuscar_Click" runat="server" />
            </div>
        </div>
    </div>
    <div style="width: 100%; display: flex; justify-content: center;">
        <asp:GridView ID="gvVentas" runat="server" AutoGenerateColumns="false" CssClass="centered-gridview table" OnPageIndexChanging="gvVentas_PageIndexChanging" DataKeyNames="IdVenta" AllowPaging="True" PageSize="6">
            <Columns>
                <asp:BoundField DataField="IdVenta" HeaderText="ID Venta" />
                <asp:BoundField DataField="Cliente.Nombre" HeaderText="Nombre Cliente" />
                <asp:BoundField DataField="Cliente.Apellido" HeaderText="Apellido Cliente" />
                <asp:BoundField DataField="Cliente.Correo" HeaderText="Correo" />
                <asp:BoundField DataField="FechaVenta" HeaderText="Fecha de Venta" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="NumeroFactura" HeaderText="Número de Factura" />
                <asp:TemplateField HeaderText="Verificación">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkVerificacion" runat="server"
                            AutoPostBack="true"
                            Checked='<%# Convert.ToInt32(Eval("Estado")) == 1 %>'
                            OnCheckedChanged="chkVerificacion_CheckedChanged" />
                        <asp:Label ID="lblEstado" runat="server"
                            Text='<%# Convert.ToInt32(Eval("Estado")) == 1 ? "Confirmada" : "Anulada" %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ver Detalle" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <asp:Button ID="btnVerDetalle" runat="server" Text="Ver Detalle" CommandName="VerDetalle" CommandArgument='<%# Eval("IdVenta") %>' OnClick="btnVerDetalle_Click" CssClass="btn btn-link" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

    <div class="modal fade" id="modalDetalleVenta" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel">Detalles de la Venta</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div style="width: 100%; display: flex; justify-content: center;">
                        <asp:GridView ID="gvDetalleVenta" runat="server" AutoGenerateColumns="false" CssClass="centered-gridview" Visible="false">
                            <Columns>
                                <asp:BoundField DataField="NombreProducto" HeaderText="Producto" />
                                <asp:BoundField DataField="NombreMarca" HeaderText="Marca" />
                                <asp:BoundField DataField="NombreCategoria" HeaderText="Categoria" />
                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                <asp:BoundField DataField="PrecioUnitario" HeaderText="Precio Unitario" DataFormatString="{0:C}" />
                                <asp:BoundField DataField="PrecioTotal" HeaderText="Precio Total" DataFormatString="{0:C}" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <!--<asp:Label ID="LblError" runat="server" CssClass="text-danger" Visible="false"></asp:Label>-->
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                    <asp:Button ID="btnDescargarPDF" runat="server" CssClass="btn btn-primary" Text="Descargar" OnClick="btnDescargarPDF_Click" />
                </div>
            </div>
        </div>
    </div>

    <div style="width: 100%; display: flex; justify-content: center; margin-top: 20px;">
        <asp:Button ID="btnGenerarVenta" runat="server" Text="Generar Venta" CssClass="btn btn-success" OnClick="btnGenerarVenta_Click" />
    </div>
    <div class="modal fade" id="staticBackdrop" data-backdrop="static" data-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h1 class="modal-title fs-5" id="staticBackdropLabel">Generar Venta</h1>
                    <button type="button" class="btn-close" data-dismiss="modal" aria-label="Close" runat="server" onserverclick="btnX_ServerClick"></button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <!-- Columna 1 -->
                        <div class="col-6">
                            <div class="mb-3">
                                <label for="ddlCliente" class="form-label">Cliente: </label>
                                <asp:DropDownList ID="ddlCliente" CssClass="form-select" runat="server" AppendDataBoundItems="true">
                                </asp:DropDownList>
                            </div>

                            <div class="mb-3">
                                <label for="txtFechaVenta">Fecha de Venta: </label>
                                <input type="date" id="txtFechaVenta" runat="server" class="form-control" />
                            </div>
                        </div>

                        <!-- Columna 2 -->
                        <div class="col-6">
                            <div class="mb-3">
                                <label for="ddlProducto" class="form-label">Producto: </label>
                                <asp:DropDownList ID="ddlProducto" CssClass="form-select" runat="server" AppendDataBoundItems="true">
                                </asp:DropDownList>
                            </div>

                            <div class="mb-3">
                                <label for="txtCantidad" class="form-label">Cantidad: </label>
                                <asp:TextBox runat="server" ID="txtCantidad" CssClass="form-control" />
                            </div>
                        </div>
                    </div>
                    <asp:Label runat="server" ID="lblErrorMessage" CssClass="text-danger" Visible="False"></asp:Label>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal" runat="server" onserverclick="btnCerrar_ServerClick">Cerrar</button>
                    <button type="button" class="btn btn-primary" id="btnAceptarGenerarVenta" runat="server" onserverclick="btnAceptarGenerarVenta_ServerClick">
                        Generar Venta
                    </button>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
