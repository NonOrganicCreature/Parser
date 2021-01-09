using System;
using System.Collections.Generic;

namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            List<ProxyData> proxyDataList = FileLoadSystem.LoadProxy();
            List<ParserData<string>> parserData = new List<ParserData<string>>();
            
            string startUrl = "https://ranobes.com/chapters/armsbs/221493-glava-234-skrytaja-voina-chast-11.html";
            parserData.Add(new ParserData<string>(startUrl, new ParserOptions<string>(ParserOptionsEnum.Selector, Config.P_CLASS_SELECTOR, Config.ATTR_HREF), null));
            
            Parser<string> urlGather = new URLGather<string>(parserData);
            urlGather.Run();

            List<ParserData<string>> pageParserData = new List<ParserData<string>>();
            
            foreach (var data in parserData)
            {
                pageParserData.Add(new ParserData<string>(
                    data.Url,
                    new ParserOptions<string>(ParserOptionsEnum.Selector, Config.HIDDEN_WAR_SELECTOR, null), 
                    null
                ));
            }
            Parser<string> pageParser = new MultiThreadingParser<string>(pageParserData, proxyDataList);
            pageParser.Run();

        }
    }
}