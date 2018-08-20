using ProIntegracao.API.Entidade;
using System.Collections.Generic;

namespace ProIntegracao.API.Repositorio
{
    public class RepositorioMatricula : Repositorio<Matricula>
    {
        public static IList<Matricula> ConsultarMatriculaPorDoc(string cpf)
        {
           return new Repositorio<Matricula>().ProcurarTodos<Matricula>(x => x.Aluno.CpfAluno == cpf);
        }

        public static IList<Matricula> ConsultarMatriculaPorDocID(string cpf, int? id)
        {
            return new Repositorio<Matricula>().ProcurarTodos<Matricula>(x => x.Aluno.CpfAluno == cpf && x.Id == id);
        }
    }
}
