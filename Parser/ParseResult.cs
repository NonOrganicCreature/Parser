namespace Parser
{
    public class ParseResult<T>
    {
        private T _value;

        public T Value
        {
            get => _value;
            set => _value = value;
        }
    }
}