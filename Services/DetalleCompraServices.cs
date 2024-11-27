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
    public class DetalleCompraService
    {
        private DataBaseAccess DB = new DataBaseAccess();

        public DataTable list(int IdCompra)
        {
            DataTable table = new DataTable();
            try
            {
                DB.setQuery("SELECT m.Nombre AS NombreMarca, p.Nombre AS NombreProducto, c.Nombre AS NombreCategoria, dc.Cantidad, dc.PrecioUnitario FROM DetalleCompra dc JOIN Producto p ON dc.IdProducto = p.IdProducto JOIN Marca m ON p.IdMarca = m.IdMarca JOIN Categoria c ON p.IdTipoProducto = c.IdCategoria WHERE dc.IdCompra = @IdCompra;");
                DB.setParameter("@IdCompra", IdCompra);
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

        public void add(int IdCompra, int IdProducto, int Cantidad, decimal PrecioUnitario)
        {
            try
            {
                DB.clearParameters();
                DB.setQuery("EXEC sp_InsertarDetalleCompra @IdCompra, @IdProducto, @Cantidad, @PrecioUnitario");

                DB.setParameter("@IdCompra", IdCompra);
                DB.setParameter("@IdProducto", IdProducto);
                DB.setParameter("@Cantidad", Cantidad);
                DB.setParameter("@PrecioUnitario", PrecioUnitario);

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

        public int getBuyQuantity(int IdComp)
        {
            try
            {
                int unitsQuantity = 0;
                DB.clearParameters();
                DB.setQuery("SELECT Cantidad FROM DetalleCompra WHERE IdCompra = @IdComp");

                DB.setParameter("@IdComp", IdComp);

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

        public int getProductId(int IdComp)
        {
            try
            {
                int idProducto = 0;
                DB.clearParameters();
                DB.setQuery("SELECT IdProducto FROM DetalleCompra WHERE IdCompra = @IdComp");

                DB.setParameter("@IdComp", IdComp);

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



