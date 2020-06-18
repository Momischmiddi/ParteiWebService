using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Aufgabe_2
{
    public class ErrorPageCreator
    {
        public static async Task CreateAsync(HttpContext context)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/html";

            var exceptionHandlerPathFeature =
                context.Features.Get<IExceptionHandlerPathFeature>();

            await context.Response.WriteAsync(
                "<html lang=\"en\">" +
                    "<head>" +
                        "<title>Fehler</title>" +
                    "</head>" +

                    "<body style=\"background-color: #21232A;\">" +
                        "<div style=\"color: white; text-align: center; margin-top:20% \">" +
                            "<h1>" +
                                "Ein unerwarteter Fehler ist aufgetreten" +
                            "</h1>" +
                            "<h2>" +
                                exceptionHandlerPathFeature?.Error.Message +
                            "</h2>" +

                            "<h2>" + 
                                "<a>" +
                                    "<a href=\"/\">Home</a><br>" +
                                "</a>" +
                            "</h2>" +
                        "</div>" +
                    "</body>" +
                "</html>");;
        }
    }
}
