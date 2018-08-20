using ProIntegracao.Data.Entidade;
using ProIntegracao.Model.Repositorio.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProIntegracao.Model.Repositorio
{
    public class RepositorioEstado : BaseRepositorio<Estado>
    {
        /// <summary>
        /// Consultar Estado Por IdUsuario
        /// </summary>
        /// <param name="usuario">usuario</param>
        /// <returns></returns>
        public List<Estado> ConsultarEstadoPorIdUsuario(Usuario usuario)
        {
            var lista = new List<Estado>();
            var _repoPerfilPagina = new RepositorioPerfilPagina();

            try
            {
                var estadosporPerfilPagina = _repoPerfilPagina
                                            .Listar()
                                            .Where(m => m.Perfil.Id == usuario.Perfil.Id)
                                            .Select(m => m.Pagina.IdEstado)
                                            .Distinct().ToList();

                lista = AjusteArrayEstados(estadosporPerfilPagina);
                
            }
            catch (Exception ex)
            {
                //InserirLog("REPOSITORIOESTADO","ERRO AO LISTAR ESTADOS POR USUARIO | Message : " + ex.Message, "Erro");                           
            }

            return lista;
        }

        /// <summary>
        /// Ajustar o Arrys de String de Estados retornando uma LIST de Estados
        /// </summary>
        /// <param name="estados">Array String de Estados (UF)</param>
        /// <returns></returns>
        public List<Estado> AjusteArrayEstados(List<string> estados)
        {
            var lista = new List<string>();
            var listaEstado = new List<Estado>();

            foreach (var item in estados)
            {
                var it = item.Split(',');
                if (it.Length > 1)
                {
                    foreach (var i in it)
                    {
                        lista.Add(i);
                    }
                }
                else
                {
                    lista.Add(item);
                }
            }

            lista = lista.Distinct().ToList();

            foreach (var item in lista)
            {
                var estado = Listar().Where(m => m.Uf.ToUpper() == item).FirstOrDefault();

                if (estado != null)
                {
                    listaEstado.Add(estado);
                }
            }
            
            return listaEstado;
        }
    }
}
