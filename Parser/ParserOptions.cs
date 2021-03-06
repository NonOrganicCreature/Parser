﻿using System;
using System.IO;
using System.Text;

namespace Parser
{
    public class ParserOptions<T> where T : class
    {
        private ParserOptionsEnum _optionsEnumValue;
        private string _parserOptionsAnchor;
        private string _attribute;

        public ParserOptions(ParserOptionsEnum optionsEnumValue, string parserOptionsAnchor, string attribute)
        {
            _optionsEnumValue = optionsEnumValue;
            _parserOptionsAnchor = parserOptionsAnchor;
            _attribute = attribute;
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

        public Func<StreamReader, T> GetParseMethod
        {
            get
            {
                Func<StreamReader, T> parseMethod = responseStream => null;
                
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
                            var node = doc.DocumentNode.SelectSingleNode(_parserOptionsAnchor);
                            string aString = "";
                            if (_attribute == null)
                            {
                                aString = node != null ? doc.DocumentNode.SelectSingleNode(_parserOptionsAnchor).InnerText : Config.END_OF_URL_PARSING;
                                Console.WriteLine("HERE");
                            }
                            else
                            {
                                aString = node != null ? doc.DocumentNode.SelectSingleNode(_parserOptionsAnchor).GetAttributeValue(_attribute, "") : Config.END_OF_URL_PARSING;
                            }
                            
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