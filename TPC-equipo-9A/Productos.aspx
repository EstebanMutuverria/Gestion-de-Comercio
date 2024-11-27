<%@ Page Title="Inventario" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" Inherits="TPC_equipo_9A.Productos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .container {
            max-width: 1200px;
            margin: 0 auto;
            padding: 20px;
        }

        .table-custom {
            width: 80%; /* Ajusta el ancho */
            margin: 20px auto; /* Centra la tabla horizontalmente */
            border-collapse: collapse;
            font-size: 18px;
            background-color: #f9f9f9;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            border-radius: 8px;
            overflow: hidden;
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
            background-color: #e0e0e0;
            cursor: pointer;
        }

        h1 {
            font-family: Rockwell, sans-serif;
            text-align: center;
            font-size: 3em;
            color: #333;
            font-weight:bold;
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
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="container">
        <h1>Listado de productos</h1>
        <div class="search-bar row mb-3 justify-content-center">
            <div class="col-md-6">
                <div class="input-group">
                    <asp:TextBox ID="txtBuscar" CssClass="form-control" runat="server" Placeholder="Buscar producto, marca o categoría..."></asp:TextBox>
                    <asp:Button ID="btnBuscar" CssClass="btn btn-primary" Text="Buscar" OnClick="btnBuscar_Click" runat="server" />
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:GridView ID="dgvProductos" runat="server" OnSelectedIndexChanged="dgvProductos_SelectedIndexChanged" DataKeyNames="IdProducto" CssClass="table-custom table-hover" AutoGenerateColumns="False" AllowPaging="True" PageSize="6" OnPageIndexChanging="dgvProductos_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                        <asp:BoundField DataField="StockMinimo" HeaderText="Stock Mínimo" />
                        <asp:BoundField DataField="StockActual" HeaderText="Stock Actual" />
                        <asp:BoundField DataField="Categoria.Nombre" HeaderText="Categoría" />
                        <asp:BoundField DataField="Marca.Nombre" HeaderText="Marca" />
                        <asp:BoundField DataField="PorcentajeGanancia" HeaderText="Porcentaje Ganancia" Visible="false" />
                        <asp:BoundField DataField="FechaVencimiento" HeaderText="Fecha de vencimiento" Visible="false" />
                        <asp:BoundField DataField="Precio" HeaderText="Precio" Visible="true" />
                        <asp:TemplateField HeaderText="Ver detalle">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnVerDetalle" runat="server" CommandName="Select">
                                    <i class="fa fa-eye"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="dgvProductos" EventName="PageIndexChanging" />
            </Triggers>
        </asp:UpdatePanel>
        <div style="text-align: center; margin-top: 20px;">
            <asp:Button ID="btnAgregarProducto" runat="server" CssClass="btn btn-success mt-3" Text="Agregar" OnClick="btnAgregarProducto_Click" />
        </div>
    </div>
</asp:Content>
