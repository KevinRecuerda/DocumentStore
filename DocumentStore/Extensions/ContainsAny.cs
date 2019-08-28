namespace DocumentStore.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Marten;
    using Marten.Linq;
    using Marten.Linq.Parsing;
    using Marten.Schema;

    public class ContainsAny : IMethodCallParser
    {
        public bool Matches(MethodCallExpression expression)
        {
            return expression.Method.Name == nameof(CollectionExtensions.ContainsAny);
        }

        public IWhereFragment Parse(IQueryableDocument mapping, ISerializer serializer, MethodCallExpression expression)
        {
            var members = FindMembers.Determine(expression);

            var locator = mapping.FieldFor(members).SqlLocator;
            var values  = expression.Arguments.Last().Value();

            return new CollectionWhereFragment($"{locator} ?| ??", values);
        }
    }

    public static class CollectionExtensions
    {
        public static bool ContainsAny<T>(this IEnumerable<T> list, params T[] items)
        {
            return list.Intersect(items).Any();
        }
    }
}