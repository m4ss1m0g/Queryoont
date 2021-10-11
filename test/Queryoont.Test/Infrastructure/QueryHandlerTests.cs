using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SqlKata;
using SqlKata.Execution;
using Queryoont.Infrastructure;
using Queryoont.Models;
using Xunit;

namespace Queryoont.Test.Infrastructure
{
    public class QueryHandlerTests : DbFixture
    {
        class Cmd : IRequest<Query> { }

        public static IEnumerable<object[]> GetValues()
        {
            yield return new object[] {
                JsonConvert.SerializeObject(new QueryModel{Select = new string[] { "Id" } } ),
                JsonConvert.SerializeObject(Enumerable.Range(1,50).ToList().Select(p => new { Id = p}))
            };

            yield return new object[] {
                JsonConvert.SerializeObject(new QueryModel{Select = new string[] { "Id", "Name" } } ),
                JsonConvert.SerializeObject(Enumerable.Range(1,50).ToList().Select(p => new { Id = p, Name = $"Foo {p}"}))
            };

            yield return new object[] {
                JsonConvert.SerializeObject(new QueryModel{Select = new string[] { "Id", "Name" }, Filter = new QueryFilter[] { new QueryFilter{ Type = "Where", Field = "Id", Oper = "<", Value = 10 } } } ),
                JsonConvert.SerializeObject(Enumerable.Range(1,9).ToList().Select(p => new { Id = p, Name = $"Foo {p}"}))
            };

            yield return new object[] {
                JsonConvert.SerializeObject(new {Foo = "Bar"}),
                JsonConvert.SerializeObject(Enumerable.Range(1,50).ToList().Select(p => new { Id = p, Name = $"Foo {p}"}))
            };
        }

        [Theory]
        [MemberData(nameof(GetValues))]
        public async Task Should_return_the_json_values_with_valid_body(string json, string result)
        {
            // Arrange
            FillDatabase();
            var query = new Query("table1");
            var mediator = new Mock<IMediator>();
            mediator.Setup(p => p.Send(It.IsAny<object>(), default)).ReturnsAsync(query);

            // Act
            var handler = new QueryHandler(mediator.Object, Db);
            var resultHandler = await handler.Handle(json, typeof(Cmd));

            // Assert
            Assert.Equal(result, resultHandler);
        }

        [Fact]
        public async Task Should_return_null_whitout_query_return_from_mediator()
        {
            var mediator = new Mock<IMediator>();
            mediator.Setup(p => p.Send(It.IsAny<object>(), default)).ReturnsAsync(new { Foo = "Bar" });

            // Act
            var handler = new QueryHandler(mediator.Object, Db);
            var resultHandler = await handler.Handle("", typeof(Cmd));

            // Assert
            Assert.Null(resultHandler);
        }

        private void FillDatabase()
        {
            var tasks = new List<Task>();
            for (int i = 1; i < 51; i++)
            {
                tasks.Add(Db.Query("table1").InsertAsync(new { Id = i, Name = $"Foo {i}" }));
            }

            Task.WaitAll(tasks.ToArray());
        }
    }
}