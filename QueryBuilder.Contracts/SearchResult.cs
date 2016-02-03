namespace QueryBuilder.Contracts
{
    using System.Collections.Generic;

    public class SearchResult<TSearchable> where TSearchable : class
    {
        public int? TotalNumberOfResults { get; set; }
        public IList<TSearchable> Results { get; set; }
    }
}
