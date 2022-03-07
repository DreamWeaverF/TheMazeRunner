using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace TheMazeRunner
{
    [CreateAssetMenu(fileName = "ThreadSynchronizationContextSO", menuName = "ScriptableObjects/ThreadSynchronizationContext/ThreadSynchronizationContextSO")]
    public class ThreadSynchronizationContextSO : ScriptableObject
    {
        private int threadId;
        private readonly ConcurrentQueue<Action> queue = new ConcurrentQueue<Action>();
        private Action action;

        public void Start()
        {
            threadId = Thread.CurrentThread.ManagedThreadId;
        }

        public void Update()
        {
            while (true)
            {
                if (!this.queue.TryDequeue(out action))
                {
                    return;
                }

                try
                {
                    action();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        public void Post(Action action)
        {
            if (Thread.CurrentThread.ManagedThreadId == this.threadId)
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }

                return;
            }

            this.queue.Enqueue(action);
        }

        public void PostNext(Action action)
        {
            this.queue.Enqueue(action);
        }
    }
}
