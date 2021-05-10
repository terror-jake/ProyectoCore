using System;
using System.Net;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebAPI.Middleware
{
    public class ManejadorErrorMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ManejadorErrorMiddleware> logger;

        public ManejadorErrorMiddleware(RequestDelegate _next, ILogger<ManejadorErrorMiddleware> _logger)
        {
            next = _next;
            logger = _logger;
        }

        // REQUIERE LLAMARSE Invoke o InvokeAsync
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await ManejadorExcepcionAsincrono(context, ex, logger);
            }
        }

        private async Task ManejadorExcepcionAsincrono(HttpContext context, Exception ex, ILogger<ManejadorErrorMiddleware> logger)
        {
            object errores = null;
            switch(ex)
            {
                case ManejadorExcepcion me : 
                    logger.LogError(ex, "Manejador Error");
                    errores = me.Errores;
                    context.Response.StatusCode = (int)me.Codigo;
                    break;

                case Exception e:
                    logger.LogError(ex, "Error de Servidor");
                    errores = string.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            // Tipo de respuesta
            context.Response.ContentType = "application/json";

            if(errores != null)
            {
                // Requiere Newtonsoft.Json NuGet
                var resultados = JsonConvert.SerializeObject(new { errores });
                await context.Response.WriteAsync(resultados);
            }



        }
    }
}