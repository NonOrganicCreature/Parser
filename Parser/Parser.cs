using System.Collections.Generic;

namespace Parser
{
    public abstract class Parser<T> where T : class
    {
        public abstract List<ParserData<T>> ParserDataList { get; }
        public abstract void Run();
        protected abstract void Parse(object parseData);

    }
}