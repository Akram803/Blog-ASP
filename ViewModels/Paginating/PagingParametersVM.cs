using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.ViewModels
{
    public class PagingParametersVM 
    {
        public int PageNumber { get; set; } = 1;  // <<

        const int maxPageSize = 20;

        private int _pageSize = 4;   // <<
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value ;
            }
        }

    }
}
