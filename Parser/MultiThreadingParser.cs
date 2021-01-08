using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Parser
{
    public class MultiThreadingParser<T> : Parser<T> where T : class
    {
        public override List<ParserData<T>> ParserDataList { get; }
        public List<ProxyData> ProxyDataList { get; }
        private object lockObj = new object();
        private bool parsingDone = false;

        private int callCount = 0;
        public MultiThreadingParser(List<ParserData<T>> initializeParserData, List<ProxyData> proxyData)
        {
            ParserDataList = initializeParserData;
            ProxyDataList = proxyData;
        }
        
        public override void Run()
        {
            List<Thread> threads = new List<Thread>();
            foreach (var parserData in ParserDataList)
            {
                if (parserData.ParseResult == null)
                {
                    Thread parseThread = new Thread(Parse);
                    threads.Add(parseThread);
                    parseThread.Start(parserData);
                }
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
            
            ParserData<T> someNotDone = ParserDataList.Find(parserData => parserData.ParseResult == null);
            if (someNotDone != null)
            {
                Run();
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
                    proxyD = ProxyDataList.Find(proxy => !proxy.Using && proxy.Valid);
                    if (proxyD != null)
                    {
                        proxyD.Using = true;
                        break; 
                    }
                }
            }
            
            HttpWebRequest request = WebRequest.CreateHttp(parseD.Url.Replace("https", "http"));
            WebProxy webProxy = new WebProxy(proxyD.ProxyValue, proxyD.ProxyPort);
            request.Proxy = webProxy;
            request.KeepAlive = false;
            request.Credentials = CredentialCache.DefaultCredentials;
            request.ContentType = "text/html; charset=UTF-8";
            request.UserAgent =
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.141 Safari/537.36";
            request.Accept = "text/html";
            request.Timeout = 2000;
            try
            {
                StreamReader responseStream =
                    new StreamReader(request.GetResponse().GetResponseStream(), Encoding.UTF8);
                proxyD.Using = false;
                
                parseD.ParseResult = new ParseResult<T>();
                parseD.ParserOptions = new ParserOptions<T>(ParserOptionsEnum.Selector, Config.HIDDEN_WAR_SELECTOR,
                    null);
                parseD.ParseResult.Value = parseD.ParserOptions.GetParseMethod(responseStream);
            }
            catch (WebException webException)
            {
                proxyD.Using = false;
                int error = webException.Response == null ? 0 : (int)((HttpWebResponse) webException.Response).StatusCode;
                if (error == 403 || error == 500 || error == 408 || error == 0 || error == 400)
                {
                    // proxyD.Valid = false;
                }
                // Console.WriteLine("Proxy av: " + ProxyDataList.FindAll(proxy => proxy.Valid).Count);
                
                
                Console.Error.WriteLine(webException.Message);
            }

        }
        
    }
}