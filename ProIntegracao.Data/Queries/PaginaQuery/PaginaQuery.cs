using ProIntegracao.Data.Entidade;
using System.Collections.Generic;
using System.Linq;


namespace ProIntegracao.Data
{
    public class PaginaQuery
    {
        public static List<Pagina> ConsultarPaginas()
        {
            return new RepositorioBase().ObterTodos<Pagina>().ToList();
        }
        public static List<Pagina> ConsultarAcaoInativas()
        {
            return new RepositorioBase().ObterTodos<Pagina>().Where(f => f.Id == 0).ToList();
        }
        public static Pagina ConsultarAcaoPorID(int idAcao)
        {
            var lista = new RepositorioBase().ProcurarTodos<Pagina>(x => x.Id == idAcao).FirstOrDefault();
            return lista;
        }
        public static void InserirAcao(Pagina matricula)
        {
            new RepositorioBase().Salvar(matricula);
        }
       
    }
}
