using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.ViewModels
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage  { get; private set; }
        public int TotalPages   { get; private set; }

        public int PageSize     { get; private set; }
        public int TotalCount   { get; private set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext     => CurrentPage < TotalPages;

        public PagedList(List<T> items, int totalCount, int pageNumber, int pageSize)
        {
            TotalCount = totalCount;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            AddRange(items);
        }
        //public static PagedListVM<T> GetPagedList(IQueryable<T> source, int pageNumber, int pageSize)
        //{
        //    var count = source.Count();
        //    var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        //    return new PagedListVM<T>(items, count, pageNumber, pageSize);
        //}
    }
}
