using ProIntegracao.Data.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProIntegracao.Data
{
    public class UsuarioQuery
    {
        public static Usuario ConsultarUsuarioPorID(int idUsuario)
        {
            return new RepositorioBase().ProcurarTodos<Usuario>(x => x.Id == idUsuario).FirstOrDefault();
        }
        public static IList<Usuario> ObterTodosUsuarioPorLogin(string login)
        {
            return new RepositorioBase().ObterUsuarioPorLogin(login);
        }
        public static bool ConsultarUsuarioLonEsenha(string login, string senha)
        {
            var RESULT = new RepositorioBase().ProcurarTodos<Usuario>(x => x.Login == login && x.Senha == senha).FirstOrDefault();
            if (RESULT != null)
            {
                return true;
            }
            else return false;
        }
        public static Usuario UsuarioAutenticado(string login, string senha)
        {
            return new RepositorioBase().ProcurarTodos<Usuario>(x => x.Login == login && x.Senha == senha).FirstOrDefault();
         
        }
        public static void InserirUsuario(Usuario usuario)
        {
            new RepositorioBase().NewAtualizar(usuario);
        }
        

        public static List<Usuario> Consultar()
        {
            var lista = new List<Usuario>();

            try
            {
                var repo = new RepositorioBase();
                lista = repo.ObterTodos<Usuario>().ToList();
            }
            catch 
            {
                //Log   
            }

            return lista; 
        }
        public static List<Usuario> ConsultarPorId(int id)
        {
            return new RepositorioBase().ProcurarTodos<Usuario>(x => x.Id == id).ToList();
        }

        public static Usuario ObterPeloLogin(string login)
        {
            return new RepositorioBase().ProcurarTodos<Usuario>(x => x.Login == login).FirstOrDefault();

        }

        public static List<Usuario> ListarUsuarioPorLogin(string termo)
        {
            var lista = new List<Usuario>();

            try
            {
                var repo = new RepositorioBase();
                lista = repo.ObterTodos<Usuario>().Where(x => x.Login.Contains(termo)).ToList();
            }
            catch 
            {
                //Log   
            }
            
            return lista;

        }

        public static void Atualizar(Usuario usuario)
        {
            new RepositorioBase().Atualizar(usuario);
        }
    }
}
