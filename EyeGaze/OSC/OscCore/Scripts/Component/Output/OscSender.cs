using System;
using System.Net;

namespace OscCore
{
    public class OscSender : IDisposable
    {
        private string m_IpAddress = "127.0.0.1";
        private int m_Port = 7000;
        private bool m_Disposed;

        /// <summary>The IP address to send to</summary>
        public string IpAddress
        {
            get => m_IpAddress;
            set
            {
                if (IPAddress.TryParse(value, out var ip))
                {
                    m_IpAddress = value;
                    ReInitialize();
                }
            }
        }

        /// <summary>The port on the remote IP to send to</summary>
        public int Port
        {
            get => m_Port;
            set
            {
                m_Port = value.ClampPort();
                ReInitialize();
            }
        }

        /// <summary>
        /// Handles serializing and sending messages. Use methods on this to send messages to the endpoint.
        /// </summary>
        public OscClient Client { get; private set; }

        public OscSender(int port = 7000, string ipAddress = "127.0.0.1")
        {
            m_IpAddress = ipAddress;
            m_Port = port.ClampPort();
            Setup();
        }

        private void Setup()
        {
            if (Client == null)
                Client = new OscClient(m_IpAddress, m_Port);
        }

        private void ReInitialize()
        {
            if (m_Disposed) return;
            
            Client = null;
            Setup();
        }

        public void Dispose()
        {
            if (m_Disposed) return;
            
            Client = null;
            m_Disposed = true;
        }
    }
}