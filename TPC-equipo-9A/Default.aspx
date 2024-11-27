<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TPC_equipo_9A.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* Fondo celeste pastel */

        .container {
            max-width: 900px;
            margin: 50px auto;
            padding: 20px;
            background-color: rgba(255, 255, 255, 0.9); /* Fondo blanco con opacidad */
            border-radius: 10px;
            box-shadow: 0 6px 15px rgba(0, 0, 0, 0.1);
            animation: fadeIn 1.5s ease-out;
            position: relative;
            top: 100px; /* Ajusta este valor para moverlo más abajo */
        }


        h1 {
            font-family: 'Poppins', sans-serif;
            font-size: 3rem;
            font-weight: bold;
            color: #333;
            text-align: center;
            margin-bottom: 15px;
            text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.3);
        }

        h3 {
            font-family: 'Roboto', sans-serif;
            font-size: 1.3rem;
            font-weight: 400;
            color: #333;
            text-align: center;
            margin-bottom: 30px;
            line-height: 1.8;
        }

        /* Animación para la entrada de los elementos */
        @keyframes fadeIn {
            from {
                opacity: 0;
                transform: translateY(20px);
            }

            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        @media (max-width: 768px) {
            .container {
                padding: 15px;
                margin: 20px;
            }

            h1 {
                font-size: 2rem;
            }

            h3 {
                font-size: 1rem;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="container-content">
            <h1>
                <asp:Literal ID="litBienvenida" runat="server" />
            </h1>
            <h3>¡Bienvenido/a a la Plataforma de Gestión de Comercio!<br />
                Estamos listos para ayudarte a gestionar tus productos, pedidos y clientes de forma rápida y eficiente.<br />
                ¿Listo para optimizar tu negocio? Explora nuestras herramientas y encuentra todo lo que necesitas para llevar tu comercio al siguiente nivel.
            </h3>

        </div>
    </div>
</asp:Content>
