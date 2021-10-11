using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Queryoont.Attributes;
using Queryoont.Infrastructure;

namespace Queryoont.Middleware
{
    internal class SqlQuery
    {
        private readonly RequestDelegate _next;
        private readonly IQueryHandler _handler;

        public SqlQuery(RequestDelegate next, IQueryHandler handler)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();

            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            var attribute = endpoint?.Metadata.GetMetadata<QueryoontAttribute>();
            if (attribute != null && attribute.Query != null)
            {
                var bodyStr = await GetRequestBody(context);
                var result = await _handler.Handle(bodyStr, attribute.Query);

                if (result != null)
                {
                    await context.Response.WriteAsync(result);
                    return;
                }
            }

            await _next(context);

        }

        private static async Task<string> GetRequestBody(HttpContext context)
        {
            // Rewind to initial position
            // The `leaveOpen` should be `true` if there's another middleware using this after or if the action going to be invoked AFTER this middleware            var req = context.Request;
            var req = context.Request;

            req.Body.Position = 0;

            using StreamReader reader
                = new StreamReader(req.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, leaveOpen: true);

            return await reader.ReadToEndAsync();
        }
    }
}