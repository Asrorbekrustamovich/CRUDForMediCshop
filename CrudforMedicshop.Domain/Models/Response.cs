using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudforMedicshop.Domain.Models
{
    public class Response<T>
    {
        public Response(string error,int statusCode=400) 
        {
            Error = error;
            StatusCode = statusCode;

        }
        public Response(T result)
        {
            Error = null;
            Result= result;
        }
        public int StatusCode { get; set; } 
        public string Error { get; set; }
        public T Result { get; set; }
    }
}
