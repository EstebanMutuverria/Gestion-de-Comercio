<%@ Page Title="Error" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="TPC_equipo_9A.Error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .error-container {
            text-align: center;
            margin-top: 50px;
        }
        .error-icon {
            font-size: 100px;
            color: #dc3545;
        }
        .error-message {
            font-size: 24px;
            color: #dc3545;
        }
        .back-home {
            margin-top: 20px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container error-container">
        <div class="row justify-content-center">
            <div class="col-md-8">
                <div class="card">
                    <div class="card-body">
                        <div class="error-icon">
                            <i class="fas fa-exclamation-circle"></i>
                        </div>
                        <h2 class="error-message">Oops! Algo salió mal.</h2>
                        <p>Lo sentimos, ha ocurrido un error inesperado. Por favor, comuníquese con el Soporte.</p>

                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
                        
                        <div class="back-home">
                            <asp:Button ID="btnBackHome" runat="server" CssClass="btn btn-primary" Text="Volver al inicio" OnClick="btnBackHome_Click"/>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
