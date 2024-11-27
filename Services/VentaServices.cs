using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using Models;
using System.Data;

namespace Services
{
    public class VentaServices
    {
        private DataBaseAccess DB = new DataBaseAccess();

        public List<Venta> listar(string filtro = "")
        {
            List<Venta> lista = new List<Venta>();

            try
            {
                string query = "SELECT V.IdVenta, C.Nombre AS NombreCliente, C.Apellido AS ApellidoCliente, " +
                               "C.Correo, V.FechaVenta, V.NumeroFactura, V.Estado " +
                               "FROM Venta V " +
                               "INNER JOIN Cliente C ON V.IdCliente = C.IdCliente";

                if (!string.IsNullOrEmpty(filtro))
                {
                    if (int.TryParse(filtro, out int idVenta))
                    {
                        query += " WHERE V.IdVenta = @Filtro";
                    }
                    else
                    {
                        query += " WHERE C.Nombre LIKE @Filtro " +
                                 "OR C.Apellido LIKE @Filtro " +
                                 "OR C.Correo LIKE @Filtro " +
                                 "OR CONVERT(VARCHAR, V.FechaVenta, 103) LIKE @Filtro " +
                                 "OR V.NumeroFactura LIKE @Filtro";
                    }
                }

                DB.setQuery(query);

                if (!string.IsNullOrEmpty(filtro))
                {
                    if (int.TryParse(filtro, out int idVenta))
                    {
                        DB.setParameter("@Filtro", idVenta);
                    }
                    else
                    {
                        DB.setParameter("@Filtro", $"%{filtro}%");
                    }
                }

                DB.excecuteQuery();

                while (DB.Reader.Read())
                {
                    Venta aux = new Venta();
                    aux.IdVenta = (int)DB.Reader["IdVenta"];
                    aux.Cliente = new Cliente();
                    aux.Cliente.Nombre = (string)DB.Reader["NombreCliente"];
                    aux.Cliente.Apellido = (string)DB.Reader["ApellidoCliente"];
                    aux.Cliente.Correo = (string)DB.Reader["Correo"];
                    aux.FechaVenta = (DateTime)DB.Reader["FechaVenta"];
                    aux.NumeroFactura = (string)DB.Reader["NumeroFactura"];
                    aux.Estado = (bool)DB.Reader["Estado"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DB.CloseConnection();
            }
        }
        public int add(int IdCliente, DateTime FechaVenta, bool Estado)
        {
            try
            {
                DB.clearParameters();
                DB.setQuery("EXEC sp_GenerarVenta @IdCliente, @FechaVenta, @Estado");
                DB.setParameter("@IdCliente", IdCliente);
                DB.setParameter("@FechaVenta", FechaVenta);
                DB.setParameter("@Estado", Estado);

                DB.excecuteQuery();

                int IdVenta = 0;
                if (DB.Reader.Read())
                {
                    IdVenta = Convert.ToInt32(DB.Reader["IdVenta"]);
                }
                return IdVenta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DB.CloseConnection();
            }
        }

        public void ActualizarEstadoVenta(int IdVenta, int nuevoEstado)
        {
            try
            {
                DB.clearParameters();
                DB.setQuery("UPDATE Venta SET Estado = @Estado WHERE IdVenta = @IdVenta");
                DB.setParameter("@Estado", nuevoEstado);
                DB.setParameter("@IdVenta", IdVenta);
                DB.excecuteAction();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DB.CloseConnection();
            }
        }

        public int getClienteIdVenta(int IdVent)
        {
            try
            {
                int idClient= 0;

                DB.clearParameters();
                DB.setQuery("SELECT IdCliente FROM Venta WHERE IdVenta = @IdVent");
                DB.setParameter("@IdVent", IdVent);
                DB.excecuteQuery();

                while (DB.Reader.Read())
                {
                    idClient = int.Parse(DB.Reader["IdCliente"].ToString());
                }
                return idClient;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FATAL ERROR: Error al obtener IdCliente. Comuníquese con el Soporte.\nDetalles: {ex.Message}");
                return 0;
            }
            finally
            {
                DB.CloseConnection();
            }
        }

        public string getNroFacturaVenta(int IdVent)
        {
            try
            {
                string numFactura = "FAC-000";

                DB.clearParameters();
                DB.setQuery("SELECT NumeroFactura FROM Venta WHERE IdVenta = @IdVent");
                DB.setParameter("@IdVent", IdVent);
                DB.excecuteQuery();

                while (DB.Reader.Read())
                {
                    numFactura = DB.Reader["NumeroFactura"].ToString();
                }
                return numFactura;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FATAL ERROR: Error al obtener IdCliente. Comuníquese con el Soporte.\nDetalles: {ex.Message}");
                return "FAC-000";
            }
            finally
            {
                DB.CloseConnection();
            }
        }

        public string getFechaVenta(int IdVent)
        {
            try
            {
                string fechaVenta = string.Empty;

                DB.clearParameters();
                DB.setQuery("SELECT FechaVenta FROM Venta WHERE IdVenta = @IdVent");
                DB.setParameter("@IdVent", IdVent);
                DB.excecuteQuery();

                while (DB.Reader.Read())
                {
                    if (DB.Reader["FechaVenta"] != DBNull.Value)
                    {
                        DateTime date = Convert.ToDateTime(DB.Reader["FechaVenta"]);
                        fechaVenta = date.ToString("dd/MM/yyyy");
                    }
                }

                return fechaVenta;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FATAL ERROR: Error al obtener FechaVenta. Comuníquese con el Soporte.\nDetalles: {ex.Message}");
                return string.Empty;
            }
            finally
            {
                DB.CloseConnection();
            }
        }
    }
}
