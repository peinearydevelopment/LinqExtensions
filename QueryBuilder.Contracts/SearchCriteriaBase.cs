namespace QueryBuilder.Contracts
{
	public class SearchCriteriaBase<T, TSearchType> : SortCriteriaBase
    {
		public TSearchType SearchType { get; set; }
		public T Value { get; set; }
	}
}