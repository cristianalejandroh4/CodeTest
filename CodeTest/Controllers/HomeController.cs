using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using PruebaIngreso.Models;
using Quote.Contracts;
using Quote.Models;

namespace PruebaIngreso.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQuoteEngine quote;
        private readonly HttpClient _httpClient;

        public HomeController(IQuoteEngine quote)
        {
            this.quote = quote;
            _httpClient = new HttpClient();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            var request = new TourQuoteRequest
            {
                adults = 1,
                ArrivalDate = DateTime.Now.AddDays(1),
                DepartingDate = DateTime.Now.AddDays(2),
                getAllRates = true,
                GetQuotes = true,
                RetrieveOptions = new TourQuoteRequestOptions
                {
                    GetContracts = true,
                    GetCalculatedQuote = true,
                },
                TourCode = "E-U10-PRVPARKTRF",
                Language = Language.Spanish
            };

            var result = this.quote.Quote(request);
            var tour = result.Tours.FirstOrDefault();
            ViewBag.Message = "Test 1 Correcto";
            return View(tour);
        }

        public ActionResult Test2()
        {
            ViewBag.Message = "Test 2 Correcto";
            return View();
        }

        public ActionResult Test3()
        {
            var code = "E-U10-UNILATIN";

            var strUrl = $"http://refactored-pancake.free.beeceptor.com/margin/{code}";
            
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

            ViewBag.Message = $"{result}";

            return View();
        }

        public ActionResult Test4()
        {
            var request = new TourQuoteRequest
            {
                adults = 1,
                ArrivalDate = DateTime.Now.AddDays(1),
                DepartingDate = DateTime.Now.AddDays(2),
                getAllRates = true,
                GetQuotes = true,
                RetrieveOptions = new TourQuoteRequestOptions
                {
                    GetContracts = true,
                    GetCalculatedQuote = true,
                },
                Language = Language.Spanish
            };

            var result = this.quote.Quote(request);

            Record record = new Properties();

            var list = new List<ViewModel>();

            foreach (var item in result.TourQuotes)
            {
                record = new CallApi(record, item.ContractService.ServiceCode);

                var viewModel = new ViewModel
                {
                    ServiceCode = item.ContractService.ServiceCode,
                    AdultNetRate = item.adultNetRate,
                    AdultRate = item.adultRate,
                    Margin = record.Margin
                };

                list.Add(viewModel);
            }

            return View(list);
        }
    }
}