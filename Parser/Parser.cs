using System.Collections.Generic;

namespace Parser
{
    public abstract class Parser<T>
    {
        public abstract Queue<ParserData<T>> ParserDataQueue { get; }
        public abstract void Run();
        protected abstract void Parse(object parseData);
    }
}