<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TPC_equipo_9A.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        body {
            background-color: #f8f9fa; /* Color de fondo claro */
        }

        .card {
            border: none; /* Sin borde para un aspecto limpio */
            border-radius: 15px; /* Bordes redondeados */
            box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1); /* Sombra */
        }

        .card-header {
            background-color: #007bff; /* Color del encabezado */
            color: white; /* Texto blanco */
            border-top-left-radius: 15px; /* Bordes redondeados en la parte superior */
            border-top-right-radius: 15px;
        }

        .form-group label {
            font-weight: 500; /* Peso de la fuente */
            margin-bottom: 5px; /* Espacio entre etiqueta y campo */
        }

        .form-control {
            border-radius: 10px; /* Bordes redondeados en los campos de texto */
            border: 1px solid #ced4da; /* Borde gris claro */
            transition: border-color 0.3s ease; /* Transición suave al enfocar */
        }

            .form-control:focus {
                border-color: #007bff; /* Cambia el color del borde al enfocar */
                box-shadow: 0 0 5px rgba(0, 123, 255, 0.5); /* Sombra al enfocar */
            }


        .btn {
            padding: 8px 20px;
            border-radius: 10px; /* Bordes redondeados en los botones */
            overflow: hidden;
            /*transition: background-color 0.3s ease, transform 0.2s;*/ /* Transiciones para el botón */
        }

            .btn::before {
                position: absolute;
                content: "";
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                background: linear-gradient(120deg, transparent, var(--primary-color), trasparent);
                transition: 0.6s;
            }

            .btn:hover {
                background: transparent;
                box-shadow: 0 0 20px 10px rgba(51,152,219,0.5);
                background-color: #0056b3; /* Color de fondo al pasar el mouse */
                transform: translateY(-2px); /* Efecto de elevar el botón */
            }

        .text-danger {
            font-weight: 400; /* Peso de la fuente para errores */
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-5">
                <div class="card">
                    <div class="card-header text-center">
                        <h3>Login</h3>
                    </div>
                    <div class="card-body">
                        <asp:Panel ID="LoginPanel" runat="server">
                            <div class="form-group">
                                <label for="username">Nombre de Usuario</label>
                                <asp:TextBox ID="txtUsername" CssClass="form-control" runat="server" placeholder="Usuario" />
                                <asp:RequiredFieldValidator
                                    ID="rfvUsername"
                                    runat="server"
                                    ControlToValidate="txtUsername"
                                    ErrorMessage="El nombre de usuario es requerido"
                                    CssClass="text-danger"
                                    Display="Dynamic" />
                            </div>
                            <div class="form-group mt-3">
                                <label for="password">Contraseña</label>
                                <asp:TextBox ID="txtPassword" CssClass="form-control" runat="server" TextMode="Password" placeholder="Contraseña" />
                                <asp:RequiredFieldValidator
                                    ID="rfvPassword"
                                    runat="server"
                                    ControlToValidate="txtPassword"
                                    ErrorMessage="La contraseña es requerida"
                                    CssClass="text-danger"
                                    Display="Dynamic" />
                            </div>
                            <asp:Label ID="lblError" runat="server" CssClass="text-danger" Visible="False" />
                            <div class="d-grid gap-2 mt-4">
                                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary btn-block" Text="Iniciar sesión" OnClick="btnSubmit_Click" />
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
