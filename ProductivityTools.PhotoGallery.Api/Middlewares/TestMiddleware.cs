using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ProductivityTools.PhotoGallery.Api.Middlewares
{
    public class PromoMiddleware
    {
        private readonly RequestDelegate next;

        public PromoMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext ctx)
        {
            await ctx.Response.WriteAsync("Invalid User Key");
            //await next(ctx);
        }
    }
}
