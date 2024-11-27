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
    public partial class RelacionesComerciales : System.Web.UI.Page
    {
        ProveedorServices serviceProveedor = new ProveedorServices();
        ClienteServices serviceCliente = new ClienteServices();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid(string filters = null)
        {
            try
            {
                List<Proveedor> proveedores = new List<Proveedor>();
                List<Cliente> clientes = new List<Cliente>();

                string tipoRelacion = ddlTipoRelacion.SelectedValue;

                if (!string.IsNullOrEmpty(tipoRelacion))
                {
                    if (tipoRelacion == "Proveedor")
                    {
                        proveedores = serviceProveedor.listar(filters);
                    }
                    else if (tipoRelacion == "Cliente")
                    {
                        clientes = serviceCliente.listar(filters);
                    }
                }
                else
                {
                    proveedores = serviceProveedor.listar(filters);
                    clientes = serviceCliente.listar(filters);
                }


                var relaciones = new List<object>();

                relaciones.AddRange(proveedores.Select(proveedor => new
                {
                    IdRelacion = proveedor.IdProveedor,
                    Nombre = proveedor.Nombre,
                    Apellido = proveedor.Apellido,
                    Correo = proveedor.Correo,
                    Telefono = proveedor.Telefono,
                    Direccion = proveedor.Direccion,
                    DNI = proveedor.DNI,
                    CUIT = proveedor.CUIT,
                    Relacion = "Proveedor",
                    Estado = proveedor.Estado
                }));

                relaciones.AddRange(clientes.Select(cliente => new
                {
                    IdRelacion = cliente.IdCliente,
                    Nombre = cliente.Nombre,
                    Apellido = cliente.Apellido,
                    Correo = cliente.Correo,
                    Telefono = cliente.Telefono,
                    Direccion = cliente.Direccion,
                    DNI = cliente.DNI,
                    CUIT = cliente.CUIT,
                    Relacion = "Cliente",
                    Estado = cliente.Estado
                }));

                gvRelaciones.DataSource = relaciones;
                gvRelaciones.DataBind();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }


        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = txtNombreRelacion.Text;
                string dni_cuit = txtDNICUIT.Text;
                string tipoRelacion = ddlTipoRelacion.SelectedValue.ToString();

                string filters = "";

                if (nombre != "")
                {
                    filters += $"Nombre LIKE '%{nombre}%'";
                }

                if (dni_cuit != "")
                {
                    if (filters != "")
                    {
                        filters += " and ";
                    }

                    filters += $"(DNI LIKE '%{dni_cuit}%' OR CUIT LIKE '%{dni_cuit}%')";
                }

                ViewState["filters"] = filters;
                BindGrid(filters);
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void btnAgregarRelacion_Click(object sender, EventArgs e)
        {
            Response.Redirect("AgregarRelacion.aspx", false);
        }

        protected void gvRelaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRelaciones.PageIndex = e.NewPageIndex;

            string filters = ViewState["filters"] as string;

            BindGrid(filters);
        }

        protected void gvRelaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] argumentos = e.CommandArgument.ToString().Split(',');


            int IdRelacion = int.Parse(argumentos[0]);
            string relacionTipo = "";
            if (argumentos.Length >= 2)
            {
                relacionTipo = argumentos[1];
            }


            string page;
            object service;
            if (relacionTipo == "Proveedor")
            {
                service = new ProveedorServices();
                page = "ProveedorPage";
            }
            else
            {
                service = new ClienteServices();
                page = "ClientePage";
            }

            switch (e.CommandName)
            {
                case "Editar":
                    Response.Redirect($"{page}.aspx?id={IdRelacion}", false);
                    break;

                case "Eliminar":
                    if (service is ProveedorServices proveedorService)
                    {
                        bool withoutBuys = proveedorService.verifyBuys(IdRelacion);

                        if (withoutBuys)
                        {
                            proveedorService.delete(IdRelacion);
                        }
                        else
                        {
                            string script = "alert('El Proveedor tiene Productos u Operaciones asociadas. De ser necesario, desactívelo.');";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", script, true);
                            return;
                        }
                    }
                    else if (service is ClienteServices clienteService)
                    {
                        bool withoutSells = clienteService.verifySells(IdRelacion);

                        if (withoutSells)
                        {
                            clienteService.delete(IdRelacion);
                        }
                        else
                        {
                            string script = "alert('El Cliente tiene Ventas asociadas. De ser necesario, desactívelo.');";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", script, true);
                            return;
                        }
                    }
                    Response.Redirect("RelacionesComerciales.aspx", false);
                    break;

                case "Estado":
                    Button btnEstado = (Button)e.CommandSource;

                    if (btnEstado.Text == "Activar")
                    {
                        if (service is ProveedorServices proveedorServiceEstado)
                        {
                            proveedorServiceEstado.setEstado(true, IdRelacion);
                        }
                        else if (service is ClienteServices clienteServiceEstado)
                        {
                            clienteServiceEstado.setEstado(true, IdRelacion);
                        }
                    }
                    else if (btnEstado.Text == "Desactivar")
                    {
                        if (service is ProveedorServices proveedorServiceEstado)
                        {
                            proveedorServiceEstado.setEstado(false, IdRelacion);
                        }
                        else if (service is ClienteServices clienteServiceEstado)
                        {
                            clienteServiceEstado.setEstado(false, IdRelacion);
                        }
                    }
                    break;

                default:
                    break;
            }
            BindGrid();
        }

    }
}