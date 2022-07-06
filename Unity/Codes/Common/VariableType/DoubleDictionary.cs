using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMazeRunner
{
    public class DoubleDictionary<T1,T2> 
    {
        private Dictionary<T1, T2> dict1 = new Dictionary<T1, T2>();
        private Dictionary<T2, T1> dict2 = new Dictionary<T2, T1>();

        public void AddKey(T1 _key,T2 _value)
        {
            dict1.Add(_key,_value);
            dict2.Add(_value, _key);
        }

        public void AddValue(T2 _value,T1 _key)
        {
            dict2.Add(_value, _key);
            dict1.Add(_key, _value);
        }

        public bool TryGetValue(T1 _key,out T2 _value)
        {
            return dict1.TryGetValue(_key, out _value);
        }

        public bool TryGetKey(T2 _value,T1 _key)
        {
            return dict2.TryGetValue(_value, out _key);
        }

        public void RemoveKey(T1 _key)
        {
            if(!dict1.TryGetValue(_key, out T2 _value))
            {
                return;
            }
            dict1.Remove(_key);
            dict2.Remove(_value);
        }

        public void RemoveValue(T2 _value)
        {
            if (!dict2.TryGetValue(_value, out T1 _key))
            {
                return;
            }
            dict1.Remove(_key);
            dict2.Remove(_value);
        }

        public void UpdateValue(T1 _key,T2 _value)
        {
            if (!dict1.TryGetValue(_key, out T2 _oldvalue))
            {
                return;
            }
            dict1[_key] = _value;
            dict2.Remove(_oldvalue);
            dict2.Add(_value, _key);
        }

        public void UpdateKey(T2 _value, T1 _key)
        {
            if (!dict2.TryGetValue(_value, out T1 _oldkey))
            {
                return;
            }
            dict2[_value] = _key;
            dict1.Remove(_oldkey);
            dict1.Add(_key, _value);
        }

    }
}
