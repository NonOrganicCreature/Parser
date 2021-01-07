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
                if (parserData.ParseResult == null)
                {
                    Thread parseThread = new Thread(Parse);
                    parseThread.Start(parserData);
                }
            }
        }

        protected override void Parse(object parseData)
        {
            base.Parse(parseData);
            
            ParserData<T> pd = (ParserData<T>) parseData;
            this.ParserDataList.Add(
                new ParserData<T>(
                    pd.ParseResult.Value.ToString(), 
                    new ParserOptions<T>(ParserOptionsEnum.Selector, Config.P_CLASS_SELECTOR), 
                    null)
                );
        }
    }
}