using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyTeddy.Domain.Models
{
    public class GenericDomainModel<T> where T : class
    {
        public bool IsSuccessful { get; set; }

        public string ErrorMessage { get; set; }

        public T Data { get; set; }

        public List<T> DataList { get; set; }
    }
}
