using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ParteiWebService.MicroServiceHelpers.PDFService
{
    public class RequestHelper
    {
        private static HttpClient client = new HttpClient();
        public static async Task<MemoryStream> GetPDFContentAsync(HttpResponseMessage response)
        {
            var stringContent = await response.Content.ReadAsStringAsync();
            var net = new WebClient();
            var data = net.DownloadData((string)stringContent);
            var content = new MemoryStream(data);

            return content;
        }

        public static async Task<HttpResponseMessage> SendPDFRequestAsync(string url, object payload)
        {
            var jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

            return await client.PostAsync(url, new StringContent(jsonPayload, Encoding.UTF8, "application/json"));
        }
    }
}
