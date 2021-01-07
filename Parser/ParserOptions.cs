using System;

namespace Parser
{
    public class ParserOptions<T> where T : class
    {
        private ParserOptionsEnum _optionsEnumValue;
        private string _parserOptionsAnchor;

        public ParserOptionsEnum OptionsEnumValue
        {
            get => _optionsEnumValue;
            set => _optionsEnumValue = value;
        }

        public string ParserOptionsAnchor
        {
            get => _parserOptionsAnchor;
            set => _parserOptionsAnchor = value;
        }

        public Func<string, T> GetParseMethod
        {
            get
            {
                Func<string, T> parseMethod = delegate(string s)
                {
                    T result = s as T;
                    return result;
                };
                switch (_optionsEnumValue)
                {
                    case ParserOptionsEnum.Tag:
                    {
                        parseMethod = stringToParse =>
                        {
                            T result = stringToParse as T;
                            return result;
                        };
                    }
                        break;
                }

                return parseMethod;
            }
        }
    }
}