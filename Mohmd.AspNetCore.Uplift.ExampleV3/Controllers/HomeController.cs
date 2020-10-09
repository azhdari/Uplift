using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mohmd.AspNetCore.Uplift.ExampleV3.Models;

namespace Mohmd.AspNetCore.Uplift.ExampleV3.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostEnvironment _hosting;

        public HomeController(IHostEnvironment hosting)
        {
            _hosting = hosting;
        }

        public IActionResult Index()
        {
            return RedirectToAction("CreateBook");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult CreateBook()
        {
            return View(new BookModel());
        }

        [HttpPost]
        public IActionResult CreateBook(BookModel model)
        {
            if (ModelState.IsValid)
            {
                // do something
            }

            return View(model);
        }

        public async Task<IActionResult> RemoteCreateBook()
        {
            string url = string.Format("{0}://{1}/home/CreateBook", HttpContext.Request.Scheme, HttpContext.Request.Host);

            var values = new NameValueCollection
            {
                { "Name", "Mohammad Azhdari" }
            };

            string path = Path.Combine(_hosting.ContentRootPath, "wwwroot", "css", "site.css");

            var files = new NameValueCollection
            {
                { "FrontImage", path }
            };

            string result = await SendHttpRequest(url, values, files);

            return RedirectToAction(nameof(Index));
        }

        private async Task<string> SendHttpRequest(string url, NameValueCollection values, NameValueCollection files = null)
        {
            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
            // The first boundary
            byte[] boundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            // The last boundary
            byte[] trailer = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
            // The first time it itereates, we need to make sure it doesn't put too many new paragraphs down or it completely messes up poor webbrick
            _ = Encoding.ASCII.GetBytes("--" + boundary + "\r\n");

            // Create the request and set parameters
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";
            request.KeepAlive = true;
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Headers["security"] = "sdf";

            // Get request stream
            Stream requestStream = request.GetRequestStream();

            foreach (string key in values.Keys)
            {
                // Write item to stream
                byte[] formItemBytes = Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}", key, values[key]));
                requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                requestStream.Write(formItemBytes, 0, formItemBytes.Length);
            }

            if (files != null)
            {
                foreach (string key in files.Keys)
                {
                    if (System.IO.File.Exists(files[key]))
                    {
                        string fileName = Path.GetFileName(files[key]);

                        int bytesRead = 0;
                        byte[] buffer = new byte[2048];
                        byte[] formItemBytes = Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n", key, fileName));
                        requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                        requestStream.Write(formItemBytes, 0, formItemBytes.Length);

                        using (FileStream fileStream = new FileStream(files[key], FileMode.Open, FileAccess.Read))
                        {
                            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                // Write file content to stream, byte by byte
                                requestStream.Write(buffer, 0, bytesRead);
                            }
                        }
                    }
                }
            }

            // Write trailer and close stream
            requestStream.Write(trailer, 0, trailer.Length);
            requestStream.Close();

            string result = null;

            using (var response = await request.GetResponseAsync())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        result = reader.ReadToEnd();
                    }
                }
            }

            return result;
        }
    }
}
