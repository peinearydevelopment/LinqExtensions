using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Framework.Searching.EntityFramework
{
    using QueryBuilder.Contracts;

    public static class EntityFrameworkExtensions
    {
        public static DbSqlQuery<TSearchable> Search<TSearchable, TEntitySearchCriteria>(this DbSet<TSearchable> dbSet, TEntitySearchCriteria entitySearchCriteria) where TSearchable : class
                                                                                                                                                 where TEntitySearchCriteria : ObjectSearchCriteria<TSearchable>
        {
            var stringBuilder = new StringBuilder()
                                    .CreateSelect<TSearchable>()
                                    .CreateFrom<TSearchable>()
                                    .CreateWhere<TSearchable, TEntitySearchCriteria>(entitySearchCriteria);

            var a = stringBuilder.Item1.ToString();
            return dbSet.SqlQuery(stringBuilder.Item1.ToString(), stringBuilder.Item2);
        }

        public static StringBuilder CreateSelect<TSearchable>(this StringBuilder stringBuilder) where TSearchable : class
        {
            var tSearchable = typeof (TSearchable);
            var tSearchableProperties = tSearchable.GetProperties();
            var sqlProperties = string.Join(", ", tSearchableProperties.Select(property => property.Name));
            stringBuilder.Append($"SELECT {sqlProperties} ");
            return stringBuilder;
        }

        public static StringBuilder CreateFrom<TSearchable>(this StringBuilder stringBuilder) where TSearchable : class
        {
            var tSearchable = typeof(TSearchable);
            stringBuilder.Append($"FROM {tSearchable.Name}s ");
            return stringBuilder;
        }

        public static Tuple<StringBuilder, object[]> CreateWhere<TSearchable, TEntitySearchCriteria>(this StringBuilder stringBuilder, TEntitySearchCriteria entitySearchCriteria) where TSearchable : class
                                                                                                                                                                                   where TEntitySearchCriteria : ObjectSearchCriteria<TSearchable>
        {
            var tEntitySearchCriteria = typeof(TEntitySearchCriteria);
            var tEntitySearchCriteriaProperties = tEntitySearchCriteria.GetProperties().Where(prop => prop.GetValue(entitySearchCriteria) != null);

            object[] parameters = new object[0];
            if (tEntitySearchCriteriaProperties.Any())
            {
                var tmpStringBuilder = new StringBuilder();
                parameters = new object[tEntitySearchCriteriaProperties.Count()];
                var parameterIndex = 0;
                foreach (var tEntitySearchCriteriaProperty in tEntitySearchCriteriaProperties)
                {
                    if (tEntitySearchCriteriaProperty.PropertyType == typeof (StringSearchCriteria))
                    {
                        var stringPropertySearchCriteria = tEntitySearchCriteriaProperty.GetValue(entitySearchCriteria) as StringSearchCriteria;
                        tmpStringBuilder.Append(CreateWhereClause(tEntitySearchCriteriaProperty.Name, parameterIndex, stringPropertySearchCriteria.SearchType));
                        parameters[parameterIndex] = new SqlParameter($"p{parameterIndex}", stringPropertySearchCriteria.Value);
                        parameterIndex++;
                    }

                    if (tEntitySearchCriteriaProperty.PropertyType == typeof(IntegerSearchCriteria))
                    {
                        var integerPropertySearchCriteria = tEntitySearchCriteriaProperty.GetValue(entitySearchCriteria) as IntegerSearchCriteria;
                        tmpStringBuilder.Append(CreateWhereClause(tEntitySearchCriteriaProperty.Name, parameterIndex, integerPropertySearchCriteria.SearchType));
                        parameters[parameterIndex] = new SqlParameter($"p{parameterIndex}", integerPropertySearchCriteria.Value);
                        parameterIndex++;
                    }

                    if (tEntitySearchCriteriaProperty.PropertyType == typeof(DecimalSearchCriteria))
                    {
                        var decimalPropertySearchCriteria = tEntitySearchCriteriaProperty.GetValue(entitySearchCriteria) as DecimalSearchCriteria;
                        tmpStringBuilder.Append(CreateWhereClause(tEntitySearchCriteriaProperty.Name, parameterIndex, decimalPropertySearchCriteria.SearchType));
                        parameters[parameterIndex] = new SqlParameter($"p{parameterIndex}", decimalPropertySearchCriteria.Value);
                        parameterIndex++;
                    }

                    if (tEntitySearchCriteriaProperty.PropertyType == typeof(DateTimeSearchCriteria))
                    {
                        var dateTimePropertySearchCriteria = tEntitySearchCriteriaProperty.GetValue(entitySearchCriteria) as DateTimeSearchCriteria;
                        tmpStringBuilder.Append(CreateWhereClause(tEntitySearchCriteriaProperty.Name, parameterIndex, dateTimePropertySearchCriteria.SearchType));
                        parameters[parameterIndex] = new SqlParameter($"p{parameterIndex}", dateTimePropertySearchCriteria.Value);
                        parameterIndex++;
                    }

                    if (tEntitySearchCriteriaProperty.PropertyType == typeof(DateTimeOffsetSearchCriteria))
                    {
                        var dateTimeOffsetPropertySearchCriteria = tEntitySearchCriteriaProperty.GetValue(entitySearchCriteria) as DateTimeOffsetSearchCriteria;
                        tmpStringBuilder.Append(CreateWhereClause(tEntitySearchCriteriaProperty.Name, parameterIndex, dateTimeOffsetPropertySearchCriteria.SearchType));
                        parameters[parameterIndex] = new SqlParameter($"p{parameterIndex}", dateTimeOffsetPropertySearchCriteria.Value);
                        parameterIndex++;
                    }
                }

                if (tmpStringBuilder.Length > 0)
                {
                    stringBuilder.Append("WHERE ").Append(tmpStringBuilder);
                }
            }

            return new Tuple<StringBuilder, object[]>(stringBuilder, parameters);
        }

        public static string CreateWhereClause(string propertyName, int propertyIndex, StringSearchType searchType)
        {
            switch (searchType)
            {
                case StringSearchType.None:
                    return string.Empty;
                case StringSearchType.Equals:
                    return $"{propertyName} = @p{propertyIndex}";
                case StringSearchType.DoesNotEqual:
                    return $"{propertyName} != @p{propertyIndex}";
                case StringSearchType.StartsWith:
                    return $"{propertyName} LIKE @p{propertyIndex} + '%'";
                case StringSearchType.EndsWith:
                    return $"{propertyName} LIKE '%' + @p{propertyIndex}";
                case StringSearchType.Contains:
                    return $"{propertyName} LIKE '%' + @p{propertyIndex} + '%'";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static string CreateWhereClause(string propertyName, int propertyIndex, IntegerSearchType searchType)
        {
            switch (searchType)
            {
                case IntegerSearchType.None:
                    return string.Empty;
                case IntegerSearchType.LessThan:
                    return $"{propertyName} < @p{propertyIndex}";
                case IntegerSearchType.LessThanOrEquals:
                    return $"{propertyName} <= @p{propertyIndex}";
                case IntegerSearchType.Equals:
                    return $"{propertyName} = @p{propertyIndex}";
                case IntegerSearchType.GreaterThanOrEquals:
                    return $"{propertyName} >= @p{propertyIndex}";
                case IntegerSearchType.GreaterThan:
                    return $"{propertyName} > @p{propertyIndex}";
                case IntegerSearchType.DoesNotEqual:
                    return $"{propertyName} <> @p{propertyIndex}";
                default:
                    throw new ArgumentOutOfRangeException(nameof(searchType), searchType, null);
            }
        }

        public static string CreateWhereClause(string propertyName, int propertyIndex, DecimalSearchType searchType)
        {
            switch (searchType)
            {
                case DecimalSearchType.None:
                    return string.Empty;
                case DecimalSearchType.LessThan:
                    return $"{propertyName} < @p{propertyIndex}";
                case DecimalSearchType.LessThanOrEquals:
                    return $"{propertyName} <= @p{propertyIndex}";
                case DecimalSearchType.Equals:
                    return $"{propertyName} = @p{propertyIndex}";
                case DecimalSearchType.GreaterThanOrEquals:
                    return $"{propertyName} >= @p{propertyIndex}";
                case DecimalSearchType.GreaterThan:
                    return $"{propertyName} > @p{propertyIndex}";
                case DecimalSearchType.DoesNotEqual:
                    return $"{propertyName} <> @p{propertyIndex}";
                default:
                    throw new ArgumentOutOfRangeException(nameof(searchType), searchType, null);
            }
        }

        public static string CreateWhereClause(string propertyName, int propertyIndex, DateTimeSearchType searchType)
        {
            switch (searchType)
            {
                case DateTimeSearchType.None:
                    return string.Empty;
                case DateTimeSearchType.Before:
                    return $"{propertyName} < @p{propertyIndex}";
                case DateTimeSearchType.BeforeOrEquals:
                    return $"{propertyName} <= @p{propertyIndex}";
                case DateTimeSearchType.Equals:
                    return $"{propertyName} = @p{propertyIndex}";
                case DateTimeSearchType.AfterOrEquals:
                    return $"{propertyName} >= @p{propertyIndex}";
                case DateTimeSearchType.After:
                    return $"{propertyName} > @p{propertyIndex}";
                default:
                    throw new ArgumentOutOfRangeException(nameof(searchType), searchType, null);
            }
        }

        public static string CreateWhereClause(string propertyName, int propertyIndex, DateTimeOffsetSearchType searchType)
        {
            switch (searchType)
            {
                case DateTimeOffsetSearchType.None:
                    return string.Empty;
                case DateTimeOffsetSearchType.Before:
                    return $"{propertyName} < @p{propertyIndex}";
                case DateTimeOffsetSearchType.BeforeOrEquals:
                    return $"{propertyName} <= @p{propertyIndex}";
                case DateTimeOffsetSearchType.Equals:
                    return $"{propertyName} = @p{propertyIndex}";
                case DateTimeOffsetSearchType.AfterOrEquals:
                    return $"{propertyName} >= @p{propertyIndex}";
                case DateTimeOffsetSearchType.After:
                    return $"{propertyName} > @p{propertyIndex}";
                default:
                    throw new ArgumentOutOfRangeException(nameof(searchType), searchType, null);
            }
        }
    }
}