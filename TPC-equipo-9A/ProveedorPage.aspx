<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ProveedorPage.aspx.cs" Inherits="TPC_equipo_9A.ProveedorPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-6">
                <div class="card">
                    <div class="card-header text-center">
                        <h3>Editando Proveedor</h3>
                    </div>

                    <div class="card-body">
                        <asp:HiddenField ID="hfIdProveedor" runat="server" />
                        <h4 id="DatosPersonales" runat="server">Datos Personales</h4>

                        <div class="form-group mt-3">
                            <label id="lblDNI" runat="server" for="txtDNI">DNI:</label>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtDNI" runat="server" CssClass="form-control" />
                            </div>
                        </div>

                        <div class="form-group mt-3">
                            <label id="lblCUIT" runat="server" for="txtCUIT">CUIT:</label>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtCUIT" runat="server" CssClass="form-control" />
                            </div>
                        </div>

                        <div class="form-group mt-3">
                            <label for="txtNombre">Nombre:</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
                            </div>
                            <asp:RequiredFieldValidator
                                ID="rfvNombre"
                                runat="server"
                                ControlToValidate="txtNombre"
                                ErrorMessage="El nombre es requerido."
                                CssClass="text-danger"
                                Display="Dynamic" />
                        </div>

                        <div class="form-group mt-3">
                            <label for="txtApellido">Apellido:</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" />
                            </div>
                            <asp:RegularExpressionValidator
                                ID="revApellido"
                                runat="server"
                                ControlToValidate="txtApellido"
                                ErrorMessage="El apellido solo puede contener letras, espacios y caracteres especiales como tildes."
                                CssClass="text-danger"
                                Display="Dynamic"
                                ValidationExpression="^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]*$" />

                        </div>

                        <br />
                        <h4 id="DatosContacto" runat="server">Datos de Contacto</h4>

                        <div class="form-group mt-3">
                            <label for="txtCorreo">Correo:</label>
                            <div class="col-md-5">
                                <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" />
                            </div>
                            <asp:RequiredFieldValidator
                                ID="rfvCorreo"
                                runat="server"
                                ControlToValidate="txtCorreo"
                                ErrorMessage="El correo es requerido."
                                CssClass="text-danger"
                                Display="Dynamic" />
                            <asp:RegularExpressionValidator
                                ID="revCorreo"
                                runat="server"
                                ControlToValidate="txtCorreo"
                                ErrorMessage="El correo es inválido."
                                CssClass="text-danger"
                                Display="Dynamic"
                                ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$" />
                        </div>

                        <div class="form-group mt-3">
                            <label for="txtTelefono">Teléfono:</label>
                            <div class="col-md-5">
                                <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" />
                            </div>
                            <asp:RequiredFieldValidator
                                ID="rfvTelefono"
                                runat="server"
                                ControlToValidate="txtTelefono"
                                ErrorMessage="El teléfono es requerido."
                                CssClass="text-danger"
                                Display="Dynamic" />
                            <asp:RegularExpressionValidator
                                ID="revTelefono"
                                runat="server"
                                ControlToValidate="txtTelefono"
                                ErrorMessage="El teléfono solo puede contener números, espacios, y +/-"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ValidationExpression="^[\d\s-+]+$" />
                        </div>

                        <div class="form-group mt-3">
                            <label for="txtDireccion">Dirección:</label>
                            <div class="col-md-5">
                                <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control" />
                            </div>
                            <asp:RequiredFieldValidator
                                ID="rfvDireccion"
                                runat="server"
                                ControlToValidate="txtDireccion"
                                ErrorMessage="La dirección es requerida."
                                CssClass="text-danger"
                                Display="Dynamic" />
                        </div>

                        <div class="d-flex justify-content-between mt-4">
                            <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-success" Text="Guardar Cambios" OnClick="btnGuardar_Click" CausesValidation="true" />
                            <asp:Button ID="btnCancelar" runat="server" CssClass="btn btn-secondary" Text="Cancelar" OnClick="btnCancelar_Click" CausesValidation="false" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

