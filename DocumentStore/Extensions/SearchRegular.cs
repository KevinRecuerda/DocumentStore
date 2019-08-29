namespace DocumentStore.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Marten;
    using Marten.Linq;
    using Marten.Linq.Parsing;
    using Marten.Schema;

    public class SearchRegular : IMethodCallParser
    {
        public bool Matches(MethodCallExpression expression)
        {
            return expression.Method.Name == nameof(SearchRegularExtensions.SearchRegular);
        }

        public IWhereFragment Parse(IQueryableDocument mapping, ISerializer serializer, MethodCallExpression expression)
        {
            var members = FindMembers.Determine(expression);
            var locator = mapping.FieldFor(members).SqlLocator;

            var value   = expression.Arguments.Last().Value() as string;
            var pattern = SearchRegularExtensions.BuildPattern(value);

            return new WhereFragment($"{locator} ~* ?", pattern);
        }
    }

    public static class SearchRegularExtensions
    {
        public static async Task<IReadOnlyList<T>> SearchRegularAsync<T>(
            this IQuerySession          session,
            Expression<Func<T, string>> selector,
            string                      search,
            int?                        top = null)
        {
            var extendedSearch       = BuildPattern(search);
            var extendedStrictSearch = BuildStrictPattern(search);

            var members = FindMembers.Determine(selector);
            var doc     = session.DocumentStore.Tenancy.Default.MappingFor(typeof(T)).ToQueryableDocument();
            var locator = doc.FieldFor(members).SqlLocator;

            var sql = " d";
            sql += $" where {locator} ~* :{nameof(extendedSearch)}";
            sql += $" order by {locator} ~* :{nameof(search)} desc, {locator} ~* :{nameof(extendedStrictSearch)} desc";

            if (top.HasValue)
                sql += $"limit {top.Value}";

            return await session.QueryAsync<T>(
                       sql,
                       new CancellationToken(),
                       new
                       {
                           search,
                           extendedSearch,
                           extendedStrictSearch
                       });
        }

        public static bool SearchRegular(this string str, string search)
        {
            var pattern = BuildPattern(search);
            return new Regex(pattern, RegexOptions.IgnoreCase).IsMatch(str);
        }

        public static string BuildPattern(string search)
        {
            return string.Join(".*", search.ToCharArray());
        }

        public static string BuildStrictPattern(string search)
        {
            return string.Join("\\S*", search.ToCharArray());
        }
    }
}