using System;
using System.Collections.Generic;

namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            List<ProxyData> proxyDataList = FileLoadSystem.LoadProxy();
        }
    }
}