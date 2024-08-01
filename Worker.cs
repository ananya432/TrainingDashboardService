using Dapper;
using Microsoft.Data.SqlClient;
using PowerBIService.Model;
using System.Globalization;
using System.Text.Json;
using PowerBIService.Services;

namespace PowerBIService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IService _service;
        private static bool _exit = false;
        public Worker(ILogger<Worker> logger, IService service)
        {
            _logger = logger;
            _service = service;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_exit)
                {
                    await Task.Delay(100000);
                    continue;
                }
                
                SqlConnection connection = new SqlConnection("Server=10.10.33.35\\Webtel;Database=PowerBI;User Id=sa;Password=Webtel@2023;Trusted_Connection=False;MultipleActiveResultSets=true;TrustServerCertificate=True;");
                connection.Open();
                //var listOfLearnerID = connection.Query<string>("SELECT [LearnersId]\r\n  FROM [dbo].[LearnerID]\r\n").ToList();
                //var listOfProductID = connection.Query<string>("SELECT [ProductId]\r\n  FROM [dbo].[ProductID]").ToList();
                var dates = new List<DateTime>();
                DateTime start = DateTime.Now.AddDays(-90);
                DateTime end = DateTime.Now;

                for (var dt = start; dt <= end; dt = dt.AddDays(1))
                {
                    dates.Add(dt);
                }
                dates = dates.OrderByDescending(d => d.Date).ToList();
                var responseTr = await _service.GetTransactionRecord();
                //var listOfProductId = response.data.Select(x => x.items.Select(x => x.id).ToList()).ToList();

                List<string> listOfLearnerId = [];
                foreach (var item in responseTr.data)
                {
                    listOfLearnerId.Add(item._id);
                }

                List<string> listOfProductId = [];
                foreach (var record in responseTr.data) {
                    foreach (var product in record.items) {
                        listOfProductId.Add(product.id);
                    }
                }

                var client = new HttpClient();
                foreach (var dateInDateTimeFormat in dates)
                {
                    foreach (var learnerId in listOfLearnerId)
                    {
                        foreach (var productId in listOfProductId)
                        {
                            try
                            {
                                //string learnerId = "669a0243835d4b30de916cd6";
                                //string productId = "64788c5ee4b029c2c7f279bb";
                                //string date = "2024/07/24";
                                //var response = await _service.GetTransactionRecord();
                                //string LearnerId = learnerId._id.ToString();
                                //string ProductId = learnerId.items.;
                                string date = dateInDateTimeFormat.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
                                var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.ongraphy.com/public/v1/learners/{learnerId}/usage?mid=webtel&key=57561343-3adf-4b38-80d5-08ffb780bf4b&productId={productId}&date={date}");
                                request.Headers.Add("Cookie", "SESSIONID=EF69EF084DA433AF999A43785156A5A8");
                                var response = await client.SendAsync(request);
                                response.EnsureSuccessStatusCode();
                                var result = await response.Content.ReadAsStringAsync();
                                var dto = JsonSerializer.Deserialize<DTO>(result);
                                DynamicParameters dynamicParam = new DynamicParameters();
                                dynamicParam.Add("Employee", productId);
                                dynamicParam.Add("Product", learnerId);
                                dynamicParam.Add("Date", date);
                                dynamicParam.Add("APIData", dto.timeSpent);
                                connection.Execute("INSERT INTO [dbo].[EmployeeTable]\r\n           ([Employee]\r\n           ,[Product]\r\n           ,[Date]\r\n           ,[APIData])\r\n     VALUES\r\n           (@Employee\r\n           ,@Product\r\n           ,@Date\r\n           ,@APIData)", dynamicParam);

                            }
                            catch (Exception ex) {
                                _logger.LogError("An error occurred, {ex}", ex);
                            }
                        }
                    }
                }
                connection.Dispose();
                _exit = true;
            }
        }
    }
}
