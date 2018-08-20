using ProIntegracao.API.Models;
using ProIntegracao.API.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProIntegracao.API.Controllers
{
    public class ExecutaPOSTController : ApiController
    {
        RepositorioMatricula repoMat = new RepositorioMatricula();
     
        // POST: api/ExecutaPOST
        public RetornoWS Post([FromBody]Ocorrencia ocorrencia)
        {
            var verificaSeExiste = repoMat.Listar().Where(x => x.Aluno.Renach == ocorrencia.numProcesso).FirstOrDefault();
            try
            {
                if (verificaSeExiste != null)
                {
                    return new RetornoWS
                    {
                        codStatus = 0,
                        mensagem = "Transação ok"
                    };
                }
                else
                {
                    return new RetornoWS
                    {
                        codStatus = 1,
                        mensagem = "Nao Existe"
                    };
                }
            }
            catch (Exception ex)
            {}
            return new RetornoWS
            {
                codStatus = 1,
                mensagem = "Nao Existe"
            };
        }

        // PUT: api/ExecutaPOST/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ExecutaPOST/5
        public void Delete(int id)
        {
        }
    }
}
