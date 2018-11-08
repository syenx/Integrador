using ProIntegracao.Data.EntidadeProSimulador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProIntegracao.UI.Helper
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumHelper
    {
        /// <summary> Get Items
        /// 
        /// </summary>
        /// <typeparam name="TEnum">T ENUM</typeparam>
        /// <returns></returns>
        public static IEnumerable<AulaStatus> GetItems<TEnum>()
            where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("TEnum must be an Enumeration type");

            var res = from e in System.Enum.GetValues(typeof(TEnum)).Cast<TEnum>()
                      select new AulaStatus() { Id = Convert.ToInt32(e), Nome = e.ToString() };

            return res;
        }

    }
}