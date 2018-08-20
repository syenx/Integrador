using System.Configuration;


namespace ProIntegracao.API.NHibernate
{
    public static class Banco
    {
        public static string stringConetionString()
        {
            return ConfigurationManager.ConnectionStrings["ProIntegracao"].ConnectionString.ToString();
        }
    }
}
