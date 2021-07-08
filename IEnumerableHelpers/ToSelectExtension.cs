using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Linq.Dynamic.Core;
namespace HtmlExtensions.IEnumerableHelpers
{
    public static class ToSelectExtension
    {
        public static List<T> ToSelect<T>(this List<T> list)
        {
            return ListToSelect(list);
        }

        private static List<T> ListToSelect<T>(List<T> list)
        {
            var term = HttpContext.Current.Request.Params["term"]??"";
            var qry = list.AsQueryable().ElementType.GetProperties()
                .Where(x => x.PropertyType == typeof(string))
                .ToArray();
            string filterString = string.Join(" || ", qry.Select(x => $"{x.Name}.Contains(@0)"));
            var _list = list.AsQueryable().Where(filterString, term)
                .ToList();
            return _list;
        }
    }
}
