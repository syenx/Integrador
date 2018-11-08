using ProIntegracao.Data.Entidade;
using ProIntegracao.Model.Repositorio.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProIntegracao.Model.Repositorio
{
    public class RepositorioPerfilPagina : BaseRepositorio<PerfilPagina>
    {
        
        public List<PerfilPagina> ListarPerfilPaginaPorUsuario(Usuario usuario)
        {
            var lista = new List<PerfilPagina>();

            try
            {
                

                lista = Listar().ToList().Where(m => m.Pagina.DtExclusao == null).ToList();

                if (!usuario.Perfil.Admin)
                    lista = lista.Where(m => m.Perfil == usuario.Perfil).ToList();

            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
            }

            return lista;

        }

    }
}
