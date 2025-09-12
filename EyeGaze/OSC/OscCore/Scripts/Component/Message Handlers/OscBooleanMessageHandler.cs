using System;

namespace OscCore
{
    public class OscBooleanMessageHandler : OscMessageHandler<bool>
    {
        public OscBooleanMessageHandler(OscReceiver receiver, string address) 
            : base(receiver, address)
        {
        }

        protected override void ValueRead(OscMessageValues values)
        {
            m_Value = values.ReadBooleanElement(0);
        }
    }
}