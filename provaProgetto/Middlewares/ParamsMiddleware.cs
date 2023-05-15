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

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Items["user"] != null)
            {
                var route = context.GetRouteData();
                if (route.Values.TryGetValue("id", out var id))
                {
                    Utente user = (Utente)context.Items["user"]!;
                    if (user!.id != Convert.ToInt32(id))
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
