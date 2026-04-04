using System;

namespace OscCore
{
    public class OscFloat64MessageHandler : OscMessageHandler<double>
    {
        public OscFloat64MessageHandler(OscReceiver receiver, string address)
            : base(receiver, address)
        {
        }

        protected override void ValueRead(OscMessageValues values)
        {
            m_Value = values.ReadFloat64Element(0);
        }
    }
}