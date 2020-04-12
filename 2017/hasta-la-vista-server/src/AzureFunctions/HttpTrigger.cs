namespace AzureFunctions
{
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Microsoft.Azure.WebJobs.Host;
    using Newtonsoft.Json;
    using QRCodeCore;

    public class HttpTrigger
    {
        public static async Task<HttpResponseMessage> GenerateQR(HttpRequestMessage req, TraceWriter log)
        {
            log.Info("Entering function");

            // Read the json request.
            var qrRequestJson = await req.Content.ReadAsStringAsync();
            var qrRequest = JsonConvert.DeserializeObject<SimpleVCardRequest>(qrRequestJson);

            log.Info("Request was deserialized");

            // Create the vCard string
            var vCard = "BEGIN:VCARD\n";
            vCard += $"FN:{qrRequest.Name}\n";
            vCard += $"TEL;WORK;VOICE:{qrRequest.Phone}\n";
            vCard += "END:VCARD";

            log.Info("vCard was created");

            // Generate de QRCode
            QRCodeData qrCodeData = new QRCodeData(vCard);
            qrCodeData.EccLevel = EccLevel.Q;

            log.Info("data created");

            var generator = new SvgQRCode(qrCodeData);
            var svg = generator.Create(512);

            log.Info("svg was created");

            var response = new HttpResponseMessage()
            {
                Content = new StringContent(svg),
                StatusCode = HttpStatusCode.OK,
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/svg+xml");

            log.Info("sending response");

            return response;
        }
    }

    // Request class to hold the Name and Phone number used to generated the vCard QR Code
    public class SimpleVCardRequest
    {
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}