using ProIntegracao.Data.Entidade;
using System.Collections.Generic;
using System.Linq;


namespace ProIntegracao.Data
{
    public class EstadoQuery
    {
        public static List<Estado> ConsultarEstado()
        {
            return new RepositorioBase().ObterTodos<Estado>().ToList();
        }
        public static List<Estado> ConsultarEstadoInativos()
        {
            return new RepositorioBase().ObterTodos<Estado>().Where(f => f.Id == 0).ToList();
        }
        public static Estado ConsultarEstadoPorID(int idAcao)
        {
            var lista = new RepositorioBase().ProcurarTodos<Estado>(x => x.Id == idAcao).FirstOrDefault();
            return lista;
        }


        


        public static void InserirEstado(Estado matricula)
        {
            new RepositorioBase().Salvar(matricula);
        }

    }
}
