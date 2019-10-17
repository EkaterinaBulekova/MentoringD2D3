namespace ExpressionMapper
{
    public interface IMapper<TOut>
    {
        TOut Map(object source);
    }
}