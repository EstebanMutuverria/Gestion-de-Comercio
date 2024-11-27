<%@ Page Title="Control de Acceso" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ControlAcceso.aspx.cs" Inherits="TPC_equipo_9A.ControlAcceso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function confirmarEliminacion(NombreUsuario, Rol) {
            var mensaje = "¿Estás seguro que deseas eliminar al " + Rol + " '" + NombreUsuario + "'?";
            return confirm(mensaje);
        }
    </script>
    <style>
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
        <h1>Control de Acceso</h1>

        <div class="card">
            <div class="card-header">
                Buscar Usuario
            </div>
            <div class="card-body">
                <div class="row mb-3">
                    <div class="col-md-2">
                        <label for="txtNombreUsuario">Nombre de Usuario</label>
                        <asp:TextBox ID="txtNombreUsuario" runat="server" CssClass="form-control w-75" />
                    </div>

                    <div class="col-md-2">
                        <label for="ddlRol">Rol</label>
                        <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-control w-75">
                            <asp:ListItem Text="Seleccionar" Value="" Selected="True" />
                            <asp:ListItem Text="Vendedor" Value="Vendedor"></asp:ListItem>
                            <asp:ListItem Text="Administrador" Value="Administrador"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <label for="ddlEstado">Estado</label>
                        <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-control w-75">
                            <asp:ListItem Text="Seleccionar" Value="" Selected="True" />
                            <asp:ListItem Text="Activo" Value="true"></asp:ListItem>
                            <asp:ListItem Text="Inactivo" Value="false"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-2 d-flex align-items-end">
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-secondary" OnClick="btnBuscar_Click" />
                    </div>

                    <div class="col-md-4 d-flex justify-content-end align-items-end">
                        <asp:Button ID="btnAgregarUsuario" runat="server" Text="Agregar Usuario" CssClass="btn btn-primary" OnClick="btnAgregarUsuario_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div class="card mt-5">
            <div class="card-header">
                Lista de Usuarios
            </div>
            <div class="card-body">
                <asp:GridView ID="gvUsuarios" runat="server" CssClass="table table-striped" DataKeyNames="IdUsuario" AutoGenerateColumns="False" OnRowCommand="gvUsuarios_RowCommand" AllowPaging="True" PageSize="6" OnPageIndexChanging="gvUsuarios_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="UsuarioID" HeaderText="ID" Visible="False" />
                        <asp:ImageField DataImageUrlField="FotoPerfil" HeaderText="" ControlStyle-CssClass="image-thumbnail" NullDisplayText="/images/user.png" />
                        <asp:BoundField DataField="NombreUsuario" HeaderText="Nombre de Usuario" />
                        <asp:BoundField DataField="Rol" HeaderText="Rol" />
                        <asp:TemplateField HeaderText="Estado">
                            <ItemTemplate>
                                <span class='<%# Convert.ToBoolean(Eval("Estado")) ? "estado-activo" : "estado-inactivo" %>' style="width: 70px; display: inline-block;">
                                    <%# Convert.ToBoolean(Eval("Estado")) ? "Activo" : "Inactivo" %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnEditar" runat="server" Text="Editar" CommandName="Editar" CommandArgument='<%# Eval("IdUsuario ") %>' CssClass="btn btn-warning btn-sm" />
                                <asp:Button 
                                    ID="btnEliminar" 
                                    runat="server" 
                                    Text="Eliminar" 
                                    OnClientClick='<%# "return confirmarEliminacion(\"" + Eval("NombreUsuario") + "\", \"" + Eval("Rol") + "\");" %>'
                                    CommandName="Eliminar" 
                                    CommandArgument='<%# Eval("IdUsuario ") %>' 
                                    CssClass="btn btn-danger btn-sm" 
                                    />
                                <asp:Button ID="btnVerPerfil" runat="server" Text="Ver Perfil" CommandName="VerPerfil" CommandArgument='<%# Eval("IdUsuario ") %>' CssClass="btn btn-info btn-sm" />

                                <span class="btn-separator"></span>

                                <asp:Button ID="btnActivar" runat="server" Text="Activar" CommandName="Activar" CommandArgument='<%# Eval("IdUsuario ") %>' CssClass="btn btn-secondary btn-sm" Style="background-color: #198754; border-color: #198754; width: 105px;" Visible='<%# !Convert.ToBoolean(Eval("Estado")) %>' />
                                <asp:Button ID="btnDesactivar" runat="server" Text="Desactivar" CommandName="Desactivar" CommandArgument='<%# Eval("IdUsuario ") %>' CssClass="btn btn-secondary btn-sm" Style="width: 105px;" Visible='<%# Convert.ToBoolean(Eval("Estado")) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
