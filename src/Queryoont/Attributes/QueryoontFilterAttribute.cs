using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Queryoont.Extensions;
using Queryoont.Models;
using Queryoont.Serialization;
using SqlKata;
using SqlKata.Execution;

namespace Queryoont.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class QueryoontFilterAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable => false;

        public QueryoontFilterAttribute()
        {
        }

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var db = serviceProvider.GetService<QueryFactory>();
            var json = serviceProvider.GetService<IJsonSerializer>();
            return new QueryoontActionFilter(db, json);
        }

        private class QueryoontActionFilter : IAsyncActionFilter
        {
            private readonly QueryFactory _db;
            private readonly IJsonSerializer _json;

            public QueryoontActionFilter(QueryFactory db, IJsonSerializer json)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
                _json = json ?? throw new ArgumentNullException(nameof(json));
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                // Before the action executes.
                var body = GetRequestBody(context.HttpContext);
                //var model = JsonConvert.DeserializeObject<QueryModel>(body);
                var model = _json.Deserialize<QueryModel>(body);

                // next() calls the action method.
                var resultContext = await next();
                // await next();
                // resultContext.Result is set.

                // After the action executes.
                var actionResult = (ObjectResult)resultContext.Result;

                if (actionResult != null && actionResult.Value is Query)
                {
                    var query = actionResult.Value as Query;
                    IEnumerable<dynamic> result = await _db.FromQuery(query).ApplyFilter(model).GetAsync();

                    var content = new ContentResult
                    {
                        Content = _json.Serialize(result),
                        ContentType = "application/json"
                    };
                    
                    resultContext.Result = content;// new JsonResult(result);
                }
            }

            private static string GetRequestBody(HttpContext context)
            {
                // Rewind to initial position
                context.Request.EnableBuffering();

                var req = context.Request;

                req.Body.Position = 0;

                using StreamReader reader
                    = new StreamReader(req.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, leaveOpen: true);

                return reader.ReadToEndAsync().GetAwaiter().GetResult();
            }

        }
    }
}