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
            
            string startUrl = "https://ranobes.com/chapters/fff-class-trashero/122176-illjustracii-vozmozhny-spojlery.html";
            parserData.Add(new ParserData<string>(startUrl, new ParserOptions<string>(ParserOptionsEnum.Selector, Config.P_CLASS_SELECTOR), null));
            
            URLGather<string> urlGather = new URLGather<string>(parserData, proxyDataList);
            
            urlGather.Run();
            
        }
    }
}