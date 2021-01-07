using System.Collections.Generic;
using System.Threading;

namespace Parser
{
    public class URLGather<T> : MultiThreadingParser<T> where T : class
    {
        public URLGather(List<ParserData<T>> initializeParserData, List<ProxyData> proxyData) : base(initializeParserData, proxyData)
        {
        }

        public override void Run()
        {
            foreach (var parserData in ParserDataList)
            {
                if (parserData.ParseResult != null)
                {
                    Thread parseThread = new Thread(Parse);
                    parseThread.Start(parserData);
                }
            }
        }

        public new void Parse(object parseData)
        {
            
        }
    }
}