using System;
using System.Linq;
using System.Web.Mvc;


namespace ProIntegracao.UI.Helper
{
    public static class MyExtension
    {
        /// <summary>To SelectList Item
        /// 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumObj"></param>
        /// <returns></returns>
        public static SelectList ToSelectList<TEnum>(this TEnum enumObj)
            where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            var values = from TEnum e in System.Enum.GetValues(typeof(TEnum))
                         select new { Id = e, Name = e.ToString() };
            return new SelectList(values, "Id", "Name", enumObj);
        }



        public static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

    }
}