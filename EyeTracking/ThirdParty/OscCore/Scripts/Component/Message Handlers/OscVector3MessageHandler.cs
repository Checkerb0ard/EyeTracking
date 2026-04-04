using System;
using UnityEngine;

namespace OscCore
{
    public class OscVector3MessageHandler : OscMessageHandler<Vector3>
    {
        public OscVector3MessageHandler(OscReceiver receiver, string address)
            : base(receiver, address)
        {
        }

        protected override void ValueRead(OscMessageValues values)
        {
            var x = values.ReadFloatElement(0);
            var y = values.ReadFloatElement(1);
            var z = values.ReadFloatElement(2);
            m_Value = new Vector3(x, y, z);
        }
    }
}