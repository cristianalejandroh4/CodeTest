using System.Net.Http;
using System.Threading.Tasks;

namespace PruebaIngreso.Models
{
    public class CallApi : AddDecorator
    {
        private readonly HttpClient _httpClient;
        private new readonly string _code;

        public CallApi(Record table, string code) : base(table, code)
        {
            _httpClient = new HttpClient();
            _code = code;
        }

        public override double Margin { get => RunApi(); }

        private double RunApi()
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