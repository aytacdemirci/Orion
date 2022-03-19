
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Orion.API.SeedWork.CustomProblemDetails;
using Orion.Application.SeedWork.CustomExceptions;
using Orion.Domain.SeedWork;
using System.Net;
using System.Text;
using System.Text.Json;


namespace Orion.API.CustomMiddlewares
{
    public class ErrorHandlerAndLogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ErrorHandlerAndLogMiddleware> _logger;

        public ErrorHandlerAndLogMiddleware(RequestDelegate next, IWebHostEnvironment env, ILogger<ErrorHandlerAndLogMiddleware> logger)
        {
            _next = next;
            _env = env;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                httpContext.Request.EnableBuffering();
                await _next(httpContext);
            }
            catch (InvalidRequestException ex)
            {
                await LogResponseAsync(ex, httpContext, LogLevel.Error);
                var problemDetails = GetBadRequestProblemDetails(ex);
                var response = httpContext.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                await response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(problemDetails));
            }
            catch (BusinessRuleValidationException ex)
            {
                await LogResponseAsync(ex, httpContext, LogLevel.Error);
                var problemDetails = GetBusinessRuleValidationProblemDetails(ex);
                var response = httpContext.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                await response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(problemDetails));
            }
            catch (Exception ex)
            {
                await LogResponseAsync(ex, httpContext, LogLevel.Error);
                var response = httpContext.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                ProblemDetails problemDetails = GetProblemDetails(ex);

                await response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(problemDetails));
            }
        }
        private async Task LogResponseAsync(Exception exception, HttpContext httpContext, LogLevel logLevel)
        {
            string requestBody = string.Empty;
            if (httpContext.Request.Body.CanSeek)
            {
                httpContext.Request.Body.Seek(0, System.IO.SeekOrigin.Begin);
                using (var sr = new System.IO.StreamReader(httpContext.Request.Body))
                {
                    requestBody = JsonConvert.SerializeObject(sr.ReadToEndAsync());
                }
            }

            StringValues authorization;
            httpContext.Request.Headers.TryGetValue("Authorization", out authorization);

            var customeDetails = new StringBuilder();
            customeDetails
                .AppendFormat("\n  Service URL     :").Append(httpContext.Request.Path.ToString())
                .AppendFormat("\n  Request Method  :").Append(httpContext.Request?.Method)
                .AppendFormat("\n  Request Body    :").Append(requestBody)
                .AppendFormat("\n  Authorization   :").Append(authorization)
                .AppendFormat("\n  Content-Type    :").Append(httpContext.Request.Headers["Content-Type"].ToString())
                .AppendFormat("\n  Cookie          :").Append(httpContext.Request.Headers["Cookie"].ToString())
                .AppendFormat("\n  Host            :").Append(httpContext.Request.Headers["Host"].ToString())
                .AppendFormat("\n  Referer         :").Append(httpContext.Request.Headers["Referer"].ToString())
                .AppendFormat("\n  Origin          :").Append(httpContext.Request.Headers["Origin"].ToString())
                .AppendFormat("\n  User-Agent      :").Append(httpContext.Request.Headers["User-Agent"].ToString())
                .AppendFormat("\n  Error Message    :").Append(exception.Message);

            _logger.Log(logLevel, exception, customeDetails.ToString());

            if (httpContext.Response.HasStarted)
            {
                _logger.LogError("the response has already started, the http status code middleware will not be executed");
                return;
            }
        }
        private ProblemDetails GetProblemDetails(Exception ex)
        {
            string traceId = Guid.NewGuid().ToString();

            if (_env.EnvironmentName == "Development")
            {
                return new ProblemDetails
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = "https://httpstatuses.com/500",
                    Title = ex.Message,
                    Detail = ex.ToString(),
                    Instance = traceId
                };
            }
            else
            {
                return new ProblemDetails
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = "https://httpstatuses.com/500",
                    Title = "Something went wrong. Please try after some time",
                    Detail = @"We apologize for inconvenience. Please let us know about the error at support@orion.com. Include traceId: {traceId} in email",
                    Instance = traceId
                };
            }
        }

        private InvalidRequestProblemDetails GetBadRequestProblemDetails(InvalidRequestException ex)
        {
            string traceId = Guid.NewGuid().ToString();

            var invalidRequestProblemDetails = new InvalidRequestProblemDetails(ex, traceId);

            return invalidRequestProblemDetails;
        }

        private BusinessRuleValidationProblemDetails GetBusinessRuleValidationProblemDetails(BusinessRuleValidationException ex)
        {
            string traceId = Guid.NewGuid().ToString();

            var businessRuleValidationProblemDetails = new BusinessRuleValidationProblemDetails(ex, traceId);

            return businessRuleValidationProblemDetails;
        }

    }
}
