using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Queryoont.Infrastructure;
using Queryoont.Interfaces;
using Queryoont.Models;
using SqlKata;
using Xunit;

namespace Queryoont.Test.Infrastructure
{
    public class QueryExecutionTests : DbFixture
    {
        public class Foo
        {
            public int Idx { get; set; }
        }

        public class CustomFilter : IFilterActions<Foo>
        {
            public async Task<IEnumerable<Foo>> AfterQueryAsync(IEnumerable<dynamic> rows)
            {
                var l = new List<Foo>();

                foreach (var item in rows)
                {
                    l.Add(new Foo { Idx = item.Id });
                }

                return await Task.FromResult(l.AsEnumerable());

            }
        }

        [Fact]
        public void Should_execute_the_filter()
        {
            var e = new QueryExecution(Db);

            // DO NOT USE Task on Fact otherwise raise an error!
            var result = e.GetDataAsync(new Query("Table1"), new QueryModel(), typeof(CustomFilter)).Result;

            Assert.NotNull(result);
        }
    }
}