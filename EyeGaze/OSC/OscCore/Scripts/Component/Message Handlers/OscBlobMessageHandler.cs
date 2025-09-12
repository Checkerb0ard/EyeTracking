using System;

namespace OscCore
{
    public class OscBlobMessageHandler : MessageHandlerBase
    {
        public event Action<byte[], int> OnMessageReceived;
        
        protected byte[] m_Buffer = new byte[128];

        public byte[] Buffer => m_Buffer;
        public int LastReceivedBlobLength { get; private set; }

        public OscBlobMessageHandler(OscReceiver receiver, string address)
            : base(receiver, address)
        {
        }

        protected override void ValueRead(OscMessageValues values)
        {
            LastReceivedBlobLength = values.ReadBlobElement(0, ref m_Buffer);
        }
        
        protected override void InvokeEvent()
        {
            OnMessageReceived?.Invoke(m_Buffer, LastReceivedBlobLength);
        }
    }
}