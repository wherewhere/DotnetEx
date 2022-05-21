using System.Net.Sockets;
using System.Resources;

namespace System.Net
{
    public class DnsEndPoint : EndPoint
    {
        private readonly AddressFamily _family;

        public DnsEndPoint(string host, int port) : this(host, port, AddressFamily.Unspecified) { }

        public DnsEndPoint(string host, int port, AddressFamily addressFamily)
        {
            ArgumentNullExceptionEx.ThrowIfNull(host);

            if (port is < IPEndPoint.MinPort or > IPEndPoint.MaxPort)
            {
                throw new ArgumentOutOfRangeException(nameof(port));
            }

            if (addressFamily is not AddressFamily.InterNetwork and
                not AddressFamily.InterNetworkV6 and
                not AddressFamily.Unspecified)
            {
                throw new ArgumentException(Strings.net_sockets_invalid_optionValue_all, nameof(addressFamily));
            }

            Host = host;
            Port = port;
            _family = addressFamily;
        }

        public override bool Equals(object? comparand)
        {
            return comparand is DnsEndPoint dnsComparand
&& _family == dnsComparand._family &&
                    Port == dnsComparand.Port &&
                    Host == dnsComparand.Host;
        }

        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(ToString());
        }

        public override string ToString()
        {
            return _family + "/" + Host + ":" + Port;
        }

        public string Host { get; }

        public override AddressFamily AddressFamily
        {
            get
            {
                return _family;
            }
        }

        public int Port { get; }
    }
}
