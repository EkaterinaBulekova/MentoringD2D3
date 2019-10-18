using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Homework.LinqProvider
{
	class Query<T> : IQueryable<T>
	{
		private DataTableLinqProvider provider;

		public Query(Expression expression, DataTableLinqProvider provider)
		{
			this.Expression = expression;
			this.provider = provider;
		}

		public Type ElementType => typeof(T);

		public Expression Expression { get; }

		public IQueryProvider Provider => provider;

		public IEnumerator<T> GetEnumerator()
		{
			return provider.Execute<IEnumerable<T>>(Expression).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return provider.Execute<IEnumerable>(Expression).GetEnumerator();
		}
	}
}
