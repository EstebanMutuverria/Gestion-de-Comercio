using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using Models;
using System.Data;
using System.Xml.Linq;

namespace Services
{
    public class CompraServices
    {
        private DataBaseAccess DB = new DataBaseAccess();

        public List<Compra> listar(string filtro = "")
        {
            List<Compra> lista = new List<Compra>();

            try
            {
                string query = "SELECT C.IdCompra, P.Nombre AS Nombre, C.FechaCompra, C.Estado " +
                               "FROM Compra C " +
                               "INNER JOIN Proveedor P ON C.IdProveedor = P.IdProveedor";

                if (!string.IsNullOrEmpty(filtro))
                {
                    if (int.TryParse(filtro, out int idCompra))
                    {
                        query += " WHERE C.IdCompra = @Filtro";
                        DB.setParameter("@Filtro", idCompra);
                    }
                    else
                    {
                        query += " WHERE P.Nombre LIKE @Filtro " +
                                 "OR CONVERT(VARCHAR, C.FechaCompra, 103) LIKE @Filtro";

                        DB.setParameter("@Filtro", $"%{filtro}%");
                    }
                }

                DB.setQuery(query);
                DB.excecuteQuery();

                while (DB.Reader.Read())
                {
                    Compra aux = new Compra();
                    aux.IdCompra = (int)DB.Reader["IdCompra"];
                    aux.Proveedor = new Proveedor();
                    aux.Proveedor.Nombre = (string)DB.Reader["Nombre"];
                    aux.FechaCompra = (DateTime)DB.Reader["FechaCompra"];
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

        /* public List<Compra> ListarPrueba(string filtro = "")
         {
             List<Compra> lista = new List<Compra>();

             try
             {
                 string query = "SELECT C.IdCompra, P.Nombre AS Nombre, C.FechaCompra, C.Estado " +
                                "FROM Compra C " +
                                "INNER JOIN Proveedor P ON C.IdProveedor = P.IdProveedor";

                 if (!string.IsNullOrEmpty(filtro))
                 {
                     query += " WHERE P.Nombre LIKE @Filtro " +
                              "OR C.IdCompra LIKE @Filtro " +
                              "OR CONVERT(VARCHAR, C.FechaCompra, 103) LIKE @Filtro";
                 }

                 DB.setQuery(query);
                 if (!string.IsNullOrEmpty(filtro))
                     DB.setParameter("@Filtro", $"%{filtro}%");

                 DB.excecuteQuery();

                 while (DB.Reader.Read())
                 {
                     Compra aux = new Compra();
                     aux.IdCompra = (int)DB.Reader["IdCompra"];
                     aux.Proveedor = new Proveedor();
                     aux.Proveedor.Nombre = (string)DB.Reader["Nombre"];
                     aux.FechaCompra = (DateTime)DB.Reader["FechaCompra"];
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
         }*/
        public int add(int IdProveedor, DateTime FechaCompra, bool Estado)
        {
            try
            {
                DB.clearParameters();
                DB.setQuery("EXEC sp_InsertarCompra @IdProveedor, @FechaCompra, @Estado; SELECT SCOPE_IDENTITY() AS IdCompra;");
                DB.setParameter("@IdProveedor", IdProveedor);
                DB.setParameter("@FechaCompra", FechaCompra);
                DB.setParameter("@Estado", Estado);

                DB.excecuteQuery();

                int IdCompra = 0;
                if (DB.Reader.Read())
                {
                    IdCompra = Convert.ToInt32(DB.Reader["IdCompra"]);
                }
                return IdCompra;
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

        /*public void delete(int IdCompra)
        {
            try
            {
                DB.clearParameters();
                DB.setQuery("EXEC sp_AnularCompra @IdCompra");
                DB.setParameter("@IdCompra", IdCompra);
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
        }*/

        public void ActualizarEstadoCompra(int IdCompra, int nuevoEstado)
        {
            try
            {
                DB.clearParameters();
                DB.setQuery("UPDATE Compra SET Estado = @Estado WHERE IdCompra = @IdCompra");
                DB.setParameter("@Estado", nuevoEstado);
                DB.setParameter("@IdCompra", IdCompra);
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

    }
}