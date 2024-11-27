using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using Models;

namespace Services
{
    public class MarcaServices
    {
        private DataBaseAccess DB = new DataBaseAccess();
        public List<Marca> listar(string id = "")
        {
            List<Marca> list = new List<Marca>();
            try
            {
                
                if (id != "")
                {
                    DB.setQuery("SELECT * FROM VW_MarcasGrid WHERE IdMarca =" + id);
                }
                else
                {
                    DB.setQuery("SELECT * FROM VW_MarcasGrid");
                }
                DB.excecuteQuery();

                while (DB.Reader.Read())
                {
                    Marca marca = new Marca();

                    marca.IdMarca = (int)DB.Reader["IdMarca"];
                    marca.Nombre = (string)DB.Reader["Nombre"];

                    list.Add(marca);
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

        public void add(Marca newMarca)
        {
            try
            {
                DB.clearParameters();
                DB.setQuery("INSERT into Marca (Nombre) values(@Nombre)");
                DB.setParameter("@Nombre", newMarca.Nombre);
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
                DB.setQuery("sp_DeleteMarca @IdMarca");
                DB.setParameter("@IdMarca", Id);

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

        public void modify(Marca marca)
        {
            try
            {
                DB.clearParameters();
                DB.setQuery("SP_ModifyMarca @IdMarca, @Nombre");

                DB.setParameter("@IdMarca", marca.IdMarca);
                DB.setParameter("@Nombre", marca.Nombre);

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
