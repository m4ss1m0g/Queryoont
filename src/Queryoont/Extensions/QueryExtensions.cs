using System;
using Dawn;
using SqlKata;
using Queryoont.Models;

namespace Queryoont.Extensions
{
    public static class QueryExtensions
    {
        public static Query ApplyFilter(this Query baseQuery, QueryModel model)
        {
            if (model != null)
            {
                if (model.Select != null && model.Select.Length > 0)
                {
                    baseQuery.AddSelect(model.Select);
                }

                if (model.Filter != null)
                {
                    foreach (var item in model.Filter)
                    {
                        baseQuery.AddFilter(item);
                    }
                }
            }

            return baseQuery;
        }

        public static Query AddSelect(this Query query, string[] select)
        {
            Guard.Argument(query, nameof(query)).NotNull();
            Guard.Argument(select, nameof(select)).NotNull().NotEmpty();

            query.Select(select);

            return query;
        }

        public static Query AddFilter(this Query query, QueryFilter filter)
        {
            Guard.Argument(query, nameof(query)).NotNull();
            Guard.Argument(filter, nameof(filter)).NotNull();

            Guard.Argument(filter.Type, nameof(filter.Type)).NotEmpty().NotNull();

            switch (filter.Type)
            {
                case "WhereCondition":
                    query.AddWhereCondition(filter);
                    break;
                case "Where":
                    query.AddWhere(filter);
                    break;
                case "OrWhere":
                    query.AddOrWhere(filter);
                    break;
                case "WhereNot":
                    query.AddWhereNot(filter);
                    break;
                case "OrWhereNot":
                    query.AddOrWhereNot(filter);
                    break;
                case "WhereNull":
                    query.AddWhereNull(filter);
                    break;
                case "OrWhereNull":
                    query.AddOrWhereNull(filter);
                    break;
                case "WhereTrue":
                    query.AddWhereTrue(filter);
                    break;
                case "WhereFalse":
                    query.AddWhereFalse(filter);
                    break;
                case "WhereIn":
                    query.AddWhereIn(filter);
                    break;
                case "WhereNotIn":
                    query.AddWhereNotIn(filter);
                    break;
                default:
                    break;
            }

            return query;
        }

        private static Query AddWhereCondition(this Query query, QueryFilter filter)
        {
            Guard.Argument(query, nameof(query)).NotNull();
            Guard.Argument(filter, nameof(filter)).NotNull();

            Guard.Argument(filter.Type, nameof(filter.Type)).Equal("WhereCondition");

            Guard.Argument(filter.Condition, nameof(filter.Condition)).NotNull().NotEmpty();

            query.Where(o =>
            {
                foreach (var item in filter.Condition)
                {
                    o.AddFilter(item);
                }
                return o;
            });

            return query;
        }

        private static Query AddWhere(this Query query, QueryFilter filter)
        {
            Guard.Argument(query, nameof(query)).NotNull();
            Guard.Argument(filter, nameof(filter)).NotNull();

            Guard.Argument(filter.Type, nameof(filter.Type)).Equal("Where");

            Guard.Argument(filter.Field, nameof(filter.Field)).NotEmpty().NotNull();
            Guard.Argument(filter.Oper, nameof(filter.Oper)).NotEmpty().NotNull();
            Guard.Argument(filter.Value, nameof(filter.Value)).NotNull();

            query.Where(filter.Field, filter.Oper, filter.Value);

            return query;
        }

        private static Query AddOrWhere(this Query query, QueryFilter filter)
        {
            Guard.Argument(query, nameof(query)).NotNull();
            Guard.Argument(filter, nameof(filter)).NotNull();

            Guard.Argument(filter.Type, nameof(filter.Type)).Equal("OrWhere");

            Guard.Argument(filter.Field, nameof(filter.Field)).NotEmpty().NotNull();
            Guard.Argument(filter.Oper, nameof(filter.Oper)).NotEmpty().NotNull();
            Guard.Argument(filter.Value, nameof(filter.Value)).NotNull();

            query.OrWhere(filter.Field, filter.Oper, filter.Value);

            return query;
        }

