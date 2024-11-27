using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using Models;

namespace Services
{
    public class CategoriaServices
    {
        private DataBaseAccess DB = new DataBaseAccess();

        public List<Categoria> listar(string id = "")
        {
            List<Categoria> list = new List<Categoria>();
            try
            {
                if (id != "")
                {
                    DB.setQuery("SELECT * FROM VW_CategoriasGrid WHERE IdCategoria =" + id);
                }
                else
                {
                    DB.setQuery("SELECT * FROM VW_CategoriasGrid");
                }
                DB.excecuteQuery();

                while (DB.Reader.Read())
                {
                    Categoria categoria = new Categoria();

                    categoria.IdCategoria = (int)DB.Reader["IdCategoria"];
                    categoria.Nombre = (string)DB.Reader["Nombre"];

                    list.Add(categoria);
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

        public void add(Categoria newCategoria)
        {
            try
            {
                DB.clearParameters();
                DB.setQuery("INSERT INTO Categoria (Nombre) VALUES (@Nombre)");


                DB.setParameter("@Nombre", newCategoria.Nombre);
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
                DB.setQuery("sp_DeleteCategoria @IdCategoria");
                DB.setParameter("@IdCategoria", Id);

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

        public void modify(Categoria categoria)
        {
            try
            {
                DB.clearParameters();
                DB.setQuery("SP_ModifyCategoria @IdCategoria, @Nombre");

                DB.setParameter("@IdCategoria", categoria.IdCategoria);
                DB.setParameter("@Nombre", categoria.Nombre);           

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
