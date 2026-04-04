using System;

namespace OscCore
{
    public class OscFloatMessageHandler : OscMessageHandler<float>
    {
        public OscFloatMessageHandler(OscReceiver receiver, string address)
            : base(receiver, address)
        {
        }

        protected override void ValueRead(OscMessageValues values)
        {
            m_Value = values.ReadFloatElement(0);
        }
    }
}