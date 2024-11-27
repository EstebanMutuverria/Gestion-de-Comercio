using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Utils;

namespace Services
{
   public class DetalleVentaServices
    {
        private DataBaseAccess DB = new DataBaseAccess();

        public DataTable list(int IdVenta)
        {
            DataTable table = new DataTable();
            try
            {
                DB.setQuery("SELECT p.Nombre AS NombreProducto, m.Nombre AS NombreMarca, c.Nombre AS NombreCategoria, dv.Cantidad, dv.PrecioUnitario, dv.PrecioTotal FROM DetalleVenta dv JOIN Producto p ON dv.IdProducto = p.IdProducto JOIN Marca m ON p.IdMarca = m.IdMarca JOIN Categoria c ON p.IdTipoProducto = c.IdCategoria WHERE dv.IdVenta = @IdVenta;");
                DB.setParameter("@IdVenta", IdVenta);
                DB.excecuteQuery();

                table.Load(DB.Reader);
                return table;
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

        public void add(int IdVenta, int IdProducto, int Cantidad)
        {
            try
            {
                DB.clearParameters();
                DB.setQuery("EXEC sp_GenerarDetalleVenta @IdCompra, @IdProducto, @Cantidad SELECT SCOPE_IDENTITY() AS IdVenta;");

                DB.setParameter("@IdCompra", IdVenta);
                DB.setParameter("@IdProducto", IdProducto);
                DB.setParameter("@Cantidad", Cantidad);
                //DB.setParameter("@PrecioUnitario", PrecioUnitario);

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

        public int getSellQuantity(int IdVent)
        {
            try
            {
                int unitsQuantity = 0;
                DB.clearParameters();
                DB.setQuery("SELECT Cantidad FROM DetalleVenta WHERE IdVenta = @IdVent");

                DB.setParameter("@IdVent", IdVent);

                DB.excecuteQuery();

                while (DB.Reader.Read())
                {
                    if (DB.Reader["Cantidad"] != DBNull.Value)
                    {
                        unitsQuantity = Convert.ToInt32(DB.Reader["Cantidad"]);
                    }
                }

                return unitsQuantity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FATAL ERROR: Error al obtener Cantidad. Comuníquese con el Soporte.\nDetalles: {ex.Message}");
                return 0;
            }
            finally
            {
                DB.CloseConnection();
            }
        }

        public int getProductId(int IdVent)
        {
            try
            {
                int idProducto = 0;
                DB.clearParameters();
                DB.setQuery("SELECT IdProducto FROM DetalleVenta WHERE IdVenta = @IdVent");

                DB.setParameter("@IdVent", IdVent);

                DB.excecuteQuery();

                while (DB.Reader.Read())
                {
                    if (DB.Reader["IdProducto"] != DBNull.Value)
                    {
                        idProducto = Convert.ToInt32(DB.Reader["IdProducto"]);
                    }
                }

                return idProducto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FATAL ERROR: Error al obtener ProductId. Comuníquese con el Soporte.\nDetalles: {ex.Message}");
                return 0;
            }
            finally
            {
                DB.CloseConnection();
            }
        }
    }
}
