using Newtonsoft.Json;

namespace Ecommerce.Api.Errors;


/// <summary>
/// Clase para el manejo de errores
/// </summary>
public class CodeErrorReponse 
{

        [JsonProperty(PropertyName = "StatusCode")]
        public int StatusCode { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string[]? Message { get; set; }

    public CodeErrorReponse(int statusCode, string[]? message = null)
    {
        StatusCode = statusCode;
        if (message is null)
        {
            Message = new string[0];
            var text = GetDefaultMessageStatusCode(statusCode);
            Message[0] = text;
        } 
        else 
        {
            Message = message;
        }
    }



    private string GetDefaultMessageStatusCode(int statusCode)
    {
        return statusCode switch 
        {
            400 => "El request enviado tiene errores",
            401 => "No tienes autorizacion para este recurso",
            404 => "No se encontro el recurso solicitado",
            500 => "Se presento un error en el servidor",
            _ => string.Empty
        };
    }
}