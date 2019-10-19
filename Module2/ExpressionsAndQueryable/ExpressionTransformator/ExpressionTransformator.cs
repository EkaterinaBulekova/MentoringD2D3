using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTransformator
{
    public enum TrancformationOptions
    {
        None,
        IncrementDecrement,
        ReplaceParametrs,
        All
    }
    public class ExpressionTransformator : ExpressionVisitor
    {
        private const string NumberOne = "1";

        private int indent = 0;

        private Dictionary<string, int> replacers;

        private readonly TrancformationOptions _options = TrancformationOptions.None;

        public ExpressionTransformator()
        {

        }

        public ExpressionTransformator(TrancformationOptions options)
        {
            _options = options;
        }

        public Expression Visit(Expression node, Dictionary<string, int> dict)
        {
            if (node == null)
                return base.Visit(node);

            replacers = dict;
            Expression result = base.Visit(node);

            return result;
        }

        public override Expression Visit(Expression node)
        {
            if (node == null)
                return base.Visit(node);

            indent++;
            Expression result = base.Visit(node);
            Console.WriteLine("{0}{1} - {2}", new String(' ', indent * 4), result.NodeType, result.GetType());

            indent--;

            return result;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node == null)
                return base.VisitBinary(node);
            Expression newRight = IsNeedReplace(node.Right) ? ReplaceParameter(node.Right) : node.Right;
            Expression newLeft = IsNeedReplace(node.Left) ? ReplaceParameter(node.Left) : node.Left;
            BinaryExpression newNode = (newLeft != node.Left || newRight != node.Right)
                ? Expression.MakeBinary(node.NodeType, newLeft, newRight)
                : node;
            if(IsDecrement(newNode)) return base.Visit(Expression.Decrement(newNode.Left));
            if(IsIncrement(newNode)) return base.Visit(Expression.Increment(newNode.Left));

            return base.VisitBinary(newNode);
        }

        private bool IsIncrement(BinaryExpression node)
        {
            return (_options == TrancformationOptions.IncrementDecrement || _options == TrancformationOptions.All)
                && node.Right.NodeType == ExpressionType.Constant
                && node.NodeType == ExpressionType.Add
                && node.Right.ToString() == NumberOne;
        }

        private bool IsDecrement(BinaryExpression node)
        {
            return (_options == TrancformationOptions.IncrementDecrement || _options == TrancformationOptions.All)
                && node.Right.NodeType == ExpressionType.Constant
                && node.NodeType == ExpressionType.Subtract
                && node.Right.ToString() == NumberOne;
        }

        private bool IsNeedReplace(Expression node)
        {
            return (_options == TrancformationOptions.ReplaceParametrs || _options == TrancformationOptions.All)
                && node.NodeType == ExpressionType.Parameter;
        }

        private Expression ReplaceParameter(Expression parameter)
        {
            if (replacers != null)
            {
                var par = parameter as ParameterExpression;
                if (replacers.ContainsKey(par.Name))
                {
                    var val = replacers[par.Name];
                    return Expression.Constant(val);
                }
            }

            return parameter;
        }
    }
}

