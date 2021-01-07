using System;
using System.Collections.Generic;
using System.IO;

namespace Parser
{
    public static class FileLoadSystem
    {
        public static List<ProxyData> LoadProxy()
        {
            List<ProxyData> pd = new List<ProxyData>();
            using (FileStream fs = File.OpenRead(Config.PROXY_PATH))
            {
                byte[] array = new byte[fs.Length];
                fs.Read(array, 0, array.Length);
                string[] textFromFile = System.Text.Encoding.UTF8.GetString(array).Split('\n');
                foreach (var proxy in textFromFile)
                {
                    string[] proxyAndPort = proxy.Split(':');
                    pd.Add(new ProxyData(proxyAndPort[0], Int32.Parse(proxyAndPort[1].TrimEnd())));
                }
            }
            return pd;
        }
    }
}