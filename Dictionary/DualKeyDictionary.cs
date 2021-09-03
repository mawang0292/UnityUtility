using System.Collections.Generic;
using System;
using System.Linq;

 public class DualKeyDictionary<TKey1, TKey2, TValue> : Dictionary<TKey1, Dictionary<TKey2, TValue>>
    {
        public TValue this[TKey1 key1 , TKey2 key2]
        {
            get
            {
                if(!ContainsKey(key1) || this[key1].ContainsKey(key2))
                {
                    throw new ArgumentOutOfRangeException();
                }
                return base[key1][key2];
            }
            set
            {
                if(!ContainsKey(key1))
                {
                    this[key1] = new Dictionary<TKey2, TValue>();
                }
                this [key1][key2] = value;
            }

        }

        public new IEnumerable<TValue> Values
        {
            get
            {
                return from baseDictionary in base.Values
                       from baseKey        in baseDictionary.Keys
                       select baseDictionary[baseKey];
            }
        }   
        public bool TryGetValue(TKey1 key1 , TKey2 key2, out TValue value)
        {
            if(ContainsKey(key1) && this[key1].ContainsKey(key2))
            {
                value = this[key1][key2];
                return true;
            }
            else
            {
                value = default(TValue);
                return false;
            }
            //return base.ContainsKey(key1) && this[key1].ContainsKey(key2);
        }

        public void Add(TKey1 key1 , TKey2 key2, TValue value)
        {
            if(!ContainsKey(key1))
            {
                this[key1] = new Dictionary<TKey2, TValue>();
            }
            this[key1][key2] = value;
        }
        public bool ContaninKey(TKey1 key1 ,TKey2 key2)
        {
            return base.ContainsKey(key1) && this[key1].ContainsKey(key2);
        }
    }