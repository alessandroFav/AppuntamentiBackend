using Microsoft.AspNetCore.Mvc;
using provaProgetto.Controllers;
using provaProgetto.Dipendenze;
using provaProgetto.Models;

namespace provaProgetto.Middlewares
{
    public class ParamsMiddleware
    {
        private readonly RequestDelegate _next;

        public ParamsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IGestioneDati dataService)
        {
            if (context.Items["user"] != null)
            {
                var route = context.GetRouteData();
                Utente user = (Utente)context.Items["user"]!;
                if (route.Values.TryGetValue("id", out var id))
                {
                    if (user!.id != Convert.ToInt32(id))
                    {
                        context.Response.StatusCode = 403;
                        await context.Response.WriteAsync("Forbidden");
                        return;
                    }
                }
                if(route.Values.TryGetValue("idAppuntamento", out var idApp))
                {
                    Appuntamento? app = dataService.GetAppuntamento(Convert.ToInt32(idApp));
                    if (app == null)
                    {
                        context.Response.StatusCode = 403;
                        await context.Response.WriteAsync("Forbidden");
                        return;
                    }
                    if(app!.idUtente != user!.id)
                    {
                        context.Response.StatusCode = 403;
                        await context.Response.WriteAsync("Forbidden");
                        return;
                    }
                }
                if(route.Values.TryGetValue("idEvento", out var idEvent))
                {
                    Evento? evento = dataService.GetEvento(Convert.ToInt32(idEvent));
                    if (evento == null)
                    {
                        context.Response.StatusCode = 403;
                        await context.Response.WriteAsync("Forbidden");
                        return;
                    }
                    if (evento!.idOrganizzatore != user!.id)
                    {
                        context.Response.StatusCode = 403;
                        await context.Response.WriteAsync("Forbidden");
                        return;
                    }
                }
            }
            await _next(context);
        }
    }
    public static class ParamsMiddlewareExtension
    {
        public static IApplicationBuilder UseParamsMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ParamsMiddleware>();
        }
    }
}
