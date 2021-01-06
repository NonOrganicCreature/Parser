namespace Parser
{
    public class ProxyData
    {
        private string _proxyValue;
        private bool _using;

        public string ProxyValue
        {
            get => _proxyValue;
            set => _proxyValue = value;
        }

        public bool Using
        {
            get => _using;
            set => _using = value;
        }
    }
}