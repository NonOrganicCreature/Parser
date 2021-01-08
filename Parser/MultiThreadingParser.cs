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
            // request.Proxy = webProxy;
            request.KeepAlive = false;
            request.Credentials = CredentialCache.DefaultCredentials;
            request.ContentType = "text/html; charset=UTF-8";
            request.Timeout = 2000;
            try
            {
                StreamReader responseStream =
                    new StreamReader(request.GetResponse().GetResponseStream(), Encoding.UTF8);
                parseD.ParseResult = new ParseResult<T>();
                parseD.ParserOptions = new ParserOptions<T>(ParserOptionsEnum.Selector, Config.P_CLASS_SELECTOR,
                    Config.ATTR_HREF);
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