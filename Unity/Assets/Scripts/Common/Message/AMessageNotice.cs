using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace TheMazeRunner
{
    [MessagePackObject]
    public abstract class AMessageNotice : IMessage
    {
        
    }

    
    public abstract class AMessageNoticeSenderSO : ScriptableObject
    {
        public DelegateSendMessageEvent SendMessageEvent;
        public abstract void OnSendMessage(AMessageNotice _notice);
        public abstract Type GetNoticeType();
    }


    public abstract class AMessageNoticeSender<T1> : AMessageNoticeSenderSO where T1 : AMessageNotice
    {
        public void OnSendMessage(T1 _notice)
        {
            SendMessageEvent.Invoke(_notice);
        }
        public override Type GetNoticeType()
        {
            return typeof(T1);
        }
    }

    public abstract class AMessageNoticeHandlerSO : ScriptableObject
    {
        public abstract void OnHandMessage(AMessageNotice _notice);
        public abstract Type GetNoticeType();
        public abstract void AddListener(UnityAction<IMessage> _call);
        public abstract void RemoveListener(UnityAction<IMessage> _call);
    }

    public abstract class AMessageNoticeHandler<T1> : AMessageNoticeHandlerSO where T1 : AMessageNotice
    {
        public override void OnHandMessage(AMessageNotice _notice)
        {
            OnHandMessage(_notice as T1);
        }
        public override Type GetNoticeType()
        {
            return typeof(T1);
        }
        public abstract void OnHandMessage(T1 _notice);
    }
}
