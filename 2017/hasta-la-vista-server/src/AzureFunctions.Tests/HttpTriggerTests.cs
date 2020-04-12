using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Xunit;

namespace AzureFunctions.Tests
{
    public class HttpTriggerTests
    {
        [Fact]
        public async void Run_ReturnsSvg()
        {
            // while (!Debugger.IsAttached) Thread.Sleep(500);

            //arrange
            var payload = new { Name = "Carlos Mendible", Phone = "231418" };

            var request = CreateHttpRequestWith(payload);
            var traceWriter = GetTestTraceWriter();

            //act
            var response = await HttpTrigger.GenerateQR(request, traceWriter);
            var svg = await response.Content.ReadAsStringAsync();

            //assert
            Assert.NotEmpty(svg);
        }

        private TraceWriter GetTestTraceWriter()
        {
            return new TestTraceWriter(System.Diagnostics.TraceLevel.Info);
        }

        private HttpRequestMessage CreateHttpRequestWith(object jsonObject)
        {
            string jsonContent = JsonConvert.SerializeObject(jsonObject);

            var request = new HttpRequestMessage()
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            };

            return request;
        }
    }
}
