using Prointegracao.Data.Entidade;
using ProIntegracao.Data;
using ProIntegracao.Data.Entidade;
using ProIntegracao.Model.Repositorio.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProIntegracao.Model.Repositorio
{
    public class RepositorioPerfil : BaseRepositorio<Perfil>
    {
        public static void InserirPerfil(Perfil perfil)
        {
            new RepositorioBase().Salvar(perfil);
        }

        public bool InserirPerfil(Perfil perfil, List<PerfilPagina> listaPaginas)
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
                    var ent = repo.ObterTodos<PerfilPagina>()
                                                .Where(m => m.Perfil.Id == perfil.Id && m.Pagina.Id == entidade.Pagina.Id)
                                                .FirstOrDefault();

                    if (ent != null)
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
                repo.RollBackTransaction();
                result = false;
            }

            return result;
        }
        
        public bool VerificarAcessoPagina(PerfilPagina item)
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
