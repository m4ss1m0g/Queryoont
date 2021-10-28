using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Queryoont.Extensions;
using Queryoont.Models;
using SqlKata;
using SqlKata.Execution;

namespace Queryoont.Infrastructure
{
    public class QueryExecution : IQueryExecution
    {
        private readonly QueryFactory _db;

        public QueryExecution(QueryFactory db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<IEnumerable<object>> GetDataAsync(Query query, QueryModel model, Type actions)
        {

            var result = await _db.FromQuery(query).ApplyFilter(model).GetAsync();

            // Uf there is a filter, then invoke
            if (actions != null && actions.GetInterfaces().Any(p => p.Name == "IFilterActions`1"))
            {
                var instance = Activator.CreateInstance(actions);
                var method = actions.GetMethod("AfterQueryAsync");

                dynamic awaitable = method.Invoke(instance, new object[] { result });
                await awaitable;
                var r = awaitable.GetAwaiter().GetResult();
                return r;
            }

            return result;
        }
    }
}