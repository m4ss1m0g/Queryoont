namespace Queryoont.Models
{
    public class QueryFilter
    {
        public string Type { get; set; }

        public QueryFilter[] Condition { get; set; }

        public string Field { get; set; }

        public string Oper { get; set; }

        public object Value { get; set; }

        public object[] Values { get; set; }
    }
}