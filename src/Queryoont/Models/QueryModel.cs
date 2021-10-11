namespace Queryoont.Models
{
    public class QueryModel
    {
        public string Version { get; set; }

        public string[] Select { get; set; }

        public QueryFilter[] Filter { get; set; }

        // public string[] Include { get; set; }
    }
}