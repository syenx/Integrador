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
    public class ExecutaGETController : ApiController
    {

        RepositorioMatricula repoMat = new RepositorioMatricula();
        // GET api/cliente
        public Agendamento Get()
        {
            return new Agendamento();
        }

        // GET api/cliente/5
        public Agendamento Get(string numeroProcesso, string versao)
        {
            var verificaSeExiste = repoMat.Listar().Where(x => x.Aluno.Renach == numeroProcesso).FirstOrDefault();
            try
            {
                if (verificaSeExiste != null)
                {
                    return new Agendamento
                    {
                        codCfc = verificaSeExiste.CodigoCfc.ToString(),
                        codEquipamentoSimulador = "9999",
                         codTurmaCfc = "22222",
                        dataCurso = verificaSeExiste.DtCadastro.ToString("dd/MM/yyyy"),
                        horario = verificaSeExiste.DtCadastro.ToString("HH:mm"),
                        modeloSimulador = "22222"
                    };
                }
                else
                {
                    return new Agendamento
                    {
                        codCfc = " não existe ",
                        codEquipamentoSimulador = "",
                        codTurmaCfc = "",
                        dataCurso = "",
                        horario = "",
                        modeloSimulador = ""
                    };
                }
            }
            catch (Exception ex )
            {
                
            }
            return new Agendamento
            {
                codCfc = " na existe ",
                codEquipamentoSimulador = "",
                codTurmaCfc = "",
                dataCurso = "",
                horario = "",
                modeloSimulador = ""
            };
        }
     
    }
}
