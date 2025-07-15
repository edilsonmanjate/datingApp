using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware;

public class ExceptionMeddleware(RequestDelegate next, ILogger<ExceptionMeddleware> logger, IHostEnvironment env)
{
     public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                statusCode = context.Response.StatusCode,
                message = "This is a server error",
                details = env.IsDevelopment() ? ex.ToString() : null
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            // Aqui está o ponto chave: serialize diretamente para o Response.Body
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }

}
