namespace DocumentStore.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text.RegularExpressions;
    using Baseline;
    using Marten;
    using Marten.Linq;
    using Marten.Linq.MatchesSql;
    using Marten.Services.Includes;
    using Marten.Util;

    public static class MartenQueryableExtensions
    {
        public static IReadOnlyList<TOut> SelectFields<TIn, TOut>(
            this IQueryable<TIn> queryable,
            IDocumentSession     session,
            (string, string)[]   fields,
            params object[]      parameters)
        {
            var command = queryable.Explain().Command;

            // sql
            var jsonFields = string.Join(",", fields.Select(f => $"'{f.Item1}', {f.Item2}"));
            var baseSql    = command.CommandText.Substring(command.CommandText.IndexOf("from"));
            var sql        = $"select json_build_object({jsonFields}) as data {baseSql}";

            // parameters
            foreach (var parameter in parameters)
            {
                if (parameter.IsAnonymousType())
                    command.AddParameters(parameter);
                else
                {
                    var npgParameter = command.AddParameter(parameter);
                    sql.UseParameter(npgParameter);
                }
            }

            command.CommandText = sql;

            // read
            var reader = command.ExecuteReader();

            var serializer = (session.DocumentStore as DocumentStore).Serializer;

            var results = new List<TOut>();
            while (reader.Read())
            {
                var result = serializer.FromJson<TOut>(reader.GetTextReader(0));
                results.Add(result);
            }

            return results;
        }

        public static IQueryable<TSource> Where<TSource, TInclude>(
            this IQueryable<TSource>         source,
            Expression<Func<TInclude, bool>> predicate,
            IQuerySession                    session)
        {
            var command    = session.Query<TInclude>().Where(predicate).ToCommand();
            var parameters = command.Parameters.Select(p => p.Value).ToArray();

            var query       = command.CommandText;
            var whereClause = query.Substring(query.IndexOf("where ") + 5);

            var executor = source.As<MartenQueryable<TSource>>().Executor;
            var include  = executor.Includes.OfType<IncludeJoin<TInclude>>().First();

            whereClause = whereClause.Replace("d.", include.TableAlias + ".");
            whereClause = Regex.Replace(whereClause, @":arg(\d+)", "?");

            return source.Where(x => x.MatchesSql(whereClause, parameters));
        }
    }
}