using System.Linq;
using Queryoont.Extensions;
using Queryoont.Models;
using SqlKata;
using Xunit;

namespace Queryoont.Test.Extensions
{
    public class QueryExtensionsTests
    {
        [Fact]
        public void Should_add_select_on_valid_query()
        {
            // Arrange
            var query = new Query("table1");

            // Act
            var result = query.AddSelect(new string[] { "Id", "Name" });

            // Assert
            var s = result.Clauses.Where(p => p.Component == "select").Count();

            Assert.Equal(2, s);
        }

        [Fact]
        public void Should_add_where_condition_on_valid_query()
        {
            // Arrange
            var query = new Query("table1");
            var filter = new QueryFilter { Type = "Where", Field = "aa", Oper = "=", Value = "bar" };

            // Act
            var result = query.AddFilter(filter);

            // Assert
            var s = result.Clauses.Where(p => p.Component == "where").Count();

            Assert.Equal(1, s);
        }

        [Fact]
        public void Should_add_orwhere_condition_on_valid_query()
        {
            // Arrange
            var query = new Query("table1");
            var filter = new QueryFilter { Type = "OrWhere", Field = "aa", Oper = "=", Value = "bar" };

            // Act
            var result = query.AddFilter(filter);

            // Assert
            var s = result.Clauses.Where(p => p.Component == "where").Count();

            Assert.Equal(1, s);
        }

        [Fact]
        public void Should_add_wherenot_condition_on_valid_query()
        {
            // Arrange
            var query = new Query("table1");
            var filter = new QueryFilter { Type = "WhereNot", Field = "aa", Oper = "=", Value = "b" };

            // Act
            var result = query.AddFilter(filter);

            // Assert
            var s = result.Clauses.Where(p => p.Component == "where").Count();

            Assert.Equal(1, s);
        }

        [Fact]
        public void Should_add_orwherenot_condition_on_valid_query()
        {
            // Arrange
            var query = new Query("table1");
            var filter = new QueryFilter { Type = "OrWhereNot", Field = "aa", Oper = "=", Value = "1" };

            // Act
            var result = query.AddFilter(filter);

            // Assert
            var s = result.Clauses.Where(p => p.Component == "where").Count();

            Assert.Equal(1, s);
        }

        [Fact]
        public void Should_add_wherenull_condition_on_valid_query()
        {
            // Arrange
            var query = new Query("table1");
            var filter = new QueryFilter { Type = "WhereNull", Field = "aa" };

            // Act
            var result = query.AddFilter(filter);

            // Assert
            var s = result.Clauses.Where(p => p.Component == "where").Count();

            Assert.Equal(1, s);
        }

        [Fact]
        public void Should_add_orwherenull_condition_on_valid_query()
        {
            // Arrange
            var query = new Query("table1");
            var filter = new QueryFilter { Type = "OrWhereNull", Field = "aa" };

            // Act
            var result = query.AddFilter(filter);

            // Assert
            var s = result.Clauses.Where(p => p.Component == "where").Count();

            Assert.Equal(1, s);
        }

        [Fact]
        public void Should_add_wheretrue_condition_on_valid_query()
        {
            // Arrange
            var query = new Query("table1");
            var filter = new QueryFilter { Type = "WhereTrue", Field = "aa" };

            // Act
            var result = query.AddFilter(filter);

            // Assert
            var s = result.Clauses.Where(p => p.Component == "where").Count();

            Assert.Equal(1, s);
        }

        [Fact]
        public void Should_add_wherefalse_condition_on_valid_query()
        {
            // Arrange
            var query = new Query("table1");
            var filter = new QueryFilter { Type = "WhereFalse", Field = "aa" };

            // Act
            var result = query.AddFilter(filter);

            // Assert
            var s = result.Clauses.Where(p => p.Component == "where").Count();

            Assert.Equal(1, s);
        }

        [Fact]
        public void Should_add_wherein_condition_on_valid_query()
        {
            // Arrange
            var query = new Query("table1");
            var filter = new QueryFilter { Type = "WhereIn", Field = "aa", Values = new object[] { 1, 2, 3, 4 } };

            // Act
            var result = query.AddFilter(filter);

            // Assert
            var s = result.Clauses.Where(p => p.Component == "where").Count();

            Assert.Equal(1, s);
        }

        [Fact]
        public void Should_add_wherenotin_condition_on_valid_query()
        {
            // Arrange
            var query = new Query("table1");
            var filter = new QueryFilter { Type = "WhereNotIn", Field = "aa", Values = new object[] { 1, 2, 3, 4 } };

            // Act
            var result = query.AddFilter(filter);

            // Assert
            var s = result.Clauses.Where(p => p.Component == "where").Count();

            Assert.Equal(1, s);
        }
    }
}