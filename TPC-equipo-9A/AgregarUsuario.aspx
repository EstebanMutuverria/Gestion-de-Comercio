<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="AgregarUsuario.aspx.cs" Inherits="TPC_equipo_9A.AgregarUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-6">
                <div class="card">
                    <div class="card-header text-center">
                        <h3>Agregar Usuario</h3>
                    </div>
                    <div class="card-body">
                        <div class="form-group">
                            <label for="txtNombreUsuario">Nombre de Usuario:</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtNombreUsuario" runat="server" CssClass="form-control" />
                            </div>
                            <asp:RequiredFieldValidator
                                ID="rfvNombreUsuario"
                                runat="server"
                                ControlToValidate="txtNombreUsuario"
                                ErrorMessage="El nombre de usuario es requerido."
                                CssClass="text-danger"
                                Display="Dynamic" />
                            <asp:CustomValidator
                                ID="cvNombreUsuario"
                                runat="server"
                                ControlToValidate="txtNombreUsuario"
                                ErrorMessage="El nombre de usuario ya existe."
                                CssClass="text-danger"
                                Display="Dynamic"
                                OnServerValidate="ValidateUsername" />
                        </div>
                        <div class="form-group mt-3">
                            <label for="txtContrasena">Contraseña:</label>
                            <div class="input-group">
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtContrasena" runat="server" CssClass="form-control" TextMode="Password" />
                                </div>
                                <span class="input-group-text" onclick="togglePasswordVisibility('<%= txtContrasena.ClientID %>', 'toggleEye')">
                                    <i class="bi bi-eye" id="toggleEye"></i>
                                </span>
                            </div>
                            <asp:RequiredFieldValidator
                                ID="rfvContrasena"
                                runat="server"
                                ControlToValidate="txtContrasena"
                                ErrorMessage="La contraseña es requerida."
                                CssClass="text-danger"
                                Display="Dynamic" />
                        </div>
                        <div class="form-group mt-3">
                            <label for="txtConfirmarContrasena">Confirmar Contraseña:</label>
                            <div class="input-group">
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtConfirmarContrasena" runat="server" CssClass="form-control" TextMode="Password" />
                                </div>
                                <span class="input-group-text" onclick="togglePasswordVisibility('<%= txtConfirmarContrasena.ClientID %>', 'toggleEyeConfirm')">
                                    <i class="bi bi-eye" id="toggleEyeConfirm"></i>
                                </span>
                            </div>
                            <asp:RequiredFieldValidator
                                ID="rfvConfirmarContrasena"
                                runat="server"
                                ControlToValidate="txtConfirmarContrasena"
                                ErrorMessage="La contraseña de confirmación es requerida."
                                CssClass="text-danger"
                                Display="Dynamic" />
                            <asp:CustomValidator
                                ID="cvContrasenas"
                                runat="server"
                                ControlToValidate="txtConfirmarContrasena"
                                ErrorMessage="Las contraseñas no coinciden."
                                CssClass="text-danger"
                                Display="Dynamic"
                                OnServerValidate="ValidatePasswords" />
                        </div>
                        <div class="form-group mt-3">
                            <label for="ddlRol">Rol:</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-control" Required="true">
                                    <asp:ListItem Text="Administrador" Value="Administrador"></asp:ListItem>
                                    <asp:ListItem Text="Vendedor" Value="Vendedor"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="d-flex justify-content-between mt-4">
                            <asp:Button ID="btnCancelar" runat="server" CssClass="btn btn-secondary" Text="Volver" OnClick="btnCancelar_Click" CausesValidation="false" />
                            <asp:Button ID="btnAgregarUsuario" runat="server" CssClass="btn btn-success" Text="Guardar Cambios" OnClick="btnAgregarUsuario_Click" CausesValidation="true" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function togglePasswordVisibility(fieldId, iconId) {
            var passwordField = document.getElementById(fieldId);
            var toggleEye = document.getElementById(iconId);
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
