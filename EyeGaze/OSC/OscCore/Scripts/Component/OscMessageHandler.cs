using System;
using EyeTracking;

namespace OscCore
{
    public abstract class MessageHandlerBase : IDisposable
    {
        protected OscReceiver m_Receiver;
        public OscReceiver Receiver => m_Receiver;
        
        protected string m_Address = "/";
        public string Address => m_Address;
        
        protected OscActionPair m_ActionPair;
        protected bool m_Registered;
        protected bool m_Disposed;
        
        public MessageHandlerBase(OscReceiver receiver, string address)
        {
            m_Receiver = receiver;
            m_Address = address;
            Initialize();
        }
        
        protected virtual void Initialize()
        {
            if (m_Registered || string.IsNullOrEmpty(Address))
                return;

            if (m_Receiver?.Server != null)
            {
                m_ActionPair = new OscActionPair(ValueRead, InvokeEvent);
                Receiver.Server.TryAddMethodPair(Address, m_ActionPair);
                m_Registered = true;
            }
        }

        public virtual void Dispose()
        {
            if (m_Disposed) return;
            
            m_Registered = false;
            m_Receiver?.Server?.RemoveMethodPair(Address, m_ActionPair);
            m_Disposed = true;
        }

        protected abstract void InvokeEvent();
        protected abstract void ValueRead(OscMessageValues values);
    }
    
    public abstract class OscMessageHandler<T> : MessageHandlerBase
    {
        public event Action<T> OnMessageReceived;
        
        protected T m_Value;
        
        public OscMessageHandler(OscReceiver receiver, string address) : base(receiver, address)
        {
        }
        
        protected override void InvokeEvent()
        {
            OnMessageReceived?.Invoke(m_Value);
        }
    }
}