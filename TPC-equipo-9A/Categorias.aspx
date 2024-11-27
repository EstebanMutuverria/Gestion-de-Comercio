<%@ Page Title="Inventario" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Categorias.aspx.cs" Inherits="TPC_equipo_9A.Categorias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .container {
            max-width: 1200px;
            margin: 0 auto;
            padding: 20px;
        }

        .table-custom {
            width: 80% !important; /* Ajusta el ancho */
            max-width: 600px; 
            margin: 20px auto !important; /* Centrando la tabla horizontalmente */
            border-collapse: collapse;
            font-size: 18px;
            background-color: #f9f9f9;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            border-radius: 8px; /* Bordes redondeados */
            overflow: hidden; /* Para asegurar que los bordes redondeados se vean correctamente */
        }

        .table-custom th, .table-custom td {
            padding: 12px 15px;
            border: 1px solid #dddddd;
            text-align: center;
        }

        .table-custom th {
            background-color: #4CAF50;
            color: white;
            font-weight: bold;
        }

        .table-custom tr:nth-child(even) {
            background-color: #f2f2f2;
        }

        .table-custom tr:hover {
            background-color: #e0e0e0; /* Color de resaltado al pasar el mouse */
            cursor: pointer;
        }

        h1 {
            font-family: Rockwell, sans-serif;
            text-align: center;
            font-size: 2em;
            color: #333;
        }

        .btn-success {
            background-color: #28a745;
            color: white;
            font-size: 16px;
            padding: 10px 20px;
            border-radius: 5px;
            transition: background-color 0.3s ease;
        }

        .btn:hover {
            transform: translateY(-2px);
        }

        .btn-success:hover {
            background-color: #218838;
        }

        .search-bar {
            margin-top: 20px;
            width:80%;
            margin: 20px auto !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="container">
        <h1>Listado de categorías</h1>
        <div class="search-bar row mb-3 justify-content-center">
            <div class="col-md-6">
                <div class="input-group">
                    <asp:TextBox ID="txtBuscar" CssClass="form-control" runat="server" Placeholder="Buscar categoría..."></asp:TextBox>
                    <asp:Button ID="btnBuscar" CssClass="btn btn-primary" Text="Buscar" OnClick="btnBuscar_Click" runat="server" />
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:GridView ID="dgvCategoria" runat="server" OnSelectedIndexChanged="dgvCategoria_SelectedIndexChanged" DataKeyNames="IdCategoria" CssClass="table-custom" AutoGenerateColumns="false" AllowPaging="True" PageSize="5" OnPageIndexChanging="dgvCategoria_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                        <asp:TemplateField HeaderText="Ver detalle" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnVerDetalle" runat="server" CommandName="Select" CommandArgument='<%# Eval("IdCategoria") %>'>
                                    <i class="fa fa-eye"></i> <!-- Ícono de ojo -->
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="dgvCategoria" EventName="PageIndexChanging" />
            </Triggers>
        </asp:UpdatePanel>
        <div style="text-align: center; margin-top: 20px;">
            <asp:Button ID="btnAgregarCategoria" runat="server" CssClass="btn btn-success mt-3" Text="Agregar" OnClick="btnAgregarCategoria_Click" />
        </div>
    </div>
</asp:Content>
