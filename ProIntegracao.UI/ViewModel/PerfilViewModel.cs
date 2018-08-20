using Prointegracao.Data.Entidade;
using ProIntegracao.Data;
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
    /// PerfilViewModel
    /// </summary>
    public class PerfilViewModel : BaseViewModel
    {
        #region Variaveis

        private static readonly RepositorioPerfilPagina _repo = new RepositorioPerfilPagina();
        private static readonly RepositorioPerfil _repoPerfil = new RepositorioPerfil();
        private static readonly RepositorioEstado _repoEstado = new RepositorioEstado();
        private static readonly RepositorioPagina _repoPagina = new RepositorioPagina();

        #endregion

        #region Construtor
        
        /// <summary>
        /// PerfilViewModel
        /// </summary>
        public PerfilViewModel()
        {
            ListaEstados = ListarEstadosPorPerfil();
        }

        /// <summary>
        /// PerfilViewModel
        /// </summary>
        /// <param name="IdEstado">Id do Estado</param>
        public PerfilViewModel(string[] IdEstado)
        {
            ListaPerfilPaginas = ListarPerfilPaginaPorEstado(IdEstado);
        }

        /// <summary>
        /// Perfil View Model
        /// </summary>
        /// <param name="idPerfil">Id Perfil</param>
        /// <param name="idEstados">Id Estados</param>
        public PerfilViewModel(int idPerfil, string[] idEstados = null)
        {
            var perfil = PerfilQuery.ConsultaPerfilPorID(idPerfil);

            Id                  = perfil.Id;
            Nome                = perfil.Nome;
            Admin               = perfil.Admin;
            DtCadastro          = perfil.DtCadastro;
            DtExclusao          = perfil.DtExclusao;
            Estados             = ListadeEstadosPorPefil(idPerfil);
            ListaEstados        = ListarEstadosPorPerfil(perfil.Id);
            ListaPerfilPaginas  = ListarPerfilPaginaPorEstado(perfil.Id, idEstados);
            
        }
        
        #endregion

        #region Propriedades
        
        /// <summary>
        /// Nome
        /// </summary>
        [DisplayName("Nome :")]
        public string Nome { get; set; }

        /// <summary>Perfil Admin
        /// </summary>
        [DisplayName("Admin :")]
        public bool Admin { get; set; }

        /// <summary>
        /// Estados
        /// </summary>
        public string Estados { get; set; }

        /// <summary>
        /// Lista Estados
        /// </summary>
        public List<SelectListItem> ListaEstados { get; set; }
        
        /// <summary>
        /// Lista Perfil Paginas
        /// </summary>
        public List<PerfilPagina> ListaPerfilPaginas { get; set; }

        /// <summary>
        /// ListaPerfilPaginasViewModel
        /// </summary>
        public List<PerfilPaginaViewModel> ListaPerfilPaginasViewModel { get; set; }

        /// <summary>
        /// Edição
        /// </summary>
        public bool Edicao { get; set; }

        #endregion

        #region Métodos

        /// <summary>Listar Estados Por Perfil
        /// </summary>
        /// <param name="idPerfil">Id do Perfil</param>
        /// <returns>string</returns>
        private string ListadeEstadosPorPefil(int idPerfil)
        {
            var result = string.Empty;
            var countEstados = _repoEstado.Listar().Count();

            if (_repoPerfil.Obter<Perfil>(idPerfil).Admin)
            {
                result = "BRASIL";
            }
            else {
                
                var paginas = _repo.Listar()
                                .Where(m => m.Perfil.Id == idPerfil)
                                .DistinctBy(m => m.Pagina)
                                .Select(m => m.Pagina)
                                .ToList();

                var lista = new List<string>();

                foreach (var item in paginas)
                {
                    lista.AddRange(item.IdEstado.Split(','));
                }
                
                var listaEstado = new List<string>();

                foreach (var item in lista.Distinct())
                {
                    listaEstado.Add(item);
                }

                if(countEstados > listaEstado.Count())
                    result = string.Join(",", listaEstado);
                else
                    result = "BRASIL";
            }
            
            return result;

        }

        /// <summary>Listar Perfil Pagina
        /// 
        /// </summary>
        /// <param name="termo">Termo de Buscao</param>
        /// <param name="ativo">Perfil ativo?</param>
        /// <returns>List PerfilPaginaViewModel</returns>
        public List<PerfilViewModel> ListarPerfilPagina(string termo, bool ativo)
        {
            var lista = new List<Perfil>();

            if(ativo)
                lista = _repoPerfil.Listar().Where(m => m.DtExclusao == null).ToList();
            else
                lista = _repoPerfil.Listar().Where(m => m.DtExclusao != null).ToList();


            if (!string.IsNullOrEmpty(termo))
                lista = lista.Where(m => m.Nome.ToLower().Contains(termo.ToLower())).ToList();

            var listaView = new List<PerfilViewModel>();

            foreach (var item in lista)
            {
                var model = new PerfilViewModel(item.Id);
                listaView.Add(model);
            }



            return listaView;
        }

        /// <summary>Listar Estados Por Pefil
        /// </summary>
        /// <param name="idPerfil">Id do Perfil</param>
        /// <returns>List Estado </returns>
        public List<SelectListItem> ListarEstadosPorPerfil(int idPerfil = 0)
        {
            //Lista de Select List Item
            var select = new List<SelectListItem>();

            // Lista de Estados
            var listaEstados = _repoEstado.Listar().OrderBy(m => m.Uf).ToList();

            // Lista Perfil Paginas
            var listaPerfilPaginas = _repo.Listar();
            
            // Lista de Páginas
            var listaPaginas = _repo.Listar().Select(m => m.Pagina).ToList();

            // Estados
            var estados = new List<string>();
            
            // Caso idPerfil > 0 traz somente estados referente perfil
            if (idPerfil > 0)
                listaPaginas = listaPerfilPaginas.Where(m => m.Perfil.Id == idPerfil).Select(m=>m.Pagina).Distinct().ToList();
            else
                listaPaginas = listaPerfilPaginas.Select(m=> m.Pagina).Distinct().ToList();
            
            
            // Cria uma Lista de selectlistitem com todos 
            // os ESTADOS identificando quais foram selecionados por perfil
            foreach (var item in listaEstados)
            {
                var sel = new SelectListItem()
                {
                    Text = item.Uf
                    , Value = item.Uf.ToString()
                    , Selected = (RetornaEstadoSelecionado(item.Uf,listaPaginas) && idPerfil > 0) ? true : false
                };

                select.Add(sel);
            }

            return select;

        }

        /// <summary>
        /// Retorna Estado Selecionado
        /// </summary>
        /// <param name="Uf">UF</param>
        /// <param name="listaPaginas">Lista de Páginas</param>
        /// <returns></returns>
        public bool RetornaEstadoSelecionado(string Uf, List<Pagina> listaPaginas)
        {

            var result = false;

            foreach (var item in listaPaginas)
            {
                var estados = item.IdEstado.Split(',');

                if (estados.Contains(Uf))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>Listar Perfil Pagina Por Estado
        /// </summary>
        /// <param name="IdEstados">Id Estados</param>
        /// <returns>List PerfilPagina </returns>
        public List<PerfilPagina> ListarPerfilPaginaPorEstado(string[] IdEstados)
        {
            var listaPagina = new List<Pagina>();
            var lista = new List<PerfilPagina>();
            var estados = new List<string>();
            
            if (IdEstados != null)
            {

                listaPagina = RetornaPaginasPorEstado(IdEstados);

                foreach (var item in listaPagina)
                {
                    lista.Add(new PerfilPagina()
                    {
                        Perfil = new Perfil()
                        , Pagina = item
                        , Inserir = false
                        , Atualizar = false
                        , Excluir = false
                        , Consultar = false
                    });
                }
            }
            
            return lista;
        }

        /// <summary>
        /// Retorna Pagina por Estado
        /// </summary>
        /// <param name="idEstados">Id Estados</param>
        /// <param name="idPerfil">Id Perfil</param>
        /// <returns></returns>
        private List<Pagina> RetornaPaginasPorEstado(string[] idEstados, int idPerfil = 0)
        {
            var listaPerfilPaginas = _repo.Listar().Where(m => m.Pagina.DtExclusao == null).ToList();

            if (idPerfil > 0)
                listaPerfilPaginas = listaPerfilPaginas.Where(m => m.Perfil.Id == idPerfil).ToList();

            var listaPaginas = _repoPagina.Listar().ToList();
            var novaListaPaginas = new List<Pagina>();
            
            foreach (var item in listaPaginas)
            {
                var estados = item.IdEstado.Split(',').ToArray();

                foreach (var estado in estados)
                {
                    if (idEstados.Contains(estado))
                        novaListaPaginas.Add(item);
                }
            }
            return novaListaPaginas.Distinct().ToList();
        }


        /// <summary>
        /// Ajuste Array Estados
        /// </summary>
        /// <param name="estados">Array de Estados</param>
        /// <returns></returns>
        public List<string> AjusteArrayEstados(List<string> estados)
        {
            var lista = new List<string>();

            foreach (var item in estados)
            {
                var it = item.Split(',');
                if (it.Length > 1)
                {
                    foreach (var i in it)
                    {
                        lista.Add(i);
                    }
                }
                else
                {
                    lista.Add(item);
                }
            }

            lista = lista.Distinct().ToList();

            return lista;
        }
        
        /// <summary>ListarPerfilPaginaPorEstado
        /// </summary>
        /// <param name="idPerfil">Id Perfil</param>
        /// <param name="IdEstados">Array Id Estado</param>
        /// <returns>List PerfilPagina </returns>
        public List<PerfilPagina> ListarPerfilPaginaPorEstado(int idPerfil, string[] IdEstados)
        {
            var listaPagina = new List<Pagina>();
            var lista = new List<PerfilPagina>();
            var estados = new List<string>();

            if (IdEstados != null)
            {
                listaPagina = RetornaPaginasPorEstado(IdEstados,idPerfil);

                foreach (var item in listaPagina)
                {

                    var perfilPagina = _repo.Listar().Where(m => m.Perfil.Id == idPerfil && m.Pagina.Id == item.Id).FirstOrDefault();

                    if (perfilPagina == null)
                    {
                        perfilPagina = new PerfilPagina()
                        {
                            Perfil = new Perfil()
                            , Pagina = item
                            , Inserir = false
                            , Atualizar = false
                            , Excluir = false
                            , Consultar = false
                        };
                    }

                    lista.Add(perfilPagina);
                }
            }

            return lista;
        }
        
        /// <summary>Parse View Model
        /// </summary>
        /// <param name="model">List PerfilPaginaViewModel</param>
        /// <returns>List PerfilPagina</returns>
        public List<PerfilPagina> ParseViewModel(List<PerfilPaginaViewModel> model)
        {

            var listaPerfil = new List<PerfilPagina>();

            if (model == null) return listaPerfil;

            foreach (var item in model)
            {
                
                    var perfilpagina = new PerfilPagina();

                    if (item.Id > 0) perfilpagina = _repo.Obter<PerfilPagina>(item.Id);

                    var perfil = new Perfil();
                    var pagina = new Pagina();
                
                    perfilpagina.Id = (perfilpagina == null) ? item.Id : perfilpagina.Id;
                    perfilpagina.Inserir = item.Inserir;
                    perfilpagina.Atualizar = item.Atualizar;
                    perfilpagina.Excluir = item.Excluir;
                    perfilpagina.Consultar = item.Consultar;
                    perfilpagina.Perfil = _repoPerfil.Obter<Perfil>(item.IdPerfil);
                    perfilpagina.Pagina = _repoPagina.Obter<Pagina>(item.IdPagina);
                    
                    
                    listaPerfil.Add(perfilpagina);
                    
            }

            return listaPerfil;

        }

        

        #endregion
    }
}