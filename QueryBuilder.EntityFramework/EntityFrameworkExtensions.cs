namespace QueryBuilder.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Contracts;

    public static class EntityFrameworkExtensions
    {
        public static SearchResult<TSearchable> Search<TSearchable, TEntitySearchCriteria>(this DbSet<TSearchable> dbSet, DbContext dbContext, TEntitySearchCriteria entitySearchCriteria) where TSearchable : class
                                                                                                                                                                                           where TEntitySearchCriteria : PagedSearchCriteria
        {
            var objectContext = ((IObjectContextAdapter)dbContext).ObjectContext;
            var objectSqlMetadata = dbContext.MapDbProperties<TSearchable>();
            var resultsDynamicQuery = GenerateResultsDynamicQuery(objectSqlMetadata, entitySearchCriteria);
            var totalResultsCountDynamicQuery = GenerateTotalResultsCountDynamicQuery(objectSqlMetadata, entitySearchCriteria);

            return new SearchResult<TSearchable>
            {
                TotalNumberOfResults = totalResultsCountDynamicQuery == null ? (int?)null : objectContext.ExecuteStoreQuery<int>(totalResultsCountDynamicQuery.ParameterizedQuery.ToString(), totalResultsCountDynamicQuery.Parameters).First(),
                Results = dbSet.SqlQuery(resultsDynamicQuery.ParameterizedQuery.ToString(), resultsDynamicQuery.Parameters).ToList()
            };
        }

        public static async Task<SearchResult<TSearchable>> SearchAsync<TSearchable, TEntitySearchCriteria>(this DbSet<TSearchable> dbSet, DbContext dbContext, TEntitySearchCriteria entitySearchCriteria) where TSearchable : class
                                                                                                                                                                                                            where TEntitySearchCriteria : PagedSearchCriteria
        {
            var objectContext = ((IObjectContextAdapter)dbContext).ObjectContext;
            var objectSqlMetadata = dbContext.MapDbProperties<TSearchable>();
            var resultsDynamicQuery = GenerateResultsDynamicQuery(objectSqlMetadata, entitySearchCriteria);
            var totalResultsCountDynamicQuery = GenerateTotalResultsCountDynamicQuery(objectSqlMetadata, entitySearchCriteria);

            return new SearchResult<TSearchable>
            {
                TotalNumberOfResults = totalResultsCountDynamicQuery == null ? (int?)null : (await objectContext.ExecuteStoreQueryAsync<int>(totalResultsCountDynamicQuery.ParameterizedQuery.ToString(), totalResultsCountDynamicQuery.Parameters)).First(),
                Results = await dbSet.SqlQuery(resultsDynamicQuery.ParameterizedQuery.ToString(), resultsDynamicQuery.Parameters).ToListAsync()
            };
        }

        private static DynamicQuery GenerateResultsDynamicQuery<TEntitySearchCriteria>(ObjectSqlMetadata objectSqlMetadata, TEntitySearchCriteria entitySearchCriteria) where TEntitySearchCriteria : PagedSearchCriteria
        {
            var dynamicQuery = new StringBuilder().CreateSelect(objectSqlMetadata.ObjectPropertyToColumnNameMapper)
                                                  .CreateFrom(objectSqlMetadata.SchemaName, objectSqlMetadata.TableName)
                                                  .CreateWhere(entitySearchCriteria, objectSqlMetadata.ObjectPropertyToColumnNameMapper);

            if (!entitySearchCriteria.ReturnAllResults)
            {
                dynamicQuery = dynamicQuery.CreateOffset(entitySearchCriteria, objectSqlMetadata.PrimaryKeyName);
            }

            return dynamicQuery;
        }

        private static DynamicQuery GenerateTotalResultsCountDynamicQuery<TEntitySearchCriteria>(ObjectSqlMetadata objectSqlMetadata, TEntitySearchCriteria entitySearchCriteria) where TEntitySearchCriteria : PagedSearchCriteria
        {
            if (entitySearchCriteria.ReturnAllResults || (entitySearchCriteria.IncludeTotalCountWithResults && entitySearchCriteria.PageIndex == 0))
            {
                return new StringBuilder().SafeSqlAppend("SELECT COUNT(1)")
                                          .CreateFrom(objectSqlMetadata.SchemaName, objectSqlMetadata.TableName)
                                          .CreateWhere(entitySearchCriteria, objectSqlMetadata.ObjectPropertyToColumnNameMapper);
            }

            return null;
        }

        private static DynamicQuery CreateOffset<TEntitySearchCriteria>(this DynamicQuery dynamicQuery, TEntitySearchCriteria entitySearchCriteria, string primaryKeyName) where TEntitySearchCriteria : PagedSearchCriteria
        {
            dynamicQuery.ParameterizedQuery
                        .SafeSqlAppend($"ORDER BY {primaryKeyName}")
                        .SafeSqlAppend($"OFFSET {entitySearchCriteria.PageIndex * entitySearchCriteria.PageSize} ROWS")
                        .SafeSqlAppend($"FETCH NEXT {entitySearchCriteria.PageSize} ROWS ONLY");

            return dynamicQuery;
        }

        private static StringBuilder CreateSelect(this StringBuilder stringBuilder, IDictionary<string, string> objectPropertyToColumnNameMapper)
        {
            var sqlProperties = string.Join(", ", objectPropertyToColumnNameMapper.Select(propertyToColumnNameMap => $"[{propertyToColumnNameMap.Value}] AS [{propertyToColumnNameMap.Key}]"));
            return stringBuilder.SafeSqlAppend($"SELECT {sqlProperties}");
        }

        private static StringBuilder CreateFrom(this StringBuilder stringBuilder, string schema, string tableName)
        {
            return stringBuilder.SafeSqlAppend($"FROM [{schema}].[{tableName}]");
        }

        private static StringBuilder SafeSqlAppend(this StringBuilder stringBuilder, string stringToAppend)
        {
            return stringBuilder.Append(stringBuilder.Length > 0 ? $" {stringToAppend}" : stringToAppend);
        }

        private static StringBuilder SafeSqlAppend(this StringBuilder stringBuilder, StringBuilder stringBuilderToAppend)
        {
            var stringToAppend = stringBuilderToAppend.ToString();
            return stringBuilder.Append(stringBuilder.Length > 0 ? $" {stringToAppend}" : stringToAppend);
        }

        private static DynamicQuery CreateWhere<TEntitySearchCriteria>(this StringBuilder stringBuilder, TEntitySearchCriteria entitySearchCriteria, IDictionary<string, string> objectPropertyToColumnNameMapper) where TEntitySearchCriteria : PagedSearchCriteria
        {
            var tEntitySearchCriteriaProperties = typeof(TEntitySearchCriteria).GetProperties();

            var parameters = new List<object>();
            var parameterIndex = 0;
            var tmpStringBuilder = new StringBuilder();
            var sortCriteria = new List<SortCriteria>();

            foreach (var propertyToColumnNameMap in objectPropertyToColumnNameMapper)
            {
                var prop = tEntitySearchCriteriaProperties.FirstOrDefault(p => p.Name.Equals(propertyToColumnNameMap.Key, StringComparison.OrdinalIgnoreCase));

                var propertySearchCriteria = prop?.GetValue(entitySearchCriteria);

                if (propertySearchCriteria is SortCriteriaBase)
                {
                    string whereClause = null;
                    object value = null;

                    if (propertySearchCriteria is StringSearchCriteria)
                    {
                        var stringSearchCriteria = propertySearchCriteria as StringSearchCriteria;
                        whereClause = CreateWhereClause(propertyToColumnNameMap.Value, parameterIndex, stringSearchCriteria.SearchType);
                        value = stringSearchCriteria.Value;
                    }

                    if (propertySearchCriteria is IntegerSearchCriteria)
                    {
                        var integerSearchCriteria = propertySearchCriteria as IntegerSearchCriteria;
                        whereClause = CreateWhereClause(propertyToColumnNameMap.Value, parameterIndex, integerSearchCriteria.SearchType);
                        value = integerSearchCriteria.Value;
                    }

                    if (propertySearchCriteria is DecimalSearchCriteria)
                    {
                        var decimalSearchCriteria = propertySearchCriteria as DecimalSearchCriteria;
                        whereClause = CreateWhereClause(propertyToColumnNameMap.Value, parameterIndex, decimalSearchCriteria.SearchType);
                        value = decimalSearchCriteria.Value;
                    }

                    if (propertySearchCriteria is DateTimeSearchCriteria)
                    {
                        var dateTimeSearchCriteria = propertySearchCriteria as DateTimeSearchCriteria;
                        whereClause = CreateWhereClause(propertyToColumnNameMap.Value, parameterIndex, dateTimeSearchCriteria.SearchType);
                        value = dateTimeSearchCriteria.Value;
                    }

                    if (propertySearchCriteria is DateTimeOffsetSearchCriteria)
                    {
                        var dateTimeOffsetSearchCriteria = propertySearchCriteria as DateTimeOffsetSearchCriteria;
                        whereClause = CreateWhereClause(propertyToColumnNameMap.Value, parameterIndex, dateTimeOffsetSearchCriteria.SearchType);
                        value = dateTimeOffsetSearchCriteria.Value;
                    }

                    if (propertySearchCriteria is BooleanSearchCriteria)
                    {
                        var booleanSearchCriteria = propertySearchCriteria as BooleanSearchCriteria;
                        whereClause = CreateWhereClause(propertyToColumnNameMap.Value, parameterIndex, booleanSearchCriteria.SearchType);
                        value = booleanSearchCriteria.Value;
                    }

                    if (!string.IsNullOrWhiteSpace(whereClause))
                    {
                        if (tmpStringBuilder.Length > 0)
                        {
                            tmpStringBuilder.SafeSqlAppend("AND");
                        }

                        tmpStringBuilder.SafeSqlAppend(whereClause);

                        parameters.Add(new SqlParameter($"p{parameterIndex}", value));
                    }

                    sortCriteria.Add(new SortCriteria { SortColumnName = propertyToColumnNameMap.Value, SortCriteriaBase = propertySearchCriteria as SortCriteriaBase });
                    parameterIndex++;
                }
            }

            if (tmpStringBuilder.Length > 0)
            {
                stringBuilder.SafeSqlAppend("WHERE").SafeSqlAppend(tmpStringBuilder);
            }

            var filteredSortCriteria = sortCriteria.Where(criteria => criteria.SortCriteriaBase.SortType != SortType.None);
            if (filteredSortCriteria.Any())
            {
                var orderedSortCriteria = filteredSortCriteria.OrderBy(criteria => criteria.SortCriteriaBase.SortOrder);
                stringBuilder.SafeSqlAppend("ORDER BY");
                foreach (var sortCriterium in orderedSortCriteria)
                {
                    var sortDirectionString = sortCriterium.SortCriteriaBase.SortType == SortType.Ascending ? "ASC" : "DESC";

                    stringBuilder.SafeSqlAppend($"{sortCriterium.SortColumnName} {sortDirectionString}");

                    if (orderedSortCriteria.Count() > 1 && orderedSortCriteria.Last() != sortCriterium)
                    {
                        stringBuilder.Append(",");
                    }
                }
            }

            return new DynamicQuery
            {
                ParameterizedQuery = stringBuilder,
                Parameters = parameters.ToArray()
            };
        }

        private static string CreateWhereClause(string propertyName, int propertyIndex, StringSearchType searchType)
        {
            switch (searchType)
            {
                case StringSearchType.None:
                    return string.Empty;
                case StringSearchType.Equals:
                    return $"[{propertyName}] = @p{propertyIndex}";
                case StringSearchType.DoesNotEqual:
                    return $"[{propertyName}] != @p{propertyIndex}";
                case StringSearchType.StartsWith:
                    return $"[{propertyName}] LIKE @p{propertyIndex} + '%'";
                case StringSearchType.EndsWith:
                    return $"[{propertyName}] LIKE '%' + @p{propertyIndex}";
                case StringSearchType.Contains:
                    return $"[{propertyName}] LIKE '%' + @p{propertyIndex} + '%'";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static string CreateWhereClause(string propertyName, int propertyIndex, IntegerSearchType searchType)
        {
            switch (searchType)
            {
                case IntegerSearchType.None:
                    return string.Empty;
                case IntegerSearchType.LessThan:
                    return $"[{propertyName}] < @p{propertyIndex}";
                case IntegerSearchType.LessThanOrEquals:
                    return $"[{propertyName}] <= @p{propertyIndex}";
                case IntegerSearchType.Equals:
                    return $"[{propertyName}] = @p{propertyIndex}";
                case IntegerSearchType.GreaterThanOrEquals:
                    return $"[{propertyName}] >= @p{propertyIndex}";
                case IntegerSearchType.GreaterThan:
                    return $"[{propertyName}] > @p{propertyIndex}";
                case IntegerSearchType.DoesNotEqual:
                    return $"[{propertyName}] <> @p{propertyIndex}";
                default:
                    throw new ArgumentOutOfRangeException(nameof(searchType), searchType, null);
            }
        }

        private static string CreateWhereClause(string propertyName, int propertyIndex, DecimalSearchType searchType)
        {
            switch (searchType)
            {
                case DecimalSearchType.None:
                    return string.Empty;
                case DecimalSearchType.LessThan:
                    return $"[{propertyName}] < @p{propertyIndex}";
                case DecimalSearchType.LessThanOrEquals:
                    return $"[{propertyName}] <= @p{propertyIndex}";
                case DecimalSearchType.Equals:
                    return $"[{propertyName}] = @p{propertyIndex}";
                case DecimalSearchType.GreaterThanOrEquals:
                    return $"[{propertyName}] >= @p{propertyIndex}";
                case DecimalSearchType.GreaterThan:
                    return $"[{propertyName}] > @p{propertyIndex}";
                case DecimalSearchType.DoesNotEqual:
                    return $"[{propertyName}] <> @p{propertyIndex}";
                default:
                    throw new ArgumentOutOfRangeException(nameof(searchType), searchType, null);
            }
        }

        private static string CreateWhereClause(string propertyName, int propertyIndex, DateTimeSearchType searchType)
        {
            switch (searchType)
            {
                case DateTimeSearchType.None:
                    return string.Empty;
                case DateTimeSearchType.Before:
                    return $"[{propertyName}] < @p{propertyIndex}";
                case DateTimeSearchType.BeforeOrEquals:
                    return $"[{propertyName}] <= @p{propertyIndex}";
                case DateTimeSearchType.Equals:
                    return $"[{propertyName}] = @p{propertyIndex}";
                case DateTimeSearchType.AfterOrEquals:
                    return $"[{propertyName}] >= @p{propertyIndex}";
                case DateTimeSearchType.After:
                    return $"[{propertyName}] > @p{propertyIndex}";
                default:
                    throw new ArgumentOutOfRangeException(nameof(searchType), searchType, null);
            }
        }

        private static string CreateWhereClause(string propertyName, int propertyIndex, DateTimeOffsetSearchType searchType)
        {
            switch (searchType)
            {
                case DateTimeOffsetSearchType.None:
                    return string.Empty;
                case DateTimeOffsetSearchType.Before:
                    return $"[{propertyName}] < @p{propertyIndex}";
                case DateTimeOffsetSearchType.BeforeOrEquals:
                    return $"[{propertyName}] <= @p{propertyIndex}";
                case DateTimeOffsetSearchType.Equals:
                    return $"[{propertyName}] = @p{propertyIndex}";
                case DateTimeOffsetSearchType.AfterOrEquals:
                    return $"[{propertyName}] >= @p{propertyIndex}";
                case DateTimeOffsetSearchType.After:
                    return $"[{propertyName}] > @p{propertyIndex}";
                default:
                    throw new ArgumentOutOfRangeException(nameof(searchType), searchType, null);
            }
        }

        private static string CreateWhereClause(string propertyName, int propertyIndex, BooleanSearchType searchType)
        {
            switch (searchType)
            {
                case BooleanSearchType.None:
                    return string.Empty;
                case BooleanSearchType.Equals:
                    return $"[{propertyName}] = @p{propertyIndex}";
                case BooleanSearchType.DoesNotEqual:
                    return $"[{propertyName}] <> @p{propertyIndex}";
                default:
                    throw new ArgumentOutOfRangeException(nameof(searchType), searchType, null);
            }
        }

        //TODO: update to use regex
        private static ObjectSqlMetadata MapDbProperties<TSearchable>(this IObjectContextAdapter dbContext) where TSearchable : class
        {
            var objectContext = dbContext.ObjectContext;
            var objectSet = objectContext.CreateObjectSet<TSearchable>();
            var traceString = objectSet.ToTraceString();

            return new ObjectSqlMetadata().MapPrimaryKeyName(objectSet)
                                          .MapObjectPropertiesToColumnNames(traceString)
                                          .MapObjectToTableName(traceString)
                                          .MapSchema(traceString);
        }

        private static ObjectSqlMetadata MapPrimaryKeyName<TSearchable>(this ObjectSqlMetadata objectSqlMetadata, ObjectSet<TSearchable> objectSet) where TSearchable : class
        {
            objectSqlMetadata.PrimaryKeyName = objectSet.EntitySet.ElementType.KeyMembers.Select(k => k.Name).First();
            return objectSqlMetadata;
        }

        private static ObjectSqlMetadata MapObjectPropertiesToColumnNames(this ObjectSqlMetadata objectSqlMetadata, string sqlClause)
        {
            var fromClauseStartIndex = sqlClause.IndexOf("FROM", StringComparison.OrdinalIgnoreCase);
            var selectClause = sqlClause.Substring(0, fromClauseStartIndex).Trim();
            var selectClauseSegments = selectClause.Split(',');
            foreach (var selectClauseSegment in selectClauseSegments)
            {
                var columnNameStartIndex = selectClauseSegment.IndexOf(".[") + ".[".Length;
                var columnNameEndIndex = selectClauseSegment.IndexOf(']', columnNameStartIndex);
                var columnName = selectClauseSegment.Substring(columnNameStartIndex, columnNameEndIndex - columnNameStartIndex);
                var objectProperyNameStartIndex = selectClauseSegment.IndexOf('[', columnNameEndIndex) + "[".Length;
                var objectProperyNameEndIndex = selectClauseSegment.IndexOf(']', objectProperyNameStartIndex);
                var objectPropertyName = selectClauseSegment.Substring(objectProperyNameStartIndex, objectProperyNameEndIndex - objectProperyNameStartIndex);
                objectSqlMetadata.ObjectPropertyToColumnNameMapper.Add(objectPropertyName, columnName);
            }

            return objectSqlMetadata;
        }

        private static ObjectSqlMetadata MapObjectToTableName(this ObjectSqlMetadata objectSqlMetadata, string sqlClause)
        {
            var fromClauseStartIndex = sqlClause.IndexOf("FROM", StringComparison.OrdinalIgnoreCase);
            var fromClause = sqlClause.Substring(fromClauseStartIndex).Trim();
            var tableNameStartIndex = fromClause.IndexOf(".[") + ".[".Length;
            var tableNameEndIndex = fromClause.IndexOf(']', tableNameStartIndex);
            objectSqlMetadata.TableName = fromClause.Substring(tableNameStartIndex, tableNameEndIndex - tableNameStartIndex);
            return objectSqlMetadata;
        }

        //TODO: parse schema also
        private static ObjectSqlMetadata MapSchema(this ObjectSqlMetadata objectSqlMetadata, string sqlClause)
        {
            objectSqlMetadata.SchemaName = "dbo";
            return objectSqlMetadata;
        }
    }
}