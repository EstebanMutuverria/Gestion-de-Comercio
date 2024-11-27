<%@ Page Title="Relaciones Comerciales" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="RelacionesComerciales.aspx.cs" Inherits="TPC_equipo_9A.RelacionesComerciales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function confirmarEliminacion(Relacion, Nombre, Correo) {
            var mensaje = "¿Estás seguro que deseas eliminar al siguiente " + Relacion + "? \n - Nombre: " + Nombre + "\n - Correo: " + Correo;
        return confirm(mensaje);
    }
    </script>
    <style>
        table .custom-active {
            width: 115px;
            background-color: #198754;
            border-color: #198754;
        }

            table .custom-active:hover, .btn-secondary:hover {
                background-color: #198754;
                border-color: #198754;
            }

        .table td {
            max-width: auto;
            min-width: 100px;
            word-break: break-word;
            padding: 8px;
            border: 1px solid #dee2e6;
        }

        h1 {
            font-family: Rockwell, sans-serif;
            text-align: center;
            font-size: 3em;
            color: #333;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <h1>Relaciones Comerciales</h1>

        <div class="card">
            <div class="card-header">
                Buscar Relación
            </div>
            <div class="card-body">
                <div class="row mb-3">
                    <div class="col-md-2">
                        <label for="txtNombreRelacion">Nombre</label>
                        <asp:TextBox ID="txtNombreRelacion" runat="server" CssClass="form-control w-75" />
                    </div>

                    <div class="col-md-2">
                        <label for="txtDNICUIT">DNI / CUIT</label>
                        <asp:TextBox ID="txtDNICUIT" runat="server" CssClass="form-control w-75" />
                    </div>

                    <div class="col-md-2">
                        <label for="ddlTipoRelacion">Tipo de Relación</label>
                        <asp:DropDownList ID="ddlTipoRelacion" runat="server" CssClass="form-control w-75">
                            <asp:ListItem Text="Seleccionar" Value="" Selected="True" />
                            <asp:ListItem Text="Cliente" Value="Cliente"></asp:ListItem>
                            <asp:ListItem Text="Proveedor" Value="Proveedor"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-2 d-flex align-items-end">
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-secondary" OnClick="btnBuscar_Click" ToolTip="Buscar relaciones comerciales por los criterios seleccionados" />
                    </div>

                    <div class="col-md-4 d-flex justify-content-end align-items-end">
                        <asp:Button ID="btnAgregarRelacion" runat="server" Text="Agregar Relación" CssClass="btn btn-primary btn-custom-margin" OnClick="btnAgregarRelacion_Click" ToolTip="Agregar una nueva relación comercial" />
                    </div>
                </div>
            </div>
        </div>

        <div class="card mt-5">
            <div class="card-header">
                Lista de Relaciones
            </div>
            <div class="card-body">
                <asp:GridView ID="gvRelaciones" runat="server" CssClass="table table-hover table-bordered"
                    DataKeyNames="IdRelacion" AutoGenerateColumns="False"
                    OnRowCommand="gvRelaciones_RowCommand"
                    AllowPaging="True" PageSize="6" OnPageIndexChanging="gvRelaciones_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />

                        <asp:TemplateField HeaderText="Apellido">
                            <ItemTemplate>
                                <asp:Label ID="lblApellido" runat="server" Text='<%# string.IsNullOrWhiteSpace(Convert.ToString(Eval("Apellido"))) ? "-" : Eval("Apellido") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="Correo" HeaderText="Correo" />
                        <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                        <asp:BoundField DataField="Direccion" HeaderText="Dirección" />

                        <asp:TemplateField HeaderText="DNI">
                            <ItemTemplate>
                                <asp:Label ID="lblDNI" runat="server" Text='<%# string.IsNullOrEmpty(Eval("DNI").ToString()) ? "-" : Eval("DNI") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="CUIT">
                            <ItemTemplate>
                                <asp:Label ID="lblCUIT" runat="server" Text='<%# string.IsNullOrEmpty(Eval("CUIT").ToString()) ? "-" : Eval("CUIT") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Relación">
                            <ItemTemplate>
                                <asp:Label ID="lblRelacion" runat="server" Text='<%# Eval("Relacion") %>'
                                    ForeColor='<%# Eval("Relacion").ToString() == "Proveedor" ? System.Drawing.Color.Orange : System.Drawing.Color.Blue %>'
                                    Font-Bold="True">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Estado">
                            <ItemTemplate>
                                <span class='<%# Convert.ToBoolean(Eval("Estado")) ? "estado-activo" : "estado-inactivo" %>' style="width: 70px; display: inline-block;">
                                    <%# Convert.ToBoolean(Eval("Estado")) ? "Activo" : "Inactivo" %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <ItemTemplate>
                                <div class="d-flex align-items-center">
                                    <asp:Button ID="btnEditar" runat="server" Text="Editar" CommandName="Editar" CommandArgument='<%# Eval("IdRelacion") + "," + Eval("Relacion") %>' CssClass="btn btn-warning btn-sm" />

                                    <span class="btn-separator mx-2"></span>

                                    <asp:Button ID="btnEliminar" 
                                        runat="server" 
                                        Text="Eliminar" 
                                        CommandName="Eliminar" 
                                        CommandArgument='<%# Eval("IdRelacion") + "," + Eval("Relacion")%>'
                                        CssClass="btn btn-danger btn-sm"
                                        OnClientClick='<%# "return confirmarEliminacion(\"" + Eval("Relacion") + "\", \"" + Eval("Nombre") + "\", \"" + Eval("Correo") + "\");" %>' />

                                    <span class="btn-separator mx-2"></span>

                                    <asp:Button
                                        ID="btnEstado"
                                        runat="server"
                                        Text='<%# Convert.ToBoolean(Eval("Estado")) ? "Desactivar" : "Activar" %>'
                                        CommandName="Estado"
                                        CommandArgument='<%# Eval("IdRelacion") + "," + Eval("Relacion")%>'
                                        CssClass='<%# Convert.ToBoolean(Eval("Estado")) ? "btn btn-secondary" : "btn btn-secondary custom-active" %>' />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