        private static Query AddWhereNot(this Query query, QueryFilter filter)
        {
            Guard.Argument(query, nameof(query)).NotNull();
            Guard.Argument(filter, nameof(filter)).NotNull();

            Guard.Argument(filter.Type, nameof(filter.Type)).Equal("WhereNot");

            Guard.Argument(filter.Field, nameof(filter.Field)).NotEmpty().NotNull();
            Guard.Argument(filter.Oper, nameof(filter.Oper)).NotEmpty().NotNull();
            Guard.Argument(filter.Value, nameof(filter.Value)).NotNull();

            query.WhereNot(filter.Field, filter.Oper, filter.Value);

            return query;
        }

        private static Query AddOrWhereNot(this Query query, QueryFilter filter)
        {
            Guard.Argument(query, nameof(query)).NotNull();
            Guard.Argument(filter, nameof(filter)).NotNull();

            Guard.Argument(filter.Type, nameof(filter.Type)).Equal("OrWhereNot");

            Guard.Argument(filter.Field, nameof(filter.Field)).NotEmpty().NotNull();
            Guard.Argument(filter.Oper, nameof(filter.Oper)).NotEmpty().NotNull();
            Guard.Argument(filter.Value, nameof(filter.Value)).NotNull();

            query.OrWhereNot(filter.Field, filter.Oper, filter.Value);

            return query;
        }

        private static Query AddWhereNull(this Query query, QueryFilter filter)
        {
            Guard.Argument(query, nameof(query)).NotNull();
            Guard.Argument(filter, nameof(filter)).NotNull();

            Guard.Argument(filter.Type, nameof(filter.Type)).Equal("WhereNull");

            Guard.Argument(filter.Field, nameof(filter.Field)).NotEmpty().NotNull();

            query.WhereNull(filter.Field);

            return query;
        }

        private static Query AddOrWhereNull(this Query query, QueryFilter filter)
        {
            Guard.Argument(query, nameof(query)).NotNull();
            Guard.Argument(filter, nameof(filter)).NotNull();

            Guard.Argument(filter.Type, nameof(filter.Type)).Equal("OrWhereNull");

            Guard.Argument(filter.Field, nameof(filter.Field)).NotEmpty().NotNull();

            query.OrWhereNull(filter.Field);

            return query;
        }

        private static Query AddWhereTrue(this Query query, QueryFilter filter)
        {
            Guard.Argument(query, nameof(query)).NotNull();
            Guard.Argument(filter, nameof(filter)).NotNull();

            Guard.Argument(filter.Type, nameof(filter.Type)).Equal("WhereTrue");

            Guard.Argument(filter.Field, nameof(filter.Field)).NotEmpty().NotNull();

            query.WhereTrue(filter.Field);

            return query;
        }

        private static Query AddWhereFalse(this Query query, QueryFilter filter)
        {
            Guard.Argument(query, nameof(query)).NotNull();
            Guard.Argument(filter, nameof(filter)).NotNull();

            Guard.Argument(filter.Type, nameof(filter.Type)).Equal("WhereFalse");

            Guard.Argument(filter.Field, nameof(filter.Field)).NotEmpty().NotNull();

            query.WhereFalse(filter.Field);

            return query;
        }

        private static Query AddWhereIn(this Query query, QueryFilter filter)
        {
            Guard.Argument(query, nameof(query)).NotNull();
            Guard.Argument(filter, nameof(filter)).NotNull();

            Guard.Argument(filter.Type, nameof(filter.Type)).Equal("WhereIn");

            Guard.Argument(filter.Field, nameof(filter.Field)).NotEmpty().NotNull();
            Guard.Argument(filter.Values, nameof(filter.Values)).NotEmpty().NotNull();

            query.WhereIn(filter.Field, filter.Values);

            return query;
        }

        private static Query AddWhereNotIn(this Query query, QueryFilter filter)
        {
            Guard.Argument(query, nameof(query)).NotNull();
            Guard.Argument(filter, nameof(filter)).NotNull();

            Guard.Argument(filter.Type, nameof(filter.Type)).Equal("WhereNotIn");

            Guard.Argument(filter.Field, nameof(filter.Field)).NotEmpty().NotNull();
            Guard.Argument(filter.Values, nameof(filter.Values)).NotEmpty().NotNull();

            query.WhereNotIn(filter.Field, filter.Values);

            return query;
        }
    }
}