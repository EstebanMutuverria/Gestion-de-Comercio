<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="AgregarRelacion.aspx.cs" Inherits="TPC_equipo_9A.AgregarRelacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form-header {
            font-size: 24px;
            text-align: center;
            text-decoration: underline;
            margin-bottom: 20px;
        }

        .section-title {
            font-size: 18px;
            margin-top: 20px;
            font-weight: bold;
        }

        .form-container {
            max-width: 800px;
            margin: auto;
        }

        .btn-container {
            text-align: center;
            margin-top: 20px;
        }

        .hidden {
            display: none;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container form-container mt-5">
        <h2 class="form-header">Agregar Relación</h2>

        <div class="row mb-3">
            <div class="col-md-6">
                <label for="ddlTipoRelacion">Tipo de Relación</label>
                <asp:DropDownList ID="ddlTipoRelacion" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTipo_SelectedIndexChanged">
                    <asp:ListItem Text="Seleccionar" Value="" Selected="True" />
                    <asp:ListItem Text="Cliente" Value="Cliente"></asp:ListItem>
                    <asp:ListItem Text="Proveedor" Value="Proveedor"></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator
                    ID="rfvTipoRelacion"
                    runat="server"
                    ControlToValidate="ddlTipoRelacion"
                    InitialValue=""
                    ErrorMessage="Seleccione un tipo de relación."
                    CssClass="text-danger"
                    Display="Dynamic" />
            </div>

            <div class="col-md-6">
                <label for="ddlTipoPersona">Tipo de Persona</label>
                <asp:DropDownList ID="ddlTipoPersona" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTipo_SelectedIndexChanged">
                    <asp:ListItem Text="Seleccionar" Value="" Selected="True" />
                    <asp:ListItem Text="Persona Física" Value="Fisica"></asp:ListItem>
                    <asp:ListItem Text="Persona Jurídica" Value="Juridica"></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvTipoPersona"
                    runat="server"
                    ControlToValidate="ddlTipoPersona"
                    InitialValue=""
                    ErrorMessage="Seleccione un tipo de persona."
                    CssClass="text-danger"
                    Display="Dynamic" />
            </div>
        </div>

        <div id="DatosPersonales" runat="server" class="section-title hidden">Datos Personales</div>

        <div id="PersonFields" runat="server" class="hidden">
            <div class="row">
                <div class="col-md-6">
                    <label for="txtNombre">Nombre</label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator
                        ID="rfvNombre"
                        runat="server"
                        ControlToValidate="txtNombre"
                        ErrorMessage="El Nombre es requerido."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="col-md-6">
                    <label for="txtApellido">Apellido</label>
                    <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" />
                    <asp:RegularExpressionValidator
                        ID="revApellido"
                        runat="server"
                        ControlToValidate="txtApellido"
                        ErrorMessage="El apellido solo puede contener letras, espacios y caracteres especiales como tildes."
                        CssClass="text-danger"
                        Display="Dynamic"
                        ValidationExpression="^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]*$" />

                </div>
            </div>

            <div class="row mt-3">
                <div class="col-md-6">
                    <label id="lblDNI" for="txtDNI" runat="server">DNI</label>
                    <asp:TextBox ID="txtDNI" runat="server" CssClass="form-control" Enabled="false" />
                    <asp:RequiredFieldValidator
                        ID="rfvDNI"
                        runat="server"
                        ControlToValidate="txtDNI"
                        ErrorMessage="DNI es requerido"
                        CssClass="text-danger"
                        Display="Dynamic" />
                    <asp:RegularExpressionValidator
                        ID="revDNI"
                        runat="server"
                        ControlToValidate="txtDNI"
                        ErrorMessage="El formato es inválido. Ej: XX.XXX.XX"
                        CssClass="text-danger"
                        ValidationExpression="^\d{1,2}\.\d{3}\.\d{3}$"
                        Display="Dynamic" />
                    <asp:CustomValidator
                        ID="cvDNI"
                        runat="server"
                        ControlToValidate="txtDNI"
                        ErrorMessage="El DNI ya se encuentra registrado."
                        CssClass="text-danger"
                        Display="Dynamic"
                        OnServerValidate="ValidateDNI" />
                </div>

                <div class="col-md-6">
                    <label id="lblCUIT" for="txtCUIT" runat="server">CUIT</label>
                    <asp:TextBox ID="txtCUIT" runat="server" CssClass="form-control" Enabled="false" />
                    <asp:RequiredFieldValidator
                        ID="rfvCUIT"
                        runat="server"
                        ControlToValidate="txtCUIT"
                        ErrorMessage="El CUIT es requerido."
                        CssClass="text-danger"
                        Display="Dynamic" />
                    <asp:RegularExpressionValidator
                        ID="revCUIT"
                        runat="server"
                        ControlToValidate="txtCUIT"
                        ErrorMessage="El formato es inválido. Ej: XX-XXXXXXXX-X"
                        CssClass="text-danger"
                        ValidationExpression="^\d{1,2}-\d{8}-\d{1}$"
                        Display="Dynamic" />
                    <asp:CustomValidator
                        ID="cvcuit"
                        runat="server"
                        ControlToValidate="txtCUIT"
                        ErrorMessage="El CUIT ya se encuentra registrado."
                        CssClass="text-danger"
                        Display="Dynamic"
                        OnServerValidate="ValidateCUIT" />
                </div>
            </div>

            <div class="section-title">Datos de Contacto</div>

            <div class="row mt-3">
                <div class="col-md-4">
                    <label for="txtCorreo">Correo</label>
                    <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" />
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

                <div class="col-md-4">
                    <label for="txtTelefono">Teléfono</label>
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" />
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

                <div class="col-md-4">
                    <label for="txtDireccion">Dirección</label>
                    <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator
                        ID="rfvDireccion"
                        runat="server"
                        ControlToValidate="txtDireccion"
                        ErrorMessage="La dirección es requerida."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

            </div>
        </div>

        <div class="btn-container">
            <asp:Button ID="btnVolver" runat="server" Text="Volver" CssClass="btn btn-secondary" OnClick="btnVolver_Click" CausesValidation="false" />
            <asp:Button ID="btnAgregar" runat="server" Text="Guardar Cambios" CssClass="btn btn-success" OnClick="btnAgregar_Click" />
        </div>
    </div>
</asp:Content>
