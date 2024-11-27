<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Perfil.aspx.cs" Inherits="TPC_equipo_9A.Perfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-6">
                <div class="card">
                    <div class="card-header text-center">
                        <h3>Perfil de Usuario</h3>
                    </div>
                    <div class="card-body">
                        <div class="text-center mb-4">
                            <asp:Image ID="imgFotoPerfil" runat="server" CssClass="rounded-circle" Width="150px" Height="150px" />
                            <div class="mt-3">
                                <asp:FileUpload ID="fuFotoPerfil" runat="server" CssClass="form-control-file" />
                            </div>
                            <div class="mt-3">
                                <asp:Button ID="btnEliminarFoto" runat="server" CssClass="btn btn-danger mt-2" Text="Eliminar Foto" OnClick="btnEliminarFoto_Click" Visible="false" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="txtNombreUsuario">Nombre de Usuario:</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtNombreUsuario" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="form-group mt-3">
                            <label for="lblContrasena">Contraseña:</label>
                            <div class="input-group">
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtContrasena" runat="server" CssClass="form-control" TextMode="Password" />
                                </div>
                                <span class="input-group-text" onclick="togglePasswordVisibility()">
                                    <i class="bi bi-eye" id="toggleEye"></i>
                                </span>
                            </div>
                        </div>
                        <div class="form-group mt-3">
                            <label for="lblRol">Rol:</label>
                            <div class="col-md-3">
                                <asp:Label ID="lblRol" runat="server" CssClass="form-control" Enabled="false" />
                            </div>
                        </div>
                        <div class="d-flex justify-content-between mt-4">
                            <asp:Button ID="btnGuardarCambios" runat="server" CssClass="btn btn-success" Text="Guardar Cambios" OnClick="btnGuardarCambios_Click" />
                            <asp:Button ID="btnControlAcceso" runat="server" CssClass="btn btn-primary" Text="Volver" Visible="false" OnClick="btnIrAControlAcceso_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function togglePasswordVisibility() {
            var passwordField = document.getElementById('<%= txtContrasena.ClientID %>');
            var toggleEye = document.getElementById('toggleEye');
            if (passwordField.type === "password") {
                passwordField.type = "text";
                toggleEye.classList.remove('bi-eye');
                toggleEye.classList.add('bi-eye-slash');
            } else {
                passwordField.type = "password";
                toggleEye.classList.remove('bi-eye-slash');
                toggleEye.classList.add('bi-eye');
            }
        }
    </script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-icons/1.5.0/font/bootstrap-icons.min.css" rel="stylesheet">
</asp:Content>
