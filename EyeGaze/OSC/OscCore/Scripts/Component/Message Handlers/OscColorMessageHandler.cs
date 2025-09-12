using System;
using UnityEngine;

namespace OscCore
{
    public class OscColorMessageHandler : OscMessageHandler<Color>
    {
        public OscColorMessageHandler(OscReceiver receiver, string address)
            : base(receiver, address)
        {
        }

        protected override void ValueRead(OscMessageValues values)
        {
            m_Value = values.ReadColor32Element(0);
        }
    }
}