using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QueryBuilder
{
	public static class QueryBuilder
	{
		public static IQueryable<TSearchable> Search<TSearchable, TSearchCriteria>(this IQueryable<TSearchable> searchableSet, TSearchCriteria searchCriteria)
		{
			var searchCriterion = CreateSearchCriteria<TSearchable, TSearchCriteria>(searchCriteria);
			return searchableSet.Where(searchable => searchCriterion.All(sc => sc(searchable)));
		}

		public static IEnumerable<TSearchable> Search<TSearchable, TSearchCriteria>(this IEnumerable<TSearchable> searchableSet, TSearchCriteria searchCriteria)
		{
			return searchableSet.Where(searchable =>
			{
				var searchCriterion = CreateSearchCriteria<TSearchable, TSearchCriteria>(searchCriteria);
				return searchCriterion.All(sc => sc(searchable));
			});
		}

		private static IEnumerable<Func<TSearchable, bool>> CreateSearchCriteria<TSearchable, TSearchCriteria>(TSearchCriteria searchCriteria)
		{
			var searchCriteriaProperties = searchCriteria.GetType().GetProperties();
			var typeOfSearchable = typeof (TSearchable);
			foreach (var searchCriteriaProperty in searchCriteriaProperties)
			{
				var searchablePropertyInfo = typeOfSearchable.GetProperty(searchCriteriaProperty.Name.Replace("Filter", ""));
				if (searchCriteriaProperty.PropertyType == typeof(StringSearchCriteria))
				{
					yield return
						CreateStringSearchCriteria<TSearchable>(searchablePropertyInfo,
							(StringSearchCriteria) searchCriteriaProperty.GetValue(searchCriteria, null));
				}

				if (searchCriteriaProperty.PropertyType == typeof(DateTimeSearchCriteria))
				{
					yield return
						CreateDateTimeSearchCriteria<TSearchable>(searchablePropertyInfo,
							(DateTimeSearchCriteria) searchCriteriaProperty.GetValue(searchCriteria, null));
				}
			}
		}

		private static Func<TSearchable, bool> CreateStringSearchCriteria<TSearchable>(PropertyInfo propertyInfo, StringSearchCriteria searchCriteria)
		{
			return searchable =>
			{
				if (searchCriteria == null) return true;

				if (propertyInfo.PropertyType != typeof(string)) throw new ArgumentException("Type of propertyInfo isn't a string.");

				var propertyValue = (string)propertyInfo.GetValue(searchable);
				switch (searchCriteria.SearchType)
				{
						case StringSearchType.Equals:
							return propertyValue.Equals(searchCriteria.Value);
						case StringSearchType.DoesNotEqual:
							return !propertyValue.Equals(searchCriteria.Value);
						case StringSearchType.StartsWith:
							return propertyValue.StartsWith(searchCriteria.Value);
						case StringSearchType.EndsWith:
							return propertyValue.Equals(searchCriteria.Value);
						case StringSearchType.Contains:
							return propertyValue.Contains(searchCriteria.Value);
						case StringSearchType.None:
							return true;
						default:	
							throw new ArgumentOutOfRangeException(string.Format("StringSearchType with value {0} hasn't been implemented.", searchCriteria.SearchType));
				}
			};
		}

		private static Func<TSearchable, bool> CreateDateTimeSearchCriteria<TSearchable>(PropertyInfo propertyInfo, DateTimeSearchCriteria searchCriteria)
		{
			return searchable =>
			{
				if (searchCriteria == null) return true;

				if (propertyInfo.PropertyType != typeof(DateTime)) throw new ArgumentException("Type of propertyInfo isn't a DateTime.");

				var propertyValue = (DateTime)propertyInfo.GetValue(searchable);
				switch (searchCriteria.SearchType)
				{
					case DateTimeSearchType.Before:
						return propertyValue < searchCriteria.Value;
					case DateTimeSearchType.BeforeOrEquals:
						return propertyValue <= searchCriteria.Value;
					case DateTimeSearchType.Equals:
						return propertyValue == searchCriteria.Value;
					case DateTimeSearchType.AfterOrEquals:
						return propertyValue >= searchCriteria.Value;
					case DateTimeSearchType.After:
						return propertyValue > searchCriteria.Value;
					case DateTimeSearchType.None:
						return true;
					default:
						throw new ArgumentOutOfRangeException(string.Format("DateTimeSearchType with value {0} hasn't been implemented.", searchCriteria.SearchType));
				}
			};
		}
	}
}
