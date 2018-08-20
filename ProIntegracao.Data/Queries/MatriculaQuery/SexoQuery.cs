using ProIntegracao.Data.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProIntegracao.Data
{
    public class SexoQuery
    {
        public static List<Sexo> ConsultarSexo()
        {
            return new RepositorioBase().ObterTodos<Sexo>().ToList();
        }
        public static List<Sexo> ConsultarSexoPorDoc(int id)
        {
            var lista = new RepositorioBase().ProcurarTodos<Sexo>(x => x.Id == id).ToList();
            return lista;
        }

        public static Sexo ConsultarSexoPorID(int id)
        {
            var lista = new RepositorioBase().ProcurarTodos<Sexo>(x => x.Id == id).FirstOrDefault();
            return lista;
        }

        public static void InserirSexo(Sexo Sexo)
        {
            new RepositorioBase().Salvar(Sexo);
        }

        public static void AlterarSexo(Sexo Sexo)
        {
            new RepositorioBase().Atualizar(Sexo);
        }

    }
}
