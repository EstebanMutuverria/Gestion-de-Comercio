using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPC_equipo_9A
{
    public partial class AgregarRelacion : System.Web.UI.Page
    {
        ProveedorServices serviceProveedor = new ProveedorServices();
        ClienteServices serviceCliente = new ClienteServices();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDNI.Text = "";
                txtCUIT.Text = "";
            }
            else
            {
                UpdateFieldVisibility();
            }
        }

        protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFieldVisibility();
        }

        private void UpdateFieldVisibility()
        {
            string tipoRelacion = ddlTipoRelacion.SelectedValue;
            string tipoPersona = ddlTipoPersona.SelectedValue;

            txtDNI.Enabled = false;
            lblDNI.Visible = true;
            rfvDNI.Enabled = false;

            txtCUIT.Enabled = false;
            lblCUIT.Visible = true;
            rfvCUIT.Enabled = false;

            string currentClass = DatosPersonales.Attributes["class"];
            if (tipoRelacion != "" && tipoPersona != "")
            {
                PersonFields.Attributes.Remove("class");

                if (!string.IsNullOrEmpty(currentClass))
                {
                    currentClass = currentClass.Replace("hidden", "").Trim();
                    DatosPersonales.Attributes["class"] = currentClass;
                }

                if (tipoPersona == "Fisica")
                {
                    txtDNI.Enabled = true;
                    txtCUIT.Text = "";
                    rfvDNI.Enabled = true;
                }
                else
                {
                    txtCUIT.Enabled = true;
                    txtDNI.Text = "";
                    rfvCUIT.Enabled = true;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(currentClass) || !currentClass.Contains("hidden"))
                {
                    DatosPersonales.Attributes["class"] += " hidden";
                }
            }

        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (ddlTipoRelacion.SelectedValue == "Cliente")
                {
                    Cliente cliente = new Cliente
                    {
                        Nombre = txtNombre.Text,
                        Apellido = txtApellido.Text,
                        Correo = txtCorreo.Text,
                        Telefono = txtTelefono.Text,
                        Direccion = txtDireccion.Text,
                        TipoPersona = ddlTipoPersona.SelectedValue,
                        DNI = txtDNI.Text,
                        CUIT = txtCUIT.Text
                    };

                    serviceCliente.add(cliente);
                }
                else if (ddlTipoRelacion.SelectedValue == "Proveedor")
                {
                    Proveedor proveedor = new Proveedor
                    {
                        Nombre = txtNombre.Text,
                        Apellido = txtApellido.Text,
                        Correo = txtCorreo.Text,
                        Telefono = txtTelefono.Text,
                        Direccion = txtDireccion.Text,
                        TipoPersona = ddlTipoPersona.SelectedValue.ToString(),
                        DNI = txtDNI.Text,
                        CUIT = txtCUIT.Text
                    };

                    serviceProveedor.add(proveedor);
                }

                Response.Redirect("RelacionesComerciales.aspx", false);
            }
        }

        protected void ValidateDNI(object source, ServerValidateEventArgs args)
        {
            string DNI = args.Value;
            string tipoRelacion = ddlTipoRelacion.SelectedValue;
            bool isAvailableInCliente = serviceCliente.DNIAvailable(DNI.ToString());
            bool isAvailableInProveedor = serviceProveedor.DNIAvailable(DNI.ToString());

            args.IsValid = isAvailableInCliente && isAvailableInProveedor;
        }

        protected void ValidateCUIT(object source, ServerValidateEventArgs args)
        {
            string CUIT = args.Value;
            string tipoRelacion = ddlTipoRelacion.SelectedValue;
            bool isAvailableInCliente = serviceCliente.CUITAvailable(CUIT.ToString());
            bool isAvailableInProveedor = serviceProveedor.CUITAvailable(CUIT.ToString());

            args.IsValid = isAvailableInCliente && isAvailableInProveedor;
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("RelacionesComerciales.aspx", false);
        }



    }
}