using System;

namespace OscCore
{
    public class OscStringMessageHandler : OscMessageHandler<string>
    {
        public OscStringMessageHandler(OscReceiver receiver, string address)
            : base(receiver, address)
        {
        }

        protected override void ValueRead(OscMessageValues values)
        {
            m_Value = values.ReadStringElement(0);
        }
    }
}