using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Queryoont.Infrastructure;
using Queryoont.Models;
using Queryoont.Serialization;
using SqlKata;

namespace Queryoont.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class QueryoontFilterAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable => false;

        public Type FilterAction { get; set; }

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var json = serviceProvider.GetService<IJsonSerializer>();
            var execution = serviceProvider.GetService<IQueryExecution>();
            return new QueryoontActionFilter(json, execution, FilterAction);
        }

        public class QueryoontActionFilter : IAsyncActionFilter
        {
            private readonly IJsonSerializer _json;
            private readonly IQueryExecution _execute;
            private readonly Type _filterAction;

            public QueryoontActionFilter(IJsonSerializer json, IQueryExecution execute, Type filterAction)
            {
                _json = json ?? throw new ArgumentNullException(nameof(json));
                _execute = execute ?? throw new ArgumentNullException(nameof(execute));
                _filterAction = filterAction;
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
                    
                    // Magic happends here !
                    var result = await _execute.GetDataAsync(query, model, _filterAction);
                    
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