using System;

namespace OscCore
{
    public class OscIntMessageHandler : OscMessageHandler<int>
    {
        public OscIntMessageHandler(OscReceiver receiver, string address)
            : base(receiver, address)
        {
        }

        protected override void ValueRead(OscMessageValues values)
        {
            m_Value = values.ReadIntElement(0);
        }
    }
}