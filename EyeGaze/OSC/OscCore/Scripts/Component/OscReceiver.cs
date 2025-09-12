using System;

namespace OscCore
{
    public class OscReceiver : IDisposable
    {
        private int m_Port = 9000;
        private bool m_Disposed;

        public int Port
        {
            get => m_Port;
            set => SetPort(value);
        }

        public bool Running { get; private set; }
        public OscServer Server { get; private set; }

        public OscReceiver(int port = 9000)
        {
            m_Port = port.ClampPort();
            Initialize();
        }

        private void Initialize()
        {
            Server = OscServer.GetOrCreate(m_Port);
            Running = true;
        }

        public void Update()
        {
            Server?.Update();
        }

        public void Dispose()
        {
            if (m_Disposed) return;
            
            Server?.Dispose();
            Server = null;
            Running = false;
            m_Disposed = true;
        }

        private void SetPort(int newPort)
        {
            var clamped = newPort.ClampPort();
            if (clamped != newPort) return;

            var oldValue = m_Port;
            var oldServer = Server;
            try
            {
                Server = OscServer.GetOrCreate(newPort);
                m_Port = newPort;
                oldServer?.Dispose();
            }
            catch (Exception e)
            {
                // Log error appropriately
                m_Port = oldValue;
                Server = oldServer;
            }
        }
    }
}