using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionMapper
{
    public class ExpressionMapper<TOut> : IMapper<TOut>
    {
        private readonly Dictionary<Type, Func<object, TOut>> _converters;
        private readonly ConstructorInfo _outConstructor;
        private readonly Dictionary<string, PropertyInfo> _outProperties;
        private readonly Type _outType;
        private readonly ParameterExpression _outInstance;


        public ExpressionMapper()
        {
            _converters = new Dictionary<Type, Func<object, TOut>>();

            _outType = typeof(TOut);
            _outProperties = _outType.GetProperties().ToDictionary(p => p.Name);
            _outConstructor = _outType.GetConstructor(Array.Empty<Type>());
            _outInstance = Expression.Variable(_outType, "outInstance");

            if (_outConstructor == null)
            {
                throw new Exception($"Default constructor for {_outType.Name} not found");
            }
        }

        public TOut Map(object source)
        {
            var sourceType = source.GetType();
            if (_converters.TryGetValue(sourceType, out var existsConverter))
            {
                return existsConverter(source);
            }

            var converter = BuildConverter(sourceType);
            _converters.Add(sourceType, converter);

            return converter(source);
        }

        private Func<object, TOut> BuildConverter(Type sourceType)
        {
            var parameter = Expression.Parameter(typeof(object), "source");
            var body = BuildBody(sourceType, parameter);

            return Expression.Lambda<Func<object, TOut>>(body, parameter).Compile();
        }

        private BlockExpression BuildBody(Type sourceType, Expression parameter)
        {
            var sourceInstance = Expression.Variable(sourceType, "typedSource");
            var expressions = GetExpressions(sourceType, parameter, sourceInstance);

            return Expression.Block(new[] { sourceInstance, _outInstance }, expressions);
        }

        private List<Expression> GetExpressions(Type sourceType, Expression parameter, Expression sourceInstance)
        {
            var expressions = CastOutInstance(sourceType, parameter, sourceInstance);
            expressions.AddRange(CastOutProperties(sourceType, sourceInstance));
            expressions.Add(_outInstance);

            return expressions;
        }

        private List<Expression> CastOutInstance(Type sourceType, Expression parameter, Expression sourceInstance)
        {
            return new List<Expression>
            {
                Expression.Assign(sourceInstance, Expression.Convert(parameter, sourceType)),
                Expression.Assign(_outInstance, Expression.New(_outConstructor))
            };
        }

        private IEnumerable<Expression> CastOutProperties(Type sourceType, Expression sourceInstance)
        {
            return GetCommonProperties(sourceType)
                .Select(property => CastProperty(sourceInstance, property));
        }

        private Expression CastProperty(Expression sourceInstance, PropertyInfo sourceProp)
        {
            return Expression.Assign(
                    Expression.Property(_outInstance, _outProperties[sourceProp.Name]),
                    Expression.Property(sourceInstance, sourceProp));
        }

        private IEnumerable<PropertyInfo> GetCommonProperties(Type sourceType)
        {
            return sourceType.GetProperties().Where(sourceProp => _outProperties.ContainsKey(sourceProp.Name));
        }
    }
}