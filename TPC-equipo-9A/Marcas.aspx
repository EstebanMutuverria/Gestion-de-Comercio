<%@ Page Title="Inventario" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Marcas.aspx.cs" Inherits="TPC_equipo_9A.Marcas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .container {
            max-width: 1200px;
            margin: 0 auto;
            padding: 20px;
        }

        .table-custom {
            width: 80% !important; /* Ajusta el ancho según tus necesidades */
            max-width: 600px; /* Limitar el tamaño máximo a 600px */
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
            font-size: 3em;
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
            width:80%;
            margin: 20px auto !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="container">
        <h1>Listado de marcas</h1>
        <div class="search-bar row mb-3 justify-content-center">
            <div class="col-md-6">
                <div class="input-group">
                    <asp:TextBox ID="txtBuscar" CssClass="form-control" runat="server" Placeholder="Buscar marca..."></asp:TextBox>
                    <asp:Button ID="btnBuscar" CssClass="btn btn-primary" Text="Buscar" OnClick="btnBuscar_Click" runat="server" />
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:GridView ID="dgvMarca" runat="server" OnSelectedIndexChanged="dgvMarca_SelectedIndexChanged" DataKeyNames="IdMarca" CssClass="table-custom" AutoGenerateColumns="false" AllowPaging="True" PageSize="5" OnPageIndexChanging="dgvMarca_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" ItemStyle-Width="50%" />
                        <asp:TemplateField HeaderText="Ver detalle" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50%">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnVerDetalle" runat="server" CommandName="Select" CommandArgument='<%# Eval("IdMarca") %>'>
                                    <i class="fa fa-eye"></i> <!-- Ícono de ojo -->
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="dgvMarca" EventName="PageIndexChanging" />
            </Triggers>
        </asp:UpdatePanel>
        <div style="text-align: center; margin-top: 20px;">
            <asp:Button ID="btnAgregarMarca" runat="server" CssClass="btn btn-success mt-3" Text="Agregar" OnClick="btnAgregarMarca_Click" />
        </div>
    </div>
</asp:Content>
