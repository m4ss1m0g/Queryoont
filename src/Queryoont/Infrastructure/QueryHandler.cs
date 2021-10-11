using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SqlKata;
using SqlKata.Execution;
using Queryoont.Extensions;
using Queryoont.Models;

namespace Queryoont.Infrastructure
{
    public class QueryHandler : IQueryHandler
    {
        private readonly IMediator _mediator;
        private readonly QueryFactory _db;

        public QueryHandler(IMediator mediator, QueryFactory db)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<string> Handle(string bodyStr, Type requestType)
        {
            QueryModel model = JsonConvert.DeserializeObject<QueryModel>(bodyStr);

            // Create and send the command
            var request = Activator.CreateInstance(requestType);
            var query = await _mediator.Send(request);

            // If the result of the command is a SqlKata Query
            if (query is Query)
            {
                // Apply filter to the query
                IEnumerable<dynamic> result = await _db.FromQuery(query as Query).ApplyFilter(model).GetAsync();

                // Return result
                return JsonConvert.SerializeObject(result);
            }

            return null;
        }
    }
}