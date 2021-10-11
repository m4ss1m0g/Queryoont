using System;
using SqlKata;

namespace Queryoont.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class QueryoontAttribute : Attribute
    {
        public QueryoontAttribute(Type query)
        {
            Query = query;
        }

        public Type Query { get; set; }
    }
}