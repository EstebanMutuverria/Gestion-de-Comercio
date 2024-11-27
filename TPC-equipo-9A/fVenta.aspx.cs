using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPC_equipo_9A
{
    public partial class fVenta : System.Web.UI.Page
    {
        private VentaServices ventaServices = new VentaServices();
        private DetalleVentaServices detalleVentaServices = new DetalleVentaServices();
        private ClienteServices clienteServices = new ClienteServices();
        private ProductoServices productoServices = new ProductoServices();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Session.Add("listaVenta", ventaServices.listar());
                    gvVentas.DataSource = Session["listaVenta"];
                    gvVentas.DataBind();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnDescargarPDF_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int idVenta = (Session["IdVentaSeleccionada"] != null) ? (int)Session["IdVentaSeleccionada"] : 0;

            int clientId = (idVenta == 0) ? 0 : ventaServices.getClienteIdVenta(idVenta);
            string numFac = (idVenta == 0) ? "Fac-000" : ventaServices.getNroFacturaVenta(idVenta);
            string fechaVenta = (idVenta == 0) ? DateTime.Now.ToString("dd/MM/yyyy") : ventaServices.getFechaVenta(idVenta);

            Cliente client = (idVenta == 0) ? null : clienteServices.getClient(clientId);
            string clientName = (client == null) ? "No reconocido" : $"{client.Nombre} {(string.IsNullOrEmpty(client.Apellido) ? "" : client.Apellido)}";


            Document pdfDoc = new Document(PageSize.A4, 25, 25, 30, 30);

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", $"attachment;filename={numFac}.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            try
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();

                Font titleFont = FontFactory.GetFont("Arial", 16, Font.BOLD);
                Paragraph title = new Paragraph("Factura de Venta", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                pdfDoc.Add(title);
                pdfDoc.Add(new Paragraph(" "));

                Font headerFont = FontFactory.GetFont("Arial", 12, Font.NORMAL);
                pdfDoc.Add(new Paragraph($"Fecha: {fechaVenta}", headerFont));
                pdfDoc.Add(new Paragraph($"Cliente: {clientName}", headerFont));
                pdfDoc.Add(new Paragraph($"Número de Factura: {numFac}", headerFont));
                pdfDoc.Add(new Paragraph(" "));

                PdfPTable table = new PdfPTable(6);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 20, 15, 15, 10, 15, 15 });

                Font headerCellFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
                table.AddCell(new PdfPCell(new Phrase("Producto", headerCellFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Marca", headerCellFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Categoría", headerCellFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Cantidad", headerCellFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Precio Unitario", headerCellFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Total", headerCellFont)) { HorizontalAlignment = Element.ALIGN_CENTER });

                Font rowFont = FontFactory.GetFont("Arial", 11, Font.NORMAL);
                foreach (GridViewRow row in gvDetalleVenta.Rows)
                {
                    table.AddCell(new PdfPCell(new Phrase(row.Cells[0].Text, rowFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase(row.Cells[1].Text, rowFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase(row.Cells[2].Text, rowFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase(row.Cells[3].Text, rowFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase(row.Cells[4].Text, rowFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase(row.Cells[5].Text, rowFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                }

                pdfDoc.Add(table);

                pdfDoc.Close();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
            finally
            {
                Response.End();
            }
        }


        protected void btnVerDetalle_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                int idVenta = int.Parse(btn.CommandArgument);
                Session["IdVentaSeleccionada"] = idVenta;

                DataTable detalles = detalleVentaServices.list(idVenta);

                if (detalles.Rows.Count == 0)
                {
                    LblError.Text = "No se encontraron detalles para esta compra.";
                    LblError.Visible = true;
                    gvDetalleVenta.Visible = false;
                }
                else
                {
                    gvDetalleVenta.DataSource = detalles;
                    gvDetalleVenta.DataBind();
                    gvDetalleVenta.Visible = true;
                    LblError.Visible = false;
                }

                ScriptManager.RegisterStartupScript(this, GetType(), "showModal", "$('#modalDetalleVenta').modal('show');", true);
            }
            catch (Exception ex)
            {
                LblError.Text = "Error al cargar detalles: " + ex.Message;
                LblError.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "showModal", "$('#modalDetalleVenta').modal('show');", true);
            }
        }

        protected void btnGenerarVenta_Click(object sender, EventArgs e)
        {
            cargarDropDownLists();
            ScriptManager.RegisterStartupScript(this, GetType(), "showStaticModal", "$('#staticBackdrop').modal('show');", true);
        }

        protected void btnAceptarGenerarVenta_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ddlCliente.SelectedValue))
                {
                    lblErrorMessage.Text = "Por favor seleccione un cliente.";
                    lblErrorMessage.Visible = true;
                    return;
                }

                if (string.IsNullOrEmpty(txtFechaVenta.Value))
                {
                    lblErrorMessage.Text = "Por favor seleccione una fecha de venta.";
                    lblErrorMessage.Visible = true;
                    return;
                }

                if (string.IsNullOrEmpty(ddlProducto.SelectedValue))
                {
                    lblErrorMessage.Text = "Por favor seleccione un producto.";
                    lblErrorMessage.Visible = true;
                    return;
                }

                if (string.IsNullOrEmpty(txtCantidad.Text))
                {
                    lblErrorMessage.Text = "Por favor ingrese una cantidad.";
                    lblErrorMessage.Visible = true;
                    return;
                }

                int IdProveedor = int.Parse(ddlCliente.SelectedValue);
                string fechaInput = txtFechaVenta.Value;
                bool Estado = true;

                DateTime fechaVenta;
                if (!DateTime.TryParseExact(fechaInput, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out fechaVenta))
                {
                    Response.Write("El formato de la fecha es incorrecto. Por favor ingrese una fecha válida.");
                    return;
                }

                int quantity = int.Parse(txtCantidad.Text);
                int idProducto = int.Parse(ddlProducto.SelectedValue);
                int stockProducto = productoServices.getStock(idProducto);
                int stockMinimo = productoServices.getMinStock(idProducto);
                int stockActual = stockProducto - quantity;

                if (stockActual < stockMinimo)
                {
                    string script = "alert('No es posible generar la venta. El Stock resultante sería menor al stock mínimo permitido.');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", script, true);
                    clearFields();
                    return;
                }

                int IdVenta = ventaServices.add(IdProveedor, fechaVenta, Estado);


                int IdProducto = int.Parse(ddlProducto.SelectedValue);
                int Cantidad = int.Parse(txtCantidad.Text);

                detalleVentaServices.add(IdVenta, IdProducto, Cantidad);

                gvVentas.DataSource = ventaServices.listar();
                gvVentas.DataBind();

                clearFields();

                ScriptManager.RegisterStartupScript(this, GetType(), "closeModal", "$('#staticBackdrop').modal('hide');", true);
            }
            catch (Exception ex)
            {
                LblError.Text = "Error al agregar la compra: " + ex.Message;
                LblError.Visible = true;
            }
        }

        protected void btnCerrar_ServerClick(object sender, EventArgs e)
        {
            clearFields();
        }

        protected void btnX_ServerClick(object sender, EventArgs e)
        {
            clearFields();
        }

        private void cargarDropDownLists()
        {
            ddlCliente.Items.Clear();
            ddlCliente.DataSource = clienteServices.listar("Estado=1");
            ddlCliente.DataTextField = "Nombre";
            ddlCliente.DataValueField = "IdCliente";
            ddlCliente.DataBind();
            ddlCliente.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar...", ""));


            ddlProducto.Items.Clear();
            ddlProducto.DataSource = productoServices.listarProductoVenta();
            ddlProducto.DataTextField = "Nombre";
            ddlProducto.DataValueField = "IdProducto";
            ddlProducto.DataBind();
            ddlProducto.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar...", ""));

        }

        private void clearFields()
        {
            ddlCliente.SelectedIndex = 0;
            txtFechaVenta.Value = string.Empty;
            ddlProducto.SelectedIndex = 0;
            txtCantidad.Text = string.Empty;
            lblErrorMessage.Text = "";
            lblErrorMessage.Visible = false;
        }

        protected void chkVerificacion_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = (CheckBox)sender;
                GridViewRow row = (GridViewRow)chk.NamingContainer;
                int idVenta = Convert.ToInt32(gvVentas.DataKeys[row.RowIndex].Value);

                bool nuevoEstado = chk.Checked;
                

                int quantity = detalleVentaServices.getSellQuantity(idVenta);
                int idProducto = detalleVentaServices.getProductId(idVenta);
                int stockProducto = productoServices.getStock(idProducto);
                int stockMinimo = productoServices.getMinStock(idProducto);
                int stockActual = nuevoEstado ? (stockProducto - quantity) : (stockProducto + quantity);
                
                if (stockActual < stockMinimo)
                {
                    string script = "alert('No es posible confirmar la venta. El Stock resultante sería menor al stock mínimo permitido.');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", script, true);
                    chk.Checked = false;
                    return;
                }
                else{
                    ventaServices.ActualizarEstadoVenta(idVenta, nuevoEstado ? 1 : 0);

                    Label lblEstado = (Label)row.FindControl("lblEstado");
                    lblEstado.Text = nuevoEstado ? "Confirmada" : "Anulada";
                    productoServices.updateStock(idProducto, stockActual);
                }
            }
            catch (Exception ex)
            {
                LblError.Text = "Error al actualizar el estado de la Venta: " + ex.Message;
                LblError.Visible = false;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string filtro = txtBuscar.Text.Trim();
                List<Venta> listaFiltrada = ventaServices.listar(filtro);
                gvVentas.DataSource = listaFiltrada;
                gvVentas.DataBind();
            }
            catch (Exception ex)
            {
                LblError.Text = "Error al realizar la búsqueda: " + ex.Message;
                LblError.Visible = true;
            }
        }

        public static string RemoveAccents(string text)
        {
            var withAccents = "áéíóúÁÉÍÓÚüÜñÑ";
            var withoutAccents = "aeiouAEIOUuUnN";

            for (int i = 0; i < withAccents.Length; i++)
            {
                text = text.Replace(withAccents[i], withoutAccents[i]);
            }

            return text;
        }
        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            btnBuscar_Click(sender, e);
        }

        protected void gvVentas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvVentas.PageIndex = e.NewPageIndex;
                if (Session["listaVenta"] != null)
                {
                    gvVentas.DataSource = Session["listaVenta"];
                    gvVentas.DataBind();
                }
            }
            catch (Exception ex)
            {
                LblError.Text = "Error al cambiar de página: " + ex.Message;
                LblError.Visible = true;
            }
        }
    }

}
