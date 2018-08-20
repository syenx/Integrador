using ProIntegracao.Data.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProIntegracao.Data.Queries.ErroQuery
{
    public class TipoErroQuery
    {





        public static List<Erro> Consultar()
        {
            return new RepositorioBase().ObterTodos<Erro>().ToList();
        }
        public static List<Erro> ConsultarPorCodigoErro(string CodigoErro)
        {
            var lista = new RepositorioBase().ProcurarTodos<Erro>(x => x.CodigoErro == CodigoErro).ToList();
            return lista;
        }

        public static Erro ConsultarTipoErroPorID(int id)
        {
            var lista = new RepositorioBase().ProcurarTodos<Erro>(x => x.Id == id).FirstOrDefault();
            return lista;
        }

        public static void Inserir(Erro matricula)
        {
            new RepositorioBase().Salvar(matricula);
        }

        public static void Alterar(Erro matricula)
        {
            new RepositorioBase().Atualizar(matricula);
        }

    }
}
