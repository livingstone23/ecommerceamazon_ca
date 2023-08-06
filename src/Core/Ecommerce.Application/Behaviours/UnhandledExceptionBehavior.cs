using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Application.Behaviours;


public class UnhandledExceptionBehavior<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{

    private readonly ILogger<TRequest> _logger;

    public UnhandledExceptionBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try 
        {
            return await next();
        }
        catch(Exception ex)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogError(ex, "Application Request: sucedio una exception para el request {Name} {@Request}", requestName, request);
            throw new Exception("Error en la aplicacion", ex);
        }
    }

}
