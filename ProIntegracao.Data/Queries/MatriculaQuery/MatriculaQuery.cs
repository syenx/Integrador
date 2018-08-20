using ProIntegracao.Data.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProIntegracao.Data
{
    public static class MatriculaQuery
    {
        public static List<Matricula> ConsultarMatricula()
        {
            return new RepositorioBase().ObterTodos<Matricula>().ToList();
        }
        public static List<Matricula> ConsultarMatriculaPorDoc(string cpf)
        {
            var lista = new RepositorioBase().ProcurarTodos<Matricula>(x => x.Aluno.CpfAluno == cpf).ToList();
            return lista;
        }

        public static Matricula ConsultarMatriculaPorID(int id)
        {
            var lista = new RepositorioBase().ProcurarTodos<Matricula>(x => x.Id == id).FirstOrDefault();
            return lista;
        }

        public static void InserirMatricula(Matricula matricula)
        {
           new RepositorioBase().Salvar(matricula);
        }

        public static void AlterarMatricula(Matricula matricula)
        {
            new RepositorioBase().Atualizar(matricula);
        }

    }
}
