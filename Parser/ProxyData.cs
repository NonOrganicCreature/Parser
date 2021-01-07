namespace Parser
{
    public class ProxyData
    {
        private string _proxyValue;
        private int _proxyPort;
        private bool _using;

        public ProxyData(string proxyValue, int proxyPort)
        {
            _proxyValue = proxyValue;
            _proxyPort = proxyPort;
            _using = false;
        }

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
        
        public int ProxyPort
        {
            get => _proxyPort;
            set => _proxyPort = value;
        }
    }
}