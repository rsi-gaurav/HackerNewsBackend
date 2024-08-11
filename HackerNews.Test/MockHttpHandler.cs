using Moq.Protected;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using HackerNews.Domain.DTO;
using System.Net.Http;

namespace HackerNews.Test
{
    public class MockHttpHandler
    {
        /// <summary>
        /// Gets the message handler.
        /// </summary>
        /// <returns>The mock HTTP message handler.</returns>
        public static Mock<HttpMessageHandler> GetMessageHandler1()
        {
            var mockHandler = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new List<int> { 1, 2, 3 }))
            };
            mockHandler
               .Protected()
               .Setup<Task<HttpResponseMessage>>
               (
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(response);
            return mockHandler;
        }

        /// <summary>
        /// Gets the message handler.
        /// </summary>
        /// <returns>The mock HTTP message handler.</returns>
        public static Mock<HttpMessageHandler> GetMessageHandler2()
        {
            var mockHandler = new Mock<HttpMessageHandler>();

            var storyContent = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new HackerNewsDTO { id = 1, title = "Test Story" }))
            };

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(storyContent);

            return mockHandler;
        }
    }
}
