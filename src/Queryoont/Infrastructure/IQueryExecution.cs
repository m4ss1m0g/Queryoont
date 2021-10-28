using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Queryoont.Models;
using SqlKata;

namespace Queryoont.Infrastructure
{
    public interface IQueryExecution
    {
        Task<IEnumerable<object>> GetDataAsync(Query query, QueryModel model, Type actions);
    }
}