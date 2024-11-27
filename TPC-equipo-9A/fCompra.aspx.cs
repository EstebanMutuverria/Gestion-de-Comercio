using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPC_equipo_9A
{
    public partial class fCompra : System.Web.UI.Page
    {
        private CompraServices compraServices = new CompraServices();
        private DetalleCompraService detalleCompraService = new DetalleCompraService();
        private ProveedorServices proveedorServices = new ProveedorServices();
        private MarcaServices marcaServices = new MarcaServices();
        private CategoriaServices categoriaServices = new CategoriaServices();
        private ProductoServices productoServices = new ProductoServices();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Session.Add("ListaCompra", compraServices.listar());
                    gvCompras.DataSource = Session["ListaCompra"];
                    gvCompras.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnVerDetalle_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                int idCompra = int.Parse(btn.CommandArgument);

                DataTable detalles = detalleCompraService.list(idCompra);

                if (detalles.Rows.Count == 0)
                {
                    LblError.Text = "No se encontraron detalles para esta compra.";
                    LblError.Visible = true;
                    gvDetalleCompra.Visible = false;
                }
                else
                {
                    gvDetalleCompra.DataSource = detalles;
                    gvDetalleCompra.DataBind();
                    gvDetalleCompra.Visible = true;
                    LblError.Visible = false;
                }

                ScriptManager.RegisterStartupScript(this, GetType(), "showModal", "$('#modalDetalleCompra').modal('show');", true);
            }
            catch (Exception ex)
            {
                LblError.Text = "Error al cargar detalles: " + ex.Message;
                LblError.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "showModal", "$('#modalDetalleCompra').modal('show');", true);
            }
        }

        protected void btnGenerarCompra_Click(object sender, EventArgs e)
        {
            cargarDropDownLists();
            ScriptManager.RegisterStartupScript(this, GetType(), "showStaticModal", "$('#staticBackdrop').modal('show');", true);
        }

        protected void btnAceptarGenerarCompra_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ddlProveedor.SelectedValue))
                {
                    lblErrorMessage.Text = "Por favor seleccione un proveedor.";
                    lblErrorMessage.Visible = true;
                    return;
                }

                if (string.IsNullOrEmpty(txtFechaCompra.Value))
                {
                    lblErrorMessage.Text = "Por favor seleccione una fecha de compra.";
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

                if (string.IsNullOrEmpty(txtPrecioUnitario.Text))
                {
                    lblErrorMessage.Text = "Por favor ingrese un precio unitario.";
                    lblErrorMessage.Visible = true;
                    return;
                }

                lblErrorMessage.Text = "";
                lblErrorMessage.Visible = false;

                bool Estado = true;
                int IdProveedor = int.Parse(ddlProveedor.SelectedValue);
                string fechaInput = txtFechaCompra.Value;

                DateTime fechaCompra;
                if (!DateTime.TryParseExact(fechaInput, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out fechaCompra))
                {
                    lblErrorMessage.Text = "El formato de la fecha es incorrecto. Por favor ingrese una fecha válida.";
                    lblErrorMessage.Visible = true;
                    return;
                }

                int IdCompra = compraServices.add(IdProveedor, fechaCompra, Estado);

                int IdProducto = int.Parse(ddlProducto.SelectedValue);
                int Cantidad = int.Parse(txtCantidad.Text);
                decimal PrecioUnitario = decimal.Parse(txtPrecioUnitario.Text);

                detalleCompraService.add(IdCompra, IdProducto, Cantidad, PrecioUnitario);

                gvCompras.DataSource = compraServices.listar();
                gvCompras.DataBind();

                clearFields();

                ScriptManager.RegisterStartupScript(this, GetType(), "closeModal", "$('#staticBackdrop').modal('hide');", true);
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = "Error al agregar la compra: " + ex.Message;
                lblErrorMessage.Visible = true;
            }
        }

        protected void btnCerrar_ServerClick(object sender, EventArgs e)
        {
            clearFields();
            ScriptManager.RegisterStartupScript(this, GetType(), "closeModal", "$('#staticBackdrop').modal('hide');", true);
        }

        protected void btnX_ServerClick(object sender, EventArgs e)
        {
            clearFields();
            ScriptManager.RegisterStartupScript(this, GetType(), "closeModal", "$('#staticBackdrop').modal('hide');", true);
        }

        private void cargarDropDownLists()
        {
            ddlProveedor.Items.Clear();
            ddlProveedor.DataSource = proveedorServices.listar("Estado=1");
            ddlProveedor.DataTextField = "Nombre";
            ddlProveedor.DataValueField = "IdProveedor";
            ddlProveedor.DataBind();
            ddlProveedor.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar...", ""));

            ddlProducto.Items.Clear();
            ddlProducto.DataSource = productoServices.listar();
            ddlProducto.DataTextField = "Nombre";
            ddlProducto.DataValueField = "IdProducto";
            ddlProducto.DataBind();
            ddlProducto.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar...", ""));
        }

        private void clearFields()
        {
            ddlProveedor.SelectedIndex = 0;
            txtFechaCompra.Value = string.Empty;
            ddlProducto.SelectedIndex = 0;
            txtCantidad.Text = string.Empty;
            txtPrecioUnitario.Text = string.Empty;
            lblErrorMessage.Text = "";
            lblErrorMessage.Visible = false;
        }

        protected void chkVerificacion_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = (CheckBox)sender;
                GridViewRow row = (GridViewRow)chk.NamingContainer;
                int idCompra = Convert.ToInt32(gvCompras.DataKeys[row.RowIndex].Value);

                bool nuevoEstado = chk.Checked;


                int quantity = detalleCompraService.getBuyQuantity(idCompra);
                int idProducto = detalleCompraService.getProductId(idCompra);
                int stockProducto = productoServices.getStock(idProducto);
                int stockMinimo = productoServices.getMinStock(idProducto);
                int stockActual = nuevoEstado ? (stockProducto + quantity) : (stockProducto - quantity);

                
                if (stockActual < stockMinimo)
                {
                    string script = "alert('No es posible anular la compra. El Stock resultante sería menor al stock mínimo permitido.');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", script, true);
                    chk.Checked = true;
                    return;
                }
                else {
                    compraServices.ActualizarEstadoCompra(idCompra, nuevoEstado ? 1 : 0);

                    Label lblEstado = (Label)row.FindControl("lblEstado");
                    lblEstado.Text = nuevoEstado ? "Confirmada" : "Anulada";
                    productoServices.updateStock(idProducto, stockActual);
                }
            }
            catch (Exception ex)
            {
                LblError.Text = "Error al actualizar el estado de la compra: " + ex.Message;
                LblError.Visible = true;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string filtro = txtBuscar.Text.Trim();
                List<Compra> listaFiltrada = compraServices.listar(filtro);
                gvCompras.DataSource = listaFiltrada;
                gvCompras.DataBind();
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

        protected void gvCompras_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvCompras.PageIndex = e.NewPageIndex;
                if (Session["ListaCompra"] != null)
                {
                    gvCompras.DataSource = Session["ListaCompra"];
                    gvCompras.DataBind();
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

