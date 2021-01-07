using System;
using System.IO;
using System.Text;

namespace Parser
{
    public class ParserOptions<T> where T : class
    {
        private ParserOptionsEnum _optionsEnumValue;
        private string _parserOptionsAnchor;

        public ParserOptions(ParserOptionsEnum optionsEnumValue, string parserOptionsAnchor)
        {
            _optionsEnumValue = optionsEnumValue;
            _parserOptionsAnchor = parserOptionsAnchor;
        }

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

        public Func<Stream, T> GetParseMethod
        {
            get
            {
                Func<Stream, T> parseMethod = responseStream => null;
                
                var doc = new HtmlAgilityPack.HtmlDocument();
                HtmlAgilityPack.HtmlNode.ElementsFlags["br"] = HtmlAgilityPack.HtmlElementFlag.Empty;
                doc.OptionWriteEmptyNodes = true;
                
                
                switch (_optionsEnumValue)
                {
                    case ParserOptionsEnum.Selector:
                    {
                        parseMethod = responseStream =>
                        {
                            doc.Load(responseStream);
                            responseStream.Close();
                            var aString = doc.DocumentNode.SelectSingleNode(_parserOptionsAnchor).InnerHtml.ToString();
                            T result = aString as T;
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