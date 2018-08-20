using ProIntegracao.Data.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProIntegracao.Data
{
    public class MenuQuery
    {
        public static List<Menu> ConsultarMenu()
        {
            return new RepositorioBase().ObterTodos<Menu>().ToList();
        }
        public static List<Menu> ConsultarMenuInativo()
        {
            return new RepositorioBase().ObterTodos<Menu>().Where(f => f.Id == 0).ToList();
        }
        public static Menu ConsultarMatriculaPorID(int idMenu)
        {
            return new RepositorioBase().ProcurarTodos<Menu>(x => x.Id == idMenu).FirstOrDefault();
        }
        public static void InserirMatricula(Menu matricula)
        {
            new RepositorioBase().Salvar(matricula);
        }
    }
}
