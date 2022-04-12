using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Events;

namespace TheMazeRunner
{
    public static class PropertyChangeBroadcast
    {
        private static readonly Dictionary<int,UnityEventBase> events = new Dictionary<int, UnityEventBase>();

        public static void AddListener<T1,T2>(this T1 _classType, string _propName, UnityAction<T2> _callback)
        {
            PropertyInfo _info = _classType.GetType().GetProperty(_propName);
            if (_info == null)
            {
                return;
            }
            if (!events.TryGetValue(_info.MetadataToken, out UnityEventBase _event))
            {
                _event = new UnityEvent<T2>();
                events.Add(_info.MetadataToken, _event);
            }
            (_event as UnityEvent<T2>).AddListener(_callback);
        }

        public static void RemoveListener<T1,T2>(this T1 _classType, string _propName, UnityAction<T2> _action)
        {
            PropertyInfo _info = _classType.GetType().GetProperty(_propName);
            if (_info == null)
            {
                return;
            }
            if (!events.TryGetValue(_info.MetadataToken, out UnityEventBase _event))
            {
                return;
            }
            (_event as UnityEvent<T2>).RemoveListener(_action);
        }

        public static void SetValueBoradCast<T1,T2>(this T1 _classType,string _propName,T2 _value)
        {
            PropertyInfo _info = _classType.GetType().GetProperty(_propName);
            if (_info == null)
            {
                return;
            }
            _info.SetValue(_classType, _value);
            if (!events.TryGetValue(_info.MetadataToken, out UnityEventBase _event))
            {
                return;
            }
            (_event as UnityEvent<T2>).Invoke(_value);
        }
    }
}
