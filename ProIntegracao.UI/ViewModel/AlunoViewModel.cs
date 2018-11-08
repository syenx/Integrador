using ProIntegracao.Data.Entidade;
using ProIntegracao.Model.Repositorio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProIntegracao.UI.ViewModel

{/// <summary>
/// /
/// </summary>
    public class AlunoViewModel : BaseViewModel
    {
        #region Variaveis
        private static readonly RepositorioAluno _repo = new RepositorioAluno();
        private static readonly RepositorioSexo _repoSexo = new RepositorioSexo();
        #endregion

        #region Propriedades
        /// <summary>
        /// CPF ALUNO
        /// </summary>
        [DisplayName("CPF:")]
        public string CpfAluno { get; set; }

        /// <summary>
        /// NOME ALUNO
        /// </summary>
        [DisplayName("Nome:")]
        public string Nome { get; set; }
      
        /// <summary>
        /// SEXO ALUNO
        /// </summary>
        public int IdSexo { get; set; }

        /// <summary>
        /// RENACH ALUNO
        /// </summary>
        [DisplayName("Renach:")]
        public string Renach { get; set; }
        /// <summary>
        /// DATA NASCIMENTO ALUNO
        /// </summary>
        [DisplayName("Data Nascimento:")]
        public string DtNascimento { get; set; }

        /// <summary>
        /// LISTA DE ALUNO PARA LISTAR ALUNO
        /// </summary>
        [DisplayName("Sexo:")]
        public IEnumerable<SelectListItem> ListaAluno { get; set; }
        
        #endregion

        #region Contrutors
        /// <summary>
        /// ALUNO VIEW MODEL.
        /// </summary>
        public AlunoViewModel()
        {
            IdSexo = 1;
            ListaAluno = CarregarAluno().AsEnumerable();
        }
        /// <summary>
        /// aluno view model 
        /// </summary>
        /// <param name="aluno"></param>
        public AlunoViewModel(Aluno aluno)
        {

            if (aluno.Id > 0)
            {
                aluno = _repo.Obter<Aluno>(aluno.Id);
            }

            Id              = aluno.Id;
            CpfAluno        = aluno.CpfAluno;
            Nome            = aluno.Nome;
            IdSexo          = aluno.Sexo.Id;
            Renach          = aluno.Renach;
            DtNascimento    =  aluno.DtNascimento.ToString();
            DtCadastro      = aluno.DtCadastro;
            DtExclusao      = aluno.DtExclusao;
            ListaAluno      = CarregarAluno().AsEnumerable();
        }

        #endregion

        #region Metodos
        /// <summary>
        /// CARREGAR TODOS ALUNOS 
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> CarregarAluno()
        {
            List<Aluno> list = new List<Aluno>();
            list = _repo.Listar().ToList();

            var lista = new List<SelectListItem>();

            foreach (var item in list)
            {
                var slitem = new SelectListItem()
                {
                    Text = item.Nome,
                    Value = item.Id.ToString()
                };

                lista.Add(slitem);
            }
            return lista;
        }


        /// <summary>
        /// CONVERTER ALUNO VIEW MODEL PARA ALUNO
        /// </summary>
        /// <param name="AlunoViewModel"></param>
        /// <returns>Objeto ALUNO Populado</returns>
        public Aluno ParseAlunoViewModel(AlunoViewModel AlunoViewModel)
        {
            var novoAluno = _repo.Listar().Where(x => x.Id == AlunoViewModel.Id).FirstOrDefault();
            try
            {
                int IdSexo = AlunoViewModel.IdSexo;

                var sexo = _repoSexo.Listar().Where(x => x.Id == IdSexo).FirstOrDefault();
                if (novoAluno != null)
                {
                  //  novoAluno.DtCadastro = DateTime.Now;
                    novoAluno.CpfAluno = AlunoViewModel.CpfAluno;
                    novoAluno.Nome = AlunoViewModel.Nome;
                    novoAluno.Renach = AlunoViewModel.Renach;
                    novoAluno.DtNascimento = AlunoViewModel.DtNascimento;
                  //  novoAluno.DtCadastro = AlunoViewModel.DtCadastro;
                    novoAluno.Sexo = sexo;
                }
                else
                {
                    var aluno = new Aluno()
                    {

                //    DtCadastro = DateTime.Now,
                    CpfAluno = AlunoViewModel.CpfAluno,
                    Nome = AlunoViewModel.Nome,
                    Renach = AlunoViewModel.Renach,
                    DtNascimento =  AlunoViewModel.DtNascimento,
                    Sexo = sexo,

                };
                    novoAluno = aluno;
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }

            return novoAluno;
        }

        #endregion

    }
}
