namespace Parser
{
    public class ParserData<T> where T : class
    {
        private string _URL;
        private ParserOptions<T> _parserOptions;
        private ParseResult<T> _parseResult;

        public ParserData(string url, ParserOptions<T> parserOptions, ParseResult<T> parseResult)
        {
            _URL = url;
            _parserOptions = parserOptions;
            _parseResult = parseResult;
        }

        public string Url
        {
            get => _URL;
            set => _URL = value;
        }

        public ParserOptions<T> ParserOptions
        {
            get => _parserOptions;
            set => _parserOptions = value;
        }

        public ParseResult<T> ParseResult
        {
            get => _parseResult;
            set => _parseResult = value;
        }
    }
}