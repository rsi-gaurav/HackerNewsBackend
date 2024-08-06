using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Threading;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HackerNews.API.Middleware
{
    public class RequestResponseExceptionLogging
    {
        // <summary>
        /// Gets the insights.
        /// </summary>
        private TelemetryClient _insights { get; }

        /// <summary>
        /// The next
        /// </summary>
        readonly RequestDelegate next;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestResponseExceptionLogging"/> class.
        /// </summary>
        /// <param name="insights">The insights.</param>
        /// <param name="next">The next.</param>
        public RequestResponseExceptionLogging(TelemetryClient insights, RequestDelegate next)
        {
            this.next = next;
            _insights = insights;
        }

        #region Invoke
        /// <summary>
        /// Invoke as an asynchronous operation.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                var request = httpContext.Request;

                var requestResponse = new Dictionary<string, string>();
                string eventName = string.Empty;
                await next(httpContext);
            }
            catch (Exception ex)
            {
              
                _insights.TrackException(ex);
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                var problemDetails = new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Status = (int)StatusCodes.Status500InternalServerError,             
                    //Detail = $"Internal server error occured, traceId : {ex.Message}"
                };
                await httpContext.Response.WriteAsJsonAsync(problemDetails);
            }
        }
        #endregion
    }
}
