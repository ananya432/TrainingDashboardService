using PowerBIService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PowerBIService.Services
{
    public class Service : IService
    {
        public async Task<Transactions> GetTransactionRecord()
        {

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.ongraphy.com/public/v1/transactions?mid=webtel&key=57561343-3adf-4b38-80d5-08ffb780bf4b&skip=0&limit=0");
            request.Headers.Add("Cookie", "SESSIONID=9D963E2DC75EEB00B8B7A7C60DCF82E1");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var record = JsonSerializer.Deserialize<Transactions>(result);
            return record;

        }
    }
}
