using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Homework.Data.Entities;

namespace Homework.LinqProvider
{
	public class EntitySet<T> : IQueryable<T> where T : Entity
	{
		protected Expression expression;

		protected IQueryProvider provider;

		public EntitySet()
		{
			expression = Expression.Constant(this);
			provider = new DataTableLinqProvider();
		}

		public Type ElementType => typeof(T);

		public Expression Expression => expression;

		public IQueryProvider Provider => provider;

		public IEnumerator<T> GetEnumerator()
		{
			return provider.Execute<IEnumerable<T>>(expression).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return provider.Execute<IEnumerable>(expression).GetEnumerator();
		}
	}
}
