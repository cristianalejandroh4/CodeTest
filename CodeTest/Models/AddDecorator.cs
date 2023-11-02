using System.Net.Http;
using System.Threading.Tasks;

namespace PruebaIngreso.Models
{
    public abstract class AddDecorator : Record
    {
        protected Record _table;
        protected string _code;

        public AddDecorator(Record table, string code)
        {
            _table = table;
            _code = code;
        }
    }
}