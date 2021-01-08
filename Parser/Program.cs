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
            
            string startUrl = "https://ranobes.com/chapters/armsbs/218157-chapter-1.html";
            parserData.Add(new ParserData<string>(startUrl, new ParserOptions<string>(ParserOptionsEnum.Selector, Config.P_CLASS_SELECTOR, Config.ATTR_HREF), null));
            
            URLGather<string> urlGather = new URLGather<string>(parserData);
            
            urlGather.Run();
            
        }
    }
}