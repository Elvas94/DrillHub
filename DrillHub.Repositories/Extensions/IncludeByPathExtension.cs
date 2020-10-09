using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace DrillHub.Repositories.Extensions
{
    public static class IncludeByPathExtension
    {
        public static DbSet<TEntity> IncludeByPath<TEntity, TProperty>(
            this DbSet<TEntity> query,
            Expression<Func<TEntity, TProperty>> path)
            where TEntity : class
            where TProperty : class
        {
            if (query == null) throw new ArgumentException(nameof(query));

            var properties = new List<string>();
            void Add(string str) => properties.Insert(0, str);

            var expression = path.Body;
            do
            {
                switch (expression.NodeType)
                {
                    case ExpressionType.MemberAccess:
                        var member = (MemberExpression)expression;
                        if (member.Member.MemberType != MemberTypes.Property)
                            throw new ArgumentException(@"The selected member must be a property.", member.Member.Name);

                        Add(member.Member.Name);
                        expression = member.Expression;
                        break;
                    case ExpressionType.Call:
                        var method = (MethodCallExpression)expression;

                        if (method.Method.Name != nameof(Single) || method.Method.DeclaringType != typeof(Enumerable))
                            throw new ArgumentException(
                                $@"Method '{string.Join(Type.Delimiter.ToString(), method.Method.DeclaringType?.FullName, method.Method.Name)}' is not supported, only method '{string.Join(Type.Delimiter.ToString(), typeof(Enumerable).FullName, nameof(Single))}' is supported to singular navigation properties.",
                                method.Method.Name);

                        expression = (MemberExpression)method.Arguments.Single();
                        break;
                    default:
                        throw new ArgumentException(
                            @"The property selector expression has an incorrect format.",
                            expression.ToString());
                }

            } while (expression.NodeType != ExpressionType.Parameter);

            return query.Include(string.Join(Type.Delimiter.ToString(), properties)) as DbSet<TEntity>;
        }

        public static DbSet<TEntity> IncludeByPath<TEntity, TProperty>(
            this DbSet<TEntity> query,
            params Expression<Func<TEntity, TProperty>>[] includePaths)
            where TEntity : class
            where TProperty : class
        {
            if (includePaths != null)
            {
                query = includePaths.Aggregate(query,
                    (current, include) => current.IncludeByPath(include));
            }

            return query;
        }
    }
}
