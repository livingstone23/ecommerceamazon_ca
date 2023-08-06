


using System.Net;
using Ecommerce.Api.Errors;
using Ecommerce.Application.Exceptions;
using Newtonsoft.Json;

namespace Ecommerce.Api.Middlewares;


public class ExceptionMiddleware 
{

    //Request que envia el cliente y interceptamos.
    private readonly RequestDelegate _next;

    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    //Metodo que evalua el request al cliente y envia respuesta de este.
    public async Task InvokeAsync(HttpContext context)
    {
        try 
        {
            await _next(context);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var result = string.Empty;

            switch(ex)
            {
                case NotFoundException notFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    break;
                case FluentValidation.ValidationException validationException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    var errors = validationException.Errors.Select(e => e.ErrorMessage).ToArray();
                    var validationJsons = JsonConvert.SerializeObject(errors);
                    result = JsonConvert.SerializeObject(new CodeErrorException(statusCode, errors, validationJsons));
                    break;
                case BadRequestException badRequestException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    break;

                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            if (string.IsNullOrEmpty(result))
            {
                result = JsonConvert.SerializeObject(new CodeErrorException( statusCode, new string[] { ex.Message }, ex.StackTrace));
            }

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(result);

        }
    }

}