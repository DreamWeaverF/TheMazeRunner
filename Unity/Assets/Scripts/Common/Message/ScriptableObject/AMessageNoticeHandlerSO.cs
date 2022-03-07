using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheMazeRunner
{
    public abstract class AMessageNoticeHandlerSO<T1> : AMessageNoticeHandlerSO where T1 : AMessageNotice
    {
        private UnityEvent<T1> messageEvent = new UnityEvent<T1>();
        public override void OnHandMessage(AMessageNotice _notice)
        {
            messageEvent.Invoke(_notice as T1);
        }
        public override Type GetNoticeType()
        {
            return typeof(T1);
        }
        public abstract void OnHandMessage(T1 _notice);

        public void AddListener(UnityAction<T1> _call)
        {
            messageEvent.AddListener(_call);
        }

        public void RemoveListener(UnityAction<T1> _call)
        {
            messageEvent.RemoveListener(_call);
        }
    }

    public abstract class AMessageNoticeHandlerSO : ScriptableObject
    {
        public abstract void OnHandMessage(AMessageNotice _notice);
        public abstract Type GetNoticeType();
    }
}
