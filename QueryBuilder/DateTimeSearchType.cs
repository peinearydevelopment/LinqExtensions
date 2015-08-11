namespace QueryBuilder
{
	// Currently all comparisons are done without any DateTime manipulation. 
	// TODO: add support for DateTimeKind matches?
	public enum DateTimeSearchType
	{
		None = 0,
		Before = 1,
		BeforeOrEquals = 2,
		Equals = 3,
		AfterOrEquals = 4,
		After = 5
	}
}
