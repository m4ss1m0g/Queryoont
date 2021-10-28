using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Queryoont.Api.Models;
using Queryoont.Interfaces;

namespace Queryoont.Api.Actions
{

    public class GetQueryActions : IFilterActions<Customer>
    {
        public Task<IEnumerable<Customer>> AfterQueryAsync(IEnumerable<dynamic> rows)
        {
            var list = new List<Customer>();

            foreach (var item in rows)
            {
                list.Add(new Customer { Id = (int)item.Id, Name = "bar" });
            }

            return Task.FromResult(list.AsEnumerable());
        }
    }
}