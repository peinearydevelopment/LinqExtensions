namespace QueryBuilder.Contracts
{
	// Currently all comparisons are done as case-sensitive comparisons. 
	// TODO: add support for case-insensitive matches?
	public enum StringSearchType
	{
		None = 0,
		Equals = 1,
		DoesNotEqual = 2,
		StartsWith = 3,
		EndsWith = 4,
		Contains = 5
	}
}
