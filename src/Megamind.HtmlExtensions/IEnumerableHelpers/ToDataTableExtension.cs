using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HtmlExtensions.IEnumerableHelpers
{

    public static class ToDataTableExtension
    {
        public static DataTable<T> ToDataTable<T>(this List<T> list)
        {
            return ListToDataTable(list);
        }

        internal static DataTable<T> ListToDataTable<T>(List<T> list)
        {

            var _list = InternalListToDataTable(list).Paginate();
            return new DataTable<T>()
            {
                recordsFiltered = list.Count() - 1,
                recordsTotal = list.Count() - 1,
                data = _list
            };
        }

        internal static IQueryable Paginate(this IQueryable list)
        {
            var start = HttpContext.Current.Request.Params["start"] == null ? 10 : Convert.ToInt32(HttpContext.Current.Request.Params["start"]);
            return list.Take(start + 10).Skip(start);
        }
        internal static IQueryable InternalListToDataTable<T>(List<T> list)
        {
            var length = HttpContext.Current.Request.Params["length"] == null ? 10 : Convert.ToInt32(HttpContext.Current.Request.Params["length"]);

            var searchValue = HttpContext.Current.Request.Params["search[value]"] == null ? "" : HttpContext.Current.Request.Params["search[value]"];
            var columnOrder = HttpContext.Current.Request.Params["order[0][column]"] == null ? "" : HttpContext.Current.Request.Params["order[0][column]"];
            var direction = HttpContext.Current.Request.Params["order[0][dir]"] == "asc" ? "ascending" : "descending";
            var orderBy = HttpContext.Current.Request.Params["columns[" + columnOrder + "][data]"] == null ? "" : HttpContext.Current.Request.Params["columns[" + columnOrder + "][data]"];

            if (string.IsNullOrEmpty(orderBy))
                return ListToDataTable(list, searchValue);
            return ListToDataTable(list, searchValue).OrderBy(orderBy + " " + direction);
        }
        internal static IQueryable ListToDataTable<T>(List<T> list, string term)
        {

            var fields = list.AsQueryable().ElementType.GetProperties()
                .Where(x => x.PropertyType == typeof(string))
                .ToArray();
            string filterString = string.Join(" || ", fields.Select(x => $"{x.Name}.Contains(@0)"));
            return list.AsQueryable().Where(filterString, term);

        }

    }

    public class DataTable<T>
    {
        private int _draw;

        public int draw
        {
            get
            {
                _draw = HttpContext.Current.Request.Params["draw"] == null
                    ? 1
                    : Convert.ToInt32(HttpContext.Current.Request.Params["draw"]);
                return _draw;
            }
            set => _draw = value;
        }

        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public IQueryable data { get; set; }
    }
}
