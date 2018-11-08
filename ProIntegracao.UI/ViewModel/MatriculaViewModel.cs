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
    /// MatriculaViewModel
    /// </summary>
    public class MatriculaViewModel : BaseViewModel
    {
        #region Variaveis

        private static readonly RepositorioMatricula _repo = new RepositorioMatricula();
        private static readonly RepositorioSexo _repoSexo = new RepositorioSexo();
        private static readonly RepositorioEstado _repoEstado = new RepositorioEstado();
        private static readonly RepositorioAluno _repoAluno = new RepositorioAluno();
        private static readonly RepositorioMatricula _repoMatri = new RepositorioMatricula();
        #endregion

        #region Construtor

        /// <summary>
        /// MatriculaViewModel
        /// </summary>
        public MatriculaViewModel()
        {
            ListaEstado = CarregarEstado().AsEnumerable();
      //      DtCadastro = DateTime.Now;
            HoraAula = "00:00";
        }

        /// <summary>
        /// MatriculaViewModel
        /// </summary>
        /// <param name="matricula">Matricula</param>
        public MatriculaViewModel(Matricula matricula)
        {

            Id = matricula.Id;
            NomeAluno = matricula.Aluno.Nome;
            IdAluno = matricula.Aluno.Id;
          //  DtCadastro = matricula.DtCadastro;
            DtExclusao = matricula.DtExclusao;
            QtdAula = matricula.QtdAula;
            IdEstado = matricula.Estado.Id;
            HoraAula = matricula.HoraAula;
            CodigoCfc = matricula.CodigoCfc;
            PSA = matricula.Psa;
            Cpf = _repoAluno.Listar().Where(m => m.Id == matricula.Aluno.Id).FirstOrDefault().CpfAluno;
            ListaEstado = CarregarEstado(matricula.Id).AsEnumerable();
        }

        #endregion

        #region Propriedade

        /// <summary>
        /// Nome
        /// </summary>
        [DisplayName("Aluno :")]
        public int IdAluno { get; set; }
        

        /// <summary>
        /// Qtd Aula
        /// </summary>
        [DisplayName("Qtd. Aulas :")]
        public int QtdAula { get; set; }


        /// <summary>
        /// Código CFC
        /// </summary>
        [DisplayName("Código CFC :")]
        public int CodigoCfc { get; set; }
        
        /// <summary>
        /// Id Estado
        /// </summary>
        [DisplayName("Estado :")]
        public int IdEstado { get; set; }

        /// <summary>
        /// Hora Aula
        /// </summary>
        [DisplayName("Hora Aula :")]
        public string HoraAula { get; set; }

        /// <summary>
        /// Hora Aula
        /// </summary>
        [DisplayName("Nome Aluno :")]
        public string NomeAluno { get; set; }

        /// <summary>
        /// Cpf do Aluno
        /// </summary>
        [DisplayName("CPF :")]
        public string Cpf { get; set; }

        /// <summary>
        /// PSA
        /// </summary>
        [DisplayName("PSA :")]
        public string PSA { get; set; }

        /// <summary>
        /// Lista Estado
        /// </summary>
        public IEnumerable<SelectListItem> ListaEstado { get; set; }

        #endregion

        #region Metodos
        
        /// <summary> Carregar Select List Estado
        /// </summary>
        /// <param name="id">Id Estado</param>
        /// <returns>List SelectListItem</returns>
        public List<SelectListItem> CarregarEstado(int id = 0)
        {
            List<Estado> estados = new List<Estado>();

            estados = _repoEstado.Listar().OrderBy(m => m.Uf).ToList();

            var lista = new List<SelectListItem>();

            foreach (var item in estados)
            {
                var slitem = new SelectListItem()
                {
                    Text = item.Nome
                    ,
                    Value = item.Id.ToString()
                    ,
                    Selected = (item.Id == id) ? true : false
                };

                lista.Add(slitem);
            }

            return lista;
        }
        
        /// <summary>Parse Matricula View Model
        /// </summary>
        /// <param name="model">MatriculaViewModel </param>
        /// <returns>Matricula</returns>
        public Matricula ParseMatriculaViewModel(MatriculaViewModel model)
        {
            var aluno = _repoAluno.Listar().Where(x => x.Id == model.IdAluno).FirstOrDefault();

            var matricula = new Matricula();

            if (model.Id > 0)
                matricula = _repo.Obter<Matricula>(model.Id);

            matricula.Id            = (matricula == null) 
                                        ? model.Id 
                                        : matricula.Id;

            matricula.Aluno         = aluno;
            matricula.QtdAula       = model.QtdAula;
            matricula.Estado        = _repoEstado.Obter<Estado>(model.IdEstado);
            matricula.HoraAula      = model.HoraAula;
            matricula.Psa           = model.PSA;
            matricula.CodigoCfc     = model.CodigoCfc;
          //  matricula.DtCadastro    = model.DtCadastro;
            matricula.DtExclusao    = model.DtExclusao;

            return matricula;
        }
      
        #endregion
    }
}