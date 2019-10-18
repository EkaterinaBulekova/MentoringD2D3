using System;
using System.Linq;
using System.Linq.Expressions;
using Homework.Data;

namespace Homework.LinqProvider
{
	public class DataTableLinqProvider : IQueryProvider
	{
		public IQueryable CreateQuery(Expression expression)
		{
			throw new NotImplementedException();
		}

		public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
		{
			return new Query<TElement>(expression, this);
		}

		public object Execute(Expression expression)
		{
			throw new NotImplementedException();
		}

		public TResult Execute<TResult>(Expression expression)
		{
			var itemType = TypeHelper.GetElementType(expression.Type);

			var translator = new DataTableExpressionsTranslator();
			var queryString = translator.Translate(expression);

			return (TResult)DataSource.GetEntities(itemType, queryString);
		}
	}
}
