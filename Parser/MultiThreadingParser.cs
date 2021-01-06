using System.Collections.Generic;
using System.Threading;

namespace Parser
{
    public class MultiThreadingParser<T> : Parser<T>
    {
        public override Queue<ParserData<T>> ParserDataQueue { get; }
        public List<ProxyData> ProxyDataList { get; }
        public object lockObj;
        
        public MultiThreadingParser(List<ParserData<T>> initializeParserData, List<ProxyData> proxyData)
        {
            ParserDataQueue = new Queue<ParserData<T>>(initializeParserData);
            ProxyDataList = proxyData;
        }
        
        public override void Run()
        {
            while (ParserDataQueue.Count > 0)
            {
                Thread parseThread = new Thread(Parse);
                parseThread.Start(ParserDataQueue.Dequeue());
            }
        }

        protected override void Parse(object parseData)
        {
            ParserData<T> pd = (ParserData<T>)parseData;
        }
    }
}