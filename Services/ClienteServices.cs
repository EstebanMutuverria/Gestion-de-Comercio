using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Utils;

namespace Services
{
    public class ClienteServices
    {
        private DataBaseAccess DB = new DataBaseAccess();

        public List<Cliente> listar(string filters = null)
        {
            List<Cliente> list = new List<Cliente>();
            try
            {
                string sqlQuery = "SELECT * FROM Cliente";
                if (!string.IsNullOrEmpty(filters))
                {
                    sqlQuery += $" WHERE {filters}";
                }

                DB.setQuery(sqlQuery);
                DB.excecuteQuery();

                while (DB.Reader.Read())
                {
                    Cliente cliente = new Cliente();
                    cliente.IdCliente = (int)DB.Reader["IdCliente"];
                    cliente.Nombre = (string)DB.Reader["Nombre"];
                    cliente.Apellido = (string)DB.Reader["Apellido"];
                    cliente.Correo = (string)DB.Reader["Correo"];
                    cliente.Telefono = (string)DB.Reader["Telefono"];
                    cliente.Direccion = (string)DB.Reader["Direccion"];
                    cliente.DNI = DB.Reader["DNI"] != DBNull.Value ? DB.Reader["DNI"].ToString() : "";
                    cliente.CUIT = DB.Reader["CUIT"] != DBNull.Value ? DB.Reader["CUIT"].ToString() : "";
                    cliente.Estado = (bool)DB.Reader["Estado"];


                    list.Add(cliente);
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

        public void add(Cliente newCliente)
        {
            try
            {
                DB.clearParameters();
                DB.setQuery("INSERT INTO Cliente (Nombre, Apellido, Correo, Telefono, Direccion, DNI, CUIT, TipoPersona) VALUES (@Nom, @Ape, @Cor, @Tel, @Dir, @D, @C, @TPersona)");

                DB.setParameter("@Nom", newCliente.Nombre);
                DB.setParameter("@Ape", newCliente.Apellido);
                DB.setParameter("@Cor", newCliente.Correo);
                DB.setParameter("@Tel", newCliente.Telefono);
                DB.setParameter("@Dir", newCliente.Direccion);
                DB.setParameter("@D", newCliente.DNI);
                DB.setParameter("@C", newCliente.CUIT);
                DB.setParameter("@TPersona", newCliente.TipoPersona);

                DB.excecuteAction();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FATAL ERROR: Error al crear Cliente. Comuníquese con el Soporte.\nDetalles: {ex.Message}");
            }
            finally
            {
                DB.CloseConnection();
            }
        }

        public void modify(Cliente cliente)
        {
            try
            {
                DB.clearParameters();
                DB.setQuery("UPDATE Cliente SET Nombre = @Nom, Apellido = @Ape, Correo = @Cor, Telefono = @Tel, Direccion = @Dir, DNI = @D, CUIT = @C WHERE IdCliente = @Id");

                DB.setParameter("@Nom", cliente.Nombre);
                DB.setParameter("@Ape", cliente.Apellido);
                DB.setParameter("@Cor", cliente.Correo);
                DB.setParameter("@Tel", cliente.Telefono);
                DB.setParameter("@Dir", cliente.Direccion);
                DB.setParameter("@D", cliente.DNI);
                DB.setParameter("@C", cliente.CUIT);
                DB.setParameter("@Id", cliente.IdCliente);

                DB.excecuteAction();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FATAL ERROR: Error al modificar Cliente. Comuníquese con el Soporte.\nDetalles: {ex.Message}");
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
                DB.setQuery("DELETE FROM Cliente WHERE IdCliente = @Id");
                DB.setParameter("@Id", id);
                DB.excecuteAction();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FATAL ERROR: Error al eliminar Cliente. Comuníquese con el Soporte.\nDetalles: {ex.Message}");
            }
            finally
            {
                DB.CloseConnection();
            }
        }

        public Cliente getClient(int Id)
        {
            try
            {
                Cliente client = new Cliente();
                DB.clearParameters();
                DB.setQuery("SELECT IdCliente, Nombre, Apellido, Correo, Telefono, Direccion, DNI, CUIT, TipoPersona FROM Cliente WHERE IdCliente = @id");
                DB.setParameter("@id", Id);
                DB.excecuteQuery();

                if (DB.Reader.Read())
                {
                    client.IdCliente = DB.Reader["IdCliente"] != DBNull.Value ? Convert.ToInt32(DB.Reader["IdCliente"]) : 0;
                    client.Nombre = DB.Reader["Nombre"] != DBNull.Value ? DB.Reader["Nombre"].ToString() : null;
                    client.Apellido = DB.Reader["Apellido"] != DBNull.Value ? DB.Reader["Apellido"].ToString() : null;
                    client.Correo = DB.Reader["Correo"] != DBNull.Value ? DB.Reader["Correo"].ToString() : null;
                    client.Telefono = DB.Reader["Telefono"] != DBNull.Value ? DB.Reader["Telefono"].ToString() : null;
                    client.Direccion = DB.Reader["Direccion"] != DBNull.Value ? DB.Reader["Direccion"].ToString() : null;
                    client.DNI = DB.Reader["DNI"] != DBNull.Value ? DB.Reader["DNI"].ToString() : null;
                    client.CUIT = DB.Reader["CUIT"] != DBNull.Value ? DB.Reader["CUIT"].ToString() : null;
                    client.TipoPersona = DB.Reader["TipoPersona"] != DBNull.Value ? DB.Reader["TipoPersona"].ToString() : null;
                }

                if (client.IdCliente == 0)
                {
                    return null;
                }

                return client;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FATAL ERROR: Error al obtener Cliente. Comuníquese con el Soporte.\nDetalles: {ex.Message}");
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
                DB.setQuery("SELECT COUNT(*) FROM Cliente WHERE DNI = @D");
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
                DB.setQuery("SELECT COUNT(*) FROM Cliente WHERE CUIT = @C");
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

        public bool verifySells(int Id)
        {
            try
            {
                DB.clearParameters();
                DB.setQuery("SELECT COUNT(*) FROM Venta WHERE IdCliente = @Id");
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
                Console.WriteLine($"FATAL ERROR: Error al verificar Ventas. Comuníquese con el Soporte.\nDetalles: {ex.Message}");
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
                DB.setQuery("UPDATE Cliente SET Estado = @est WHERE IdCliente = @id");

                DB.setParameter("@est", estado);
                DB.setParameter("@id", id);


                DB.excecuteAction();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FATAL ERROR: Error al modificar Estado Cliente. Comuníquese con el Soporte.\nDetalles: {ex.Message}");
            }
            finally
            {
                DB.CloseConnection();
            }
        }
    }
}
