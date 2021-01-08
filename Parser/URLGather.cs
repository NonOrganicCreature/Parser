using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Parser
{
    public class URLGather<T> : Parser<T> where T : class
    {
        public override List<ParserData<T>> ParserDataList { get; }
        public URLGather(List<ParserData<T>> initializeParserData)
        {
            ParserDataList = initializeParserData;
        }

        public override void Run()
        {
            Parse(ParserDataList[0]);
        }

        protected override void Parse(object parseData)
        {
            ParserData<T> parseD = (ParserData<T>) parseData;
            HttpWebRequest request = WebRequest.CreateHttp(parseD.Url);
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
                Console.Error.WriteLine(webException.Message);
                this.Parse(parseData);
                return;
            }

            Console.WriteLine(parseD.ParseResult.Value.ToString());
            
            if (parseD.ParseResult.Value.ToString() == Config.END_OF_URL_PARSING)
            {
                return;
            }

            var newParserData = new ParserData<T>(
                parseD.ParseResult.Value.ToString(),
                new ParserOptions<T>(ParserOptionsEnum.Selector, Config.P_CLASS_SELECTOR, Config.ATTR_HREF),
                null);
            
            ParserDataList.Add(newParserData);
            Parse(newParserData);
        }
    }
}