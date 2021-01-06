using System;

namespace Parser
{
    public class ParserOptions
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

        public Func<string, string> GetParseMethod()
        {
            Func<string, string> parseMethod = stringToParse => { return stringToParse;};
            switch (_optionsEnumValue)
            {
                case ParserOptionsEnum.Tag:
                {
                    parseMethod = stringToParse =>
                    {
                        return stringToParse;
                    };
                }
                break;
            }

            return parseMethod;
        }
        
    }
}