using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Homework.LinqProvider
{
	public class DataTableExpressionsTranslator : ExpressionVisitor
	{
		StringBuilder resultString;

		public string Translate(Expression exp)
		{
			resultString = new StringBuilder();
			Visit(exp);
			return resultString.ToString();
		}

		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			if (IsWhere(node))
			{
                VisitWhere(node);
				return node;
			}

            if (IsContains(node)) 
            {
                VisitContains(node);
                return node;
            }

            if (IsStartsWith(node))
            {
                VisitStartsWith(node);
                return node;
            }

            if (IsEndsWith(node))
            {
                VisitEndsWith(node);
                return node;
            }

            return base.VisitMethodCall(node);
		}

		protected override Expression VisitBinary(BinaryExpression node)
		{
			switch (node.NodeType)
			{
				case ExpressionType.Equal:
                    VisitEqual(node);                
                    break;

                case ExpressionType.AndAlso:
                    VisitAndAlso(node);
                    break;

                default:
					throw new NotSupportedException(string.Format("Operation {0} is not supported", node.NodeType));
			};

			return node;
		}

        private bool IsWhere(MethodCallExpression node)
        {
            return node.Method.DeclaringType == typeof(Queryable)
                && node.Method.Name == "Where";
        }

        private bool IsContains(MethodCallExpression node)
        {
            return node.Method.DeclaringType == typeof(string) &&
                    node.Method.Name == "Contains";
        }

        private bool IsStartsWith(MethodCallExpression node)
        {
            return node.Method.DeclaringType == typeof(string) &&
                    node.Method.Name == "StartsWith";
        }

        private bool IsEndsWith(MethodCallExpression node)
        {
            return node.Method.DeclaringType == typeof(string) &&
                    node.Method.Name == "EndsWith";
        }

        private bool IsEqualLeft(BinaryExpression node)
        {
            return node.Left.NodeType == ExpressionType.Constant 
                && node.Right.NodeType == ExpressionType.MemberAccess;
        }

        private bool IsEqualRight(BinaryExpression node)
        {
            return node.Right.NodeType == ExpressionType.Constant 
                && node.Left.NodeType == ExpressionType.MemberAccess;
        }

        private void VisitWhere(MethodCallExpression node)
        {
            var predicate = node.Arguments[1];
            Visit(predicate);
        }

        private void VisitContains(MethodCallExpression node)
        {
            VisitWithLike(node, " LIKE '%{0}%'");
        }

        private void VisitStartsWith(MethodCallExpression node)
        {
            VisitWithLike(node, " LIKE '{0}%'");
        }

        private void VisitEndsWith(MethodCallExpression node)
        {
            VisitWithLike(node, " LIKE '%{0}'");
        }

        private void VisitWithLike(MethodCallExpression node, string template)
        {
            var condition = node.Arguments[0].ToString().Replace("\"", "");
            Visit(node.Object);
            resultString.Append(string.Format(template, condition));
        }

        private void VisitEqualLeft(BinaryExpression node)
        {
            Visit(node.Right);
            resultString.Append(" = '");
            Visit(node.Left);
            resultString.Append("'");
        }

        private void VisitEqualRight(BinaryExpression node)
        {
            Visit(node.Left);
            resultString.Append(" = '");
            Visit(node.Right);
            resultString.Append("'");
        }

        private void VisitAndAlso(BinaryExpression node)
        {
            Visit(node.Left);
            resultString.Append(" AND ");
            Visit(node.Right);
        }

        protected override Expression VisitMember(MemberExpression node)
		{
			resultString.Append(node.Member.Name);
			return base.VisitMember(node);
		}

		protected override Expression VisitConstant(ConstantExpression node)
		{
			resultString.Append(node.Value);
			return node;
		}

        private void VisitEqual(BinaryExpression node)
        {
            if (IsEqualRight(node))
                VisitEqualRight(node);
            else
                if (IsEqualLeft(node))
                    VisitEqualLeft(node);
                else
                    throw new NotSupportedException(
                        $"Operand types ({node.Left.NodeType} = {node.Right.NodeType}) are not supported");
        }	
    }
}
