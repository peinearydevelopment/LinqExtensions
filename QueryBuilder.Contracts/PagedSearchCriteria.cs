namespace QueryBuilder.Contracts
{
    public class PagedSearchCriteria
    {
        public PagedSearchCriteria()
        {
            PageIndex = 0;
            PageSize = 10;
        }

        /// <summary>
        /// A zero-based page index of the results to be returned.
        /// Default value: 0
        /// NOTE: this value is ignored if ReturnAllResults is set to true.
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// The maximum number of results to be returned with the search results.
        /// Default value: 10
        /// NOTE: this value is ignored if ReturnAllResults is set to true.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// If set to true, PageIndex and PageSize values are ignored.
        /// </summary>
        public bool ReturnAllResults { get; set; }

        /// <summary>
        /// If set to true AND PageIndex = 0, the SearchResult set will include a count of all the records matching the search criteria in the data store.
        /// </summary>
        public bool IncludeTotalCountWithResults { get; set; }
    }
}
