using ProIntegracao.Data.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProIntegracao.Data.Queries.ErroQuery
{
    public class ForcarErroQuery
    {







        public static List<ForcarErro> Consultar()
        {
            return new RepositorioBase().ObterTodos<ForcarErro>().ToList();
        }
        public static List<ForcarErro> ConsultarPorIDMatricula(int IdMatricula)
        {
            var lista = new RepositorioBase().ProcurarTodos<ForcarErro>(x => x.Aluno.Id == IdMatricula).ToList();
            return lista;
        }

        public static ForcarErro ConsultarForcarErroPorID(int id)
        {
            var lista = new RepositorioBase().ProcurarTodos<ForcarErro>(x => x.Id == id).FirstOrDefault();
            return lista;
        }

        public static void Inserir(ForcarErro matricula)
        {
            new RepositorioBase().Salvar(matricula);
        }

        public static void Alterar(ForcarErro matricula)
        {
            new RepositorioBase().Atualizar(matricula);
        }
    }
}
