using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace Parser
{
    public class MultiThreadingParser<T> : Parser<T> where T : class
    {
        public override List<ParserData<T>> ParserDataList { get; }
        public List<ProxyData> ProxyDataList { get; }
        public object lockObj = new object();
        
        public MultiThreadingParser(List<ParserData<T>> initializeParserData, List<ProxyData> proxyData)
        {
            ParserDataList = initializeParserData;
            ProxyDataList = proxyData;
        }
        
        public override void Run()
        {
            foreach (var parserData in ParserDataList)
            {
                Thread parseThread = new Thread(Parse);
                parseThread.Start(parserData);
            }
        }

        protected override void Parse(object parseData)
        {
            ParserData<T> parseD = (ParserData<T>)parseData;
            ProxyData proxyD;
            while (true)
            {
                lock (lockObj)
                {
                    proxyD = ProxyDataList.Find(proxy => !proxy.Using);
                    if (proxyD != null)
                    {
                        proxyD.Using = true;
                        break; 
                    }
                }
            }

            HttpWebRequest request = WebRequest.CreateHttp(parseD.Url);
            WebProxy webProxy = new WebProxy(proxyD.ProxyValue, proxyD.ProxyPort);
            // request.Proxy = webProxy;
            request.ContentType = "text/html; charset=UTF-8";
            Stream responseStream = request.GetResponse().GetResponseStream();
            
            
            parseD.ParseResult = new ParseResult<T>();
            parseD.ParseResult.Value = parseD.ParserOptions.GetParseMethod(responseStream);
        }
        
    }
}