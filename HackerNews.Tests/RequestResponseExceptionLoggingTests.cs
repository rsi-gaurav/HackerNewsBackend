using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HackerNews.API.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ApplicationInsights;
using Moq;
using Xunit;

namespace HackerNews.Tests
{
    /// <summary>
    /// Unit tests for the RequestResponseExceptionLogging middleware.
    /// </summary>
    public class RequestResponseExceptionLoggingTests
    {
        /// <summary>
        /// Verifies that the Invoke method calls the next middleware.
        /// </summary>
        [Fact]
        public async Task Invoke_Should_CallNextMiddleware()
        {
            // Arrange
            var insightsMock = new Mock<TelemetryClient>();
            var nextMock = new Mock<RequestDelegate>();
            var middleware = new RequestResponseExceptionLogging(insightsMock.Object, nextMock.Object);
            var httpContext = new DefaultHttpContext();

            // Act
            await middleware.Invoke(httpContext);

            // Assert
            nextMock.Verify(next => next(httpContext), Times.Once);
        }

        /// <summary>
        /// Verifies that the Invoke method tracks the exception and sets the status code when an exception occurs.
        /// </summary>
        [Fact]
        public async Task Invoke_Should_TrackExceptionAndSetStatusCode_WhenExceptionOccurs()
        {
            // Arrange
            var insightsMock = new Mock<TelemetryClient>();
            var nextMock = new Mock<RequestDelegate>();
            var middleware = new RequestResponseExceptionLogging(insightsMock.Object, nextMock.Object);
            var httpContext = new DefaultHttpContext();
            var exception = new Exception("Test exception");

            nextMock.Setup(next => next(httpContext)).ThrowsAsync(exception);

            // Act
            await middleware.Invoke(httpContext);

            // Assert
            insightsMock.Verify(insights => insights.TrackException(exception, null, null), Times.Once);
            Assert.Equal(StatusCodes.Status500InternalServerError, httpContext.Response.StatusCode);
        }
    }
}
