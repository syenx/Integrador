using ProIntegracao.Data.Entidade;
using ProIntegracao.Model.Repositorio.Base;
using System;
using System.Linq;

namespace ProIntegracao.Model.Repositorio
{
    /// <summary>
    /// Repositorio Usuario
    /// </summary>
    public class RepositorioUsuario : BaseRepositorio<Usuario>
    {
        /// <summary>
        /// Buscar Usuario por Login e Senha 
        /// Validação de Usuario
        /// </summary>
        /// <param name="usuario">User name</param>
        /// <param name="senha">Password</param>
        /// <returns></returns>
        public Usuario BuscarUsuarioLogineSenha(string usuario, string senha)
        {
            var user = new Usuario();

            try {
                user = Listar().Where(m => m.Login.ToUpper().Equals(usuario.ToUpper()) 
                                        && m.Senha.Equals(senha) 
                                        && m.DtExclusao == null)
                                        .SingleOrDefault();

            } catch (Exception ex){
                user = null;
                
            }
            return user;
        }

        /// <summary>Buscar Usuario Login e Senha por Hash
        /// </summary>
        /// <param name="hash">Hash</param>
        /// <returns></returns>
        public Usuario BuscarUsuarioLogineSenhaPorHash(string hash)
        {
            var usuario = new Usuario();
            try
            {
                usuario = Listar().Where(m => m.Hash == hash && m.DtExclusao == null).SingleOrDefault();
            }
            catch (Exception ex)
            {
                var message = string.Format("Verificação de Usuário por HASH | Erro : {0}", ex.Message);
           
            }

            return usuario;
        }

    }
}
