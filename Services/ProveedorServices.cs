using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Services
{
    public class ProveedorServices
    {
        
        private DataBaseAccess DB = new DataBaseAccess();

        public List<Proveedor> listar(string filters = null)
        {
            List<Proveedor> list = new List<Proveedor>();
            try
            {
                string sqlQuery = "SELECT * FROM Proveedor";
                if (!string.IsNullOrEmpty(filters))
                {
                    sqlQuery += $" WHERE {filters}";
                }

                DB.setQuery(sqlQuery);
                DB.excecuteQuery();

                while (DB.Reader.Read())
                {
                    Proveedor proveedor = new Proveedor();
                    proveedor.IdProveedor = (int)DB.Reader["IdProveedor"];
                    proveedor.Nombre = (string)DB.Reader["Nombre"];
                    proveedor.Apellido = (string)DB.Reader["Apellido"];
                    proveedor.Telefono = (string)DB.Reader["Telefono"];
                    proveedor.Correo = (string)DB.Reader["Correo"];
                    proveedor.Direccion = (string)DB.Reader["Direccion"];
                    proveedor.DNI = DB.Reader["DNI"] != DBNull.Value ? DB.Reader["DNI"].ToString() : "";
                    proveedor.CUIT = DB.Reader["CUIT"] != DBNull.Value ? DB.Reader["CUIT"].ToString() : "";
                    proveedor.Estado = (bool)DB.Reader["Estado"];

                    list.Add(proveedor);
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

        public void add(Proveedor newProveedor)
        {
            try
            {
                DB.clearParameters();
                DB.setQuery("INSERT INTO Proveedor (Nombre, Apellido, Correo, Telefono, Direccion, DNI, CUIT, TipoPersona) VALUES (@Nom, @Ape, @Cor, @Tel, @Dir, @D, @C, @TPersona)");

                DB.setParameter("@Nom", newProveedor.Nombre);
                DB.setParameter("@Ape", newProveedor.Apellido);
                DB.setParameter("@Cor", newProveedor.Correo);
                DB.setParameter("@Tel", newProveedor.Telefono);
                DB.setParameter("@Dir", newProveedor.Direccion);
                DB.setParameter("@D", newProveedor.DNI);
                DB.setParameter("@C", newProveedor.CUIT);
                DB.setParameter("@TPersona", newProveedor.TipoPersona);

                DB.excecuteAction();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FATAL ERROR: Error al crear Proveedor. Comuníquese con el Soporte.\nDetalles: {ex.Message}");
            }
            finally
            {
                DB.CloseConnection();
            }
        }

        public void modify(Proveedor proveedor) 
        {
            try
            {
                DB.clearParameters();
                DB.setQuery("UPDATE Proveedor SET Nombre = @Nom, Apellido = @Ape, Correo = @Cor, Telefono = @Tel, Direccion = @Dir, DNI = @D, CUIT = @C WHERE IdProveedor = @Id");

                DB.setParameter("@Nom", proveedor.Nombre);
                DB.setParameter("@Ape", proveedor.Apellido);
                DB.setParameter("@Cor", proveedor.Correo);
                DB.setParameter("@Tel", proveedor.Telefono);
                DB.setParameter("@Dir", proveedor.Direccion);
                DB.setParameter("@D", proveedor.DNI);
                DB.setParameter("@C", proveedor.CUIT);
                DB.setParameter("@Id", proveedor.IdProveedor);

                DB.excecuteAction();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FATAL ERROR: Error al modificar Proveedor. Comuníquese con el Soporte.\nDetalles: {ex.Message}");
            }
            finally
            {
                DB.CloseConnection();
            }
        }

        public void delete(int id)
        {
            try
            {
                DB.clearParameters();
                DB.setQuery("DELETE FROM Proveedor WHERE IdProveedor = @Id");
                DB.setParameter("@Id", id);
                DB.excecuteAction();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FATAL ERROR: Error al eliminar Proveedor. Comuníquese con el Soporte.\nDetalles: {ex.Message}");
            }
            finally
            {
                DB.CloseConnection();
            }
        }

        public Proveedor getProvider(int Id)
        {
            try
            {
                Proveedor proveedor = new Proveedor();
                DB.clearParameters();
                DB.setQuery("SELECT IdProveedor, Nombre, Apellido, Correo, Telefono, Direccion, DNI, CUIT, TipoPersona FROM Proveedor WHERE IdProveedor = @id");
                DB.setParameter("@id", Id);
                DB.excecuteQuery();

                if (DB.Reader.Read())
                {
                    proveedor.IdProveedor = DB.Reader["IdProveedor"] != DBNull.Value ? Convert.ToInt32(DB.Reader["IdProveedor"]) : 0;
                    proveedor.Nombre = DB.Reader["Nombre"] != DBNull.Value ? DB.Reader["Nombre"].ToString() : null;
                    proveedor.Apellido = DB.Reader["Apellido"] != DBNull.Value ? DB.Reader["Apellido"].ToString() : null;
                    proveedor.Correo = DB.Reader["Correo"] != DBNull.Value ? DB.Reader["Correo"].ToString() : null;
                    proveedor.Telefono = DB.Reader["Telefono"] != DBNull.Value ? DB.Reader["Telefono"].ToString() : null;
                    proveedor.Direccion = DB.Reader["Direccion"] != DBNull.Value ? DB.Reader["Direccion"].ToString() : null;
                    proveedor.DNI = DB.Reader["DNI"] != DBNull.Value ? DB.Reader["DNI"].ToString() : null;
                    proveedor.CUIT = DB.Reader["CUIT"] != DBNull.Value ? DB.Reader["CUIT"].ToString() : null;
                    proveedor.TipoPersona = DB.Reader["TipoPersona"] != DBNull.Value ? DB.Reader["TipoPersona"].ToString() : null;
                }

                if (proveedor.IdProveedor == 0)
                {
                    return null;
                }

                return proveedor;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FATAL ERROR: Error al obtener Proveedor. Comuníquese con el Soporte.\nDetalles: {ex.Message}");
                return null;
            }
            finally
            {
                DB.CloseConnection();
            }
        }

        public bool DNIAvailable(string DNI)
        {
            try
            {
                DB.clearParameters();
                DB.setQuery("SELECT COUNT(*) FROM Proveedor WHERE DNI = @D");
                DB.setParameter("@D", DNI);
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
                Console.WriteLine($"FATAL ERROR: Error al verificar DNI. Comuníquese con el Soporte.\nDetalles: {ex.Message}");
                return false;
            }
            finally
            {
                DB.CloseConnection();
            }
        }

        public bool CUITAvailable(string CUIT)
        {
            try
            {
                DB.clearParameters();
                DB.setQuery("SELECT COUNT(*) FROM Proveedor WHERE CUIT = @C");
                DB.setParameter("@C", CUIT);
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
                Console.WriteLine($"FATAL ERROR: Error al verificar CUIT. Comuníquese con el Soporte.\nDetalles: {ex.Message}");
                return false;
            }
            finally
            {
                DB.CloseConnection();
            }
        }

        public bool verifyProducts(int Id)
        {
            try
            {
                DB.clearParameters();
                DB.setQuery("SELECT COUNT(*) FROM ProveedorProducto WHERE IdProveedor = @Id");
                DB.setParameter("@Id", Id);
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
                Console.WriteLine($"FATAL ERROR: Error al verificar Productos. Comuníquese con el Soporte.\nDetalles: {ex.Message}");
                return false;
            }
            finally
            {
                DB.CloseConnection();
            }
        }

        public bool verifyBuys(int Id)
        {
            try
            {
                DB.clearParameters();
                DB.setQuery("SELECT COUNT(*) FROM Compra WHERE IdProveedor = @Id");
                DB.setParameter("@Id", Id);
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

        public void setEstado(bool estado, int id)
        {
            try
            {
                DB.clearParameters();
                DB.setQuery("UPDATE Proveedor SET Estado = @est WHERE IdProveedor = @Id");

                DB.setParameter("@est", estado);
                DB.setParameter("@id", id);


                DB.excecuteAction();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FATAL ERROR: Error al modificar Estado Proveedor. Comuníquese con el Soporte.\nDetalles: {ex.Message}");
            }
            finally
            {
                DB.CloseConnection();
            }
        }

    }
}

