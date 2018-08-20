using ProIntegracao.Data.Entidade;
using ProIntegracao.Model.Repositorio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace ProIntegracao.UI.ViewModel
{
    /// <summary>
    ///  Simular Erro View Model
    /// </summary>
    public class SimularErroViewModel : BaseViewModel
    {
        #region Variaveis

        private static readonly RepositorioSimularErro _repo = new RepositorioSimularErro();
        private static readonly RepositorioTipoErro _repoTipoErro = new RepositorioTipoErro();
        private static readonly RepositorioMatricula _repoMatricula = new RepositorioMatricula();
        private static readonly RepositorioAluno _repoAluno = new RepositorioAluno();

        #endregion
        
        #region Propriedades

        /// <summary>
        /// Cpf do Aluno
        /// </summary>
        [DisplayName("CPF:")]
        public string Cpf { get; set; }

        /// <summary>
        /// Id da Matricula
        /// </summary>
        public int IdMatricula { get; set; }

        /// <summary>
        /// Id Tipo Erro
        /// </summary>
        [DisplayName("Tipo Erro:")]
        public int IdTipoErro { get; set; }

        /// <summary>
        /// Matricula
        /// </summary>
        public Matricula Matricula { get; set; }

        /// <summary>
        /// Lista Tipo Erro
        /// </summary>
        public IEnumerable<SelectListItem> ListaTipoErro { get; set; }


        #endregion
        
        #region Construtor

        /// <summary>
        /// Forca Erro View Model
        /// </summary>
        public SimularErroViewModel()
        {
            ListaTipoErro = CarregarTipoErro().AsEnumerable();
        }

        #endregion
        
        #region Métodos

        /// <summary>
        /// Carrgar Lista de Tipo Erro
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> CarregarTipoErro()
        {
            List<Erro> list = new List<Erro>();
            list = _repoTipoErro.Listar().ToList();

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
        /// Obeter matricula por CPF
        /// </summary>
        /// <param name="cpf">cpf do Aluno</param>
        /// <returns></returns>
        public Matricula ObterMatriculaPorCpf(string cpf)
        {
            var matricula = new Matricula();
            try
            {
                matricula = _repoMatricula.Listar().Where(m => m.Aluno.CpfAluno == cpf).SingleOrDefault();
            }
            catch (Exception)
            {

            }
            return matricula;
        }
        
        /// <summary> Parse Pagina View Model
        /// </summary>
        /// <param name="model">Pagina View Model</param>
        /// <returns>Pagina</returns>
        public ForcarErro ParseForcarViewModel(SimularErroViewModel model)
        {
            var forcarerro = new ForcarErro();
            var aluno = _repoAluno.Listar().Where(m => m.CpfAluno == model.Cpf).SingleOrDefault();

            if (model.Id > 0) forcarerro = _repo.Obter<ForcarErro>(model.Id);

            forcarerro.Id = (forcarerro == null) ? model.Id : forcarerro.Id;
            forcarerro.Aluno = _repoAluno.Listar().Where(m => m.Id == aluno.Id).FirstOrDefault();
            forcarerro.Erro = _repoTipoErro.Obter<Erro>(model.IdTipoErro);

            return forcarerro;
        }
        
        #endregion
    }
}