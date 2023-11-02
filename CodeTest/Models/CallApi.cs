namespace PruebaIngreso.Models
{
    public class CallApi : AddDecorator
    {
        public CallApi(Record table, string code) : base(table, code)
        {
        }

        public override double Margin => _table.Margin;
    }
}