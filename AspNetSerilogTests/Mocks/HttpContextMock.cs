using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Text;

namespace AspNetSerilogTests.Mocks
{
    public static class HttpContextMock
    {
        public static DefaultHttpContext DefaultHttpContext()
        {
            var httpContext = new DefaultHttpContext();

            var requestBody = new
            {
                Test = "1",
                Card = new
                {
                    Number = "4622943127049865",
                    Password = "somepass#here2",
                    Email = "email@test.com",
                    Card = new
                    {
                        Number = "4622943127049865",
                        Password = "somepass#here2"
                    },
                    Teste = new
                    {
                        Card = new
                        {
                            Number = "4622943127049865",
                            Password = "somepass#here2",
                            Email = "email@test.com"
                        }
                    }
                }
            };

            string jsonString = JsonConvert.SerializeObject(requestBody);
            byte[] byteArray = Encoding.UTF8.GetBytes(jsonString);
            var stream = new MemoryStream(byteArray);
            httpContext.Request.Body = stream;

            httpContext.Request.Headers.Add("Content-Type", "json");

            return httpContext;
        }
    }
}
