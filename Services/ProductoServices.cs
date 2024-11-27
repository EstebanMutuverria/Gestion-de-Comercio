using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using Models;
using System.Data.SqlClient;

namespace Services
{
    public class ProductoServices
    {
        private DataBaseAccess DB = new DataBaseAccess();

        public List<Producto> listar(string id = "")
        {
            List<Producto> list = new List<Producto>();
            try
            {
                if (id != "")
                {
                    DB.setQuery("SELECT * FROM VW_productosGrid WHERE IdProducto =" + id);
                }
                else
                {
                    DB.setQuery("SELECT * FROM VW_productosGrid");
                }
                DB.excecuteQuery();

                while (DB.Reader.Read())
                {
                    Producto producto = new Producto();

                    producto.IdProducto = (int)DB.Reader["IdProducto"];
                    producto.Nombre = (string)DB.Reader["Nombre"];
                    AsignarMarcaYCategoria(producto, DB.Reader);
                    producto.StockActual = (int)DB.Reader["StockActual"];
                    producto.StockMinimo = (int)DB.Reader["StockMinimo"];
                    producto.PorcentajeGanancia = (decimal)DB.Reader["PorcentajeGanancia"];
                    producto.FechaVencimiento = DB.Reader["FechaVencimiento"] != DBNull.Value ? (DateTime)DB.Reader["FechaVencimiento"] : (DateTime?)null;
                    producto.Precio = (decimal)DB.Reader["Precio"];

                    list.Add(producto);
                }

                return list;
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

        public List<Producto> listarProductoVenta(string id = "")
        {
            List<Producto> list = new List<Producto>();
            try
            {

                DB.setQuery("SELECT * FROM VW_productosGridDDL");
                DB.excecuteQuery();

                while (DB.Reader.Read())
                {
                    Producto producto = new Producto();

                    producto.IdProducto = (int)DB.Reader["IdProducto"];
                    producto.Nombre = (string)DB.Reader["Nombre"];
                    AsignarMarcaYCategoria(producto, DB.Reader);
                    producto.StockActual = (int)DB.Reader["StockActual"];
                    producto.StockMinimo = (int)DB.Reader["StockMinimo"];
                    producto.PorcentajeGanancia = (decimal)DB.Reader["PorcentajeGanancia"];
                    producto.FechaVencimiento = DB.Reader["FechaVencimiento"] != DBNull.Value ? (DateTime)DB.Reader["FechaVencimiento"] : (DateTime?)null;
                  

                    list.Add(producto);
                }

                return list;
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

        private void AsignarMarcaYCategoria(Producto producto, SqlDataReader reader)
        {
            producto.Categoria = new Categoria();
            if (!reader.IsDBNull(reader.GetOrdinal("IdCategoria")))
            {
                producto.Categoria.IdCategoria = (int)reader["IdCategoria"];
                producto.Categoria.Nombre = (string)reader["NombreCategoria"];
            }
            else
            {
                producto.Categoria.IdCategoria = 0;
                producto.Categoria.Nombre = string.Empty;
            }


            producto.Marca = new Marca();
            if (!reader.IsDBNull(reader.GetOrdinal("IdMarca")))
            {
                producto.Marca.IdMarca = (int)reader["IdMarca"];
                producto.Marca.Nombre = (string)reader["NombreMarca"];
            }
            else
            {
                producto.Marca.IdMarca = 0;
                producto.Marca.Nombre = string.Empty;
            }
        }

        public void add(Producto newProducto)
        {
            try
            {
                DB.clearParameters();
                DB.setQuery("SP_insertProducto @Nombre, @IdMarca, @IdCategoria, @StockActual, @StockMinimo, @PorcentajeGanancia, @FechaVencimiento, @Precio");

                DB.setParameter("@Nombre", newProducto.Nombre);
                DB.setParameter("@IdMarca", newProducto.Marca.IdMarca);
                DB.setParameter("@IdCategoria", newProducto.Categoria.IdCategoria);
                DB.setParameter("@StockActual", newProducto.StockActual);
                DB.setParameter("@StockMinimo", newProducto.StockMinimo);
                DB.setParameter("@PorcentajeGanancia", newProducto.PorcentajeGanancia);
                DB.setParameter("@FechaVencimiento", newProducto.FechaVencimiento != null ? newProducto.FechaVencimiento : (object)DBNull.Value);
                DB.setParameter("@Precio", newProducto.Precio);


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

        public void delete(string Id)
        {
            try
            {
                DB.clearParameters();
                DB.setQuery("DELETE FROM Producto WHERE IdProducto = @IdProducto");
                DB.setParameter("@IdProducto", Id);

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

        public void modify(Producto producto)
        {
            try
            {
                DB.clearParameters();
                DB.setQuery("UPDATE Producto set Nombre = @Nombre, IdMarca = @IdMarca, IdTipoProducto = @IdCategoria, StockActual = @StockActual, StockMinimo = @StockMinimo, PorcentajeGanancia = @PorcentajeGanancia, FechaVencimiento = @FechaVencimiento where IdProducto = @IdProducto\r\n");

                DB.setParameter("@IdProducto", producto.IdProducto);
                DB.setParameter("@Nombre", producto.Nombre);
                DB.setParameter("@IdMarca", producto.Marca.IdMarca);
                DB.setParameter("@IdCategoria", producto.Categoria.IdCategoria);
                DB.setParameter("@StockActual", producto.StockActual);
                DB.setParameter("@StockMinimo", producto.StockMinimo);
                DB.setParameter("@PorcentajeGanancia", producto.PorcentajeGanancia);
                DB.setParameter("@FechaVencimiento", producto.FechaVencimiento != null ? producto.FechaVencimiento : (object)DBNull.Value);


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

        public int getStock(int IdProduct)
        {
            try
            {
                int stock = 0;
                DB.clearParameters();
                DB.setQuery("SELECT StockActual FROM Producto WHERE IdProducto = @IdProduct");

                DB.setParameter("@IdProduct", IdProduct);

                DB.excecuteQuery();

                while (DB.Reader.Read())
                {
                    if (DB.Reader["StockActual"] != DBNull.Value)
                    {
                        stock = Convert.ToInt32(DB.Reader["StockActual"]);
                    }
                }

                return stock;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FATAL ERROR: Error al obtener Stock. Comuníquese con el Soporte.\nDetalles: {ex.Message}");
                return 0;
            }
            finally
            {
                DB.CloseConnection();
            }
        }

        public int getMinStock(int IdProduct)
        {
            try
            {
                int stock = 0;
                DB.clearParameters();
                DB.setQuery("SELECT StockMinimo FROM Producto WHERE IdProducto = @IdProduct");

                DB.setParameter("@IdProduct", IdProduct);

                DB.excecuteQuery();

                while (DB.Reader.Read())
                {
                    if (DB.Reader["StockMinimo"] != DBNull.Value)
                    {
                        stock = Convert.ToInt32(DB.Reader["StockMinimo"]);
                    }
                }

                return stock;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FATAL ERROR: Error al obtener StockMin. Comuníquese con el Soporte.\nDetalles: {ex.Message}");
                return 0;
            }
            finally
            {
                DB.CloseConnection();
            }
        }

        public void updateStock(int idProduct, int StockAct)
        {
            try
            {
                DB.clearParameters();
                DB.setQuery("UPDATE Producto set StockActual = @StockActual where IdProducto = @IdProducto\r\n");

                DB.setParameter("@IdProducto", idProduct);
                DB.setParameter("@StockActual", StockAct);

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

        public bool verifySells(int IdProd)
        {
            try
            {
                DB.clearParameters();
                DB.setQuery("SELECT COUNT(*) FROM DetalleVenta WHERE IdProducto = @IdProd");
                DB.setParameter("@IdProd", IdProd);
                DB.excecuteQuery();

                if (DB.Reader.Read())
                {
                    int count = DB.Reader.GetInt32(0);
                    return count == 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FATAL ERROR: Error al verificar VentasProducto. Comuníquese con el Soporte.\nDetalles: {ex.Message}");
                return false;
            }
            finally
            {
                DB.CloseConnection();
            }
        }

        public bool verifyBuys(int IdProd)
        {
            try
            {
                DB.clearParameters();
                DB.setQuery("SELECT COUNT(*) FROM DetalleCompra WHERE IdProducto = @IdProd");
                DB.setParameter("@IdProd", IdProd);
                DB.excecuteQuery();

                if (DB.Reader.Read())
                {
                    int count = DB.Reader.GetInt32(0);
                    return count == 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FATAL ERROR: Error al verificar Compras. Comuníquese con el Soporte.\nDetalles: {ex.Message}");
                return false;
            }
            finally
            {
                DB.CloseConnection();
            }
        }
    }
}
