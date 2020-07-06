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
        public static string PDFMicroServiceUrl = "https://parteipdfgenerator.azurewebsites.net/PDFCreate/";
        public enum EndPoint
        {
            CreateMemberListPDF,
            CreateTripPDF
        }

        private static HttpClient client = new HttpClient();
        public static async Task<MemoryStream> GetPDFContentAsync(HttpResponseMessage response)
        {
            var stringContent = await response.Content.ReadAsStringAsync();
            var net = new WebClient();
            var data = net.DownloadData((string)stringContent);
            var content = new MemoryStream(data);

            return content;
        }

        public static async Task<HttpResponseMessage> SendPDFRequestAsync(EndPoint endPoint, object payload)
        {
            var jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
            var microServiceEndpoint = PDFMicroServiceUrl + endPoint.ToString();

            return await client.PostAsync(microServiceEndpoint, new StringContent(jsonPayload, Encoding.UTF8, "application/json"));
        }
    }
}
