using ProIntegracao.Data.Entidade;
using ProIntegracao.Model.Repositorio.Base;
using System.Collections.Generic;

namespace ProIntegracao.Model.Repositorio
{
    public class RepositorioMenu : BaseRepositorio<Menu>
    {

        public bool SalvarMenu(List<Menu> menus, List<Pagina> paginas) {

            var result = true;
            
            try
            {
                var i = 1;

                BeginTransaction();

                foreach (var item in menus)
                {
                    if (item.MenuPai == null)
                    {
                        item.Ordem = i;
                    }

                    Session.GetSession().Save(item);
                    
                    result = true;
                    i++;
                }

                foreach (var it in paginas)
                {
                    it.Ordem = i;
                    Session.GetSession().Save(it);
                    i++;
                }


                CommitTransaction();

            }
            catch (System.Exception ex)
            {
                RollBackTransaction();
                result = false;
            }
            
            return result;
        }




    }
}
