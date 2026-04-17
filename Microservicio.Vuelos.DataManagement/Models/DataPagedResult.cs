using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class DataPagedResult<T>
    {
        public IEnumerable<T> Data { get; set; }

        public MetaData Meta { get; set; }
    }

    public class MetaData
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public int Total { get; set; }

        public int TotalPages { get; set; }
    }
}