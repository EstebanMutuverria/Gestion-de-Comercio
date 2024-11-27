using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPC_equipo_9A
{
    public partial class ProveedorPage : System.Web.UI.Page
    {
        ProveedorServices proveedorService = new ProveedorServices();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int idProveedor = Convert.ToInt32(Request.QueryString["id"]);
                CargarProveedor(idProveedor);
            }
        }

        private void CargarProveedor(int idProveedor)
        {
            
            Proveedor proveedor = proveedorService.getProvider(idProveedor);

            if (proveedor != null)
            {
                hfIdProveedor.Value = proveedor.IdProveedor.ToString();
                txtNombre.Text = proveedor.Nombre;
                txtApellido.Text = proveedor.Apellido;
                txtDNI.Text = proveedor.DNI;
                txtCUIT.Text = proveedor.CUIT;
                txtCorreo.Text = proveedor.Correo;
                txtTelefono.Text = proveedor.Telefono;
                txtDireccion.Text = proveedor.Direccion;
            }
            else
            {
                Response.Redirect("ErrorPage.aspx");
            }

            if(proveedor.TipoPersona == "Fisica")
            {
                txtCUIT.Visible = false;
                lblCUIT.Visible = false;
                txtDNI.Enabled = false;
            }
            else
            {
                txtDNI.Visible = false;
                lblDNI.Visible = false;
                txtCUIT.Enabled = false;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
            Proveedor proveedor = new Proveedor
            {
                IdProveedor = Convert.ToInt32(hfIdProveedor.Value),
                Nombre = txtNombre.Text,
                Apellido = txtApellido.Text,
                DNI = txtDNI.Text,
                CUIT = txtCUIT.Text,
                Correo = txtCorreo.Text,
                Telefono = txtTelefono.Text,
                Direccion = txtDireccion.Text
            };

            ProveedorServices proveedorService = new ProveedorServices();
            proveedorService.modify(proveedor);

            Response.Redirect("RelacionesComerciales.aspx", false);

            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("RelacionesComerciales.aspx", false);
        }
    }
}