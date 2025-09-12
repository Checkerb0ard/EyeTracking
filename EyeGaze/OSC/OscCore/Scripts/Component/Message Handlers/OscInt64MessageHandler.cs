using System;

namespace OscCore
{
    public class OscInt64MessageHandler : OscMessageHandler<long>
    {
        public OscInt64MessageHandler(OscReceiver receiver, string address)
            : base(receiver, address)
        {
        }

        protected override void ValueRead(OscMessageValues values)
        {
            m_Value = values.ReadInt64Element(0);
        }
    }
}