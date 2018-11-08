using Prointegracao.Data.Entidade;
using ProIntegracao.Data.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProIntegracao.Data
{
    public class PerfilQuery
    {
        private static RepositorioBase repo = new RepositorioBase();

        public static List<Perfil> ConsultarPerfil()
        {
            var lista = new List<Perfil>();

            try
            {
                lista = repo.ObterTodos<Perfil>().ToList();
            }
            catch
            {
                //Log   
            }

            return lista;
        }
        
        public static Perfil ConsultaPerfilPorID(int idPerfil)
        {
            return new RepositorioBase().ProcurarTodos<Perfil>(x => x.Id == idPerfil).FirstOrDefault();
        }

        public static void InserirPerfil(Perfil matricula)
        {
            new RepositorioBase().Salvar(matricula);
        }

        public static bool InserirPerfil(Perfil perfil, List<PerfilPagina> listaPaginas)
        {
            var repo = new RepositorioBase();
            var result = true;
            
            try
            {
                repo.BeginTransaction();
                perfil.Id = repo.SalvarSemTransacao(perfil);

                // Apaga todos os registros
                foreach (var entidade in listaPaginas)
                {
                    var ent = repo.ObterTodos<PerfilPagina>().Where(m => m.Perfil.Id == perfil.Id && m.Pagina.Id == entidade.Pagina.Id).FirstOrDefault();
                    if(ent != null)
                        repo.DeletarSemTransacao(ent);
                }

                //Insere somente os registros necessários
                foreach (var item in listaPaginas)
                {
                    if (!VerificarAcessoPagina(item))
                    {
                        item.Perfil = perfil;
                        if (repo.SalvarSemTransacao(item) == 0)
                        {
                            throw new Exception();
                        }
                    }
                }

                repo.CommitTransaction();

            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
                repo.RollBackTransaction();
                result = false;
            }
            
            return result;
        }
        
        /// <summary>
        /// Verificar Acesso a Página
        /// </summary>
        /// <param name="item">Perfil Página</param>
        /// <returns></returns>
        public static bool VerificarAcessoPagina(PerfilPagina item)
        {
            //Se todos os itens forem falsos, não acrescenta na contagem
            if (!item.Atualizar &&
                !item.Consultar &&
                !item.Excluir &&
                !item.Inserir)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
