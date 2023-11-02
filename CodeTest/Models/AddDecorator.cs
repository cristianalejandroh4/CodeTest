using System.Net.Http;
using System.Threading.Tasks;

namespace PruebaIngreso.Models
{
    public abstract class AddDecorator : Record
    {
        protected Record _table;
        protected string _code;
        private readonly HttpClient _httpClient;

        public AddDecorator(Record table, string code)
        {
            _table = table;
            _code = code;
            _httpClient = new HttpClient();
        }

        public double RunApi()
        {
            var strUrl = $"http://refactored-pancake.free.beeceptor.com/margin/{_code}";

            HttpResponseMessage response = new HttpResponseMessage();

            Task.Run(async () =>
            {
                response = await _httpClient.GetAsync(strUrl);
            }).GetAwaiter().GetResult();

            var result = string.Empty;

            if (response.IsSuccessStatusCode)
            {
                Task.Run(async () =>
                {
                    result = await response.Content.ReadAsStringAsync();
                }).GetAwaiter().GetResult();
            }
            else
            {
                result = "{ 'margin': 0.0 }";
            }

            var found = result.IndexOf(": ") + 2;
            var lenght = (result.Length - found) - 2;
            var substr = result.Substring(found, lenght);
            var flag = double.TryParse(substr, out double margin);

            margin = !flag ? 0 : margin;

            return margin;
        }
    }
}