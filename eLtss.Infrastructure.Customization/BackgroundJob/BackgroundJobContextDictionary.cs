﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace eLtss.Infrastructure.Customization.BackgroundJob
{
    public class BackgroundJobContextDictionary : IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable
    {
        // Fields
        private Dictionary<string, object> _dictionary;

        // Methods
        public BackgroundJobContextDictionary()
        {
            this._dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        public BackgroundJobContextDictionary(IDictionary<string, object> dictionary)
        {
            this._dictionary = new Dictionary<string, object>(dictionary, StringComparer.OrdinalIgnoreCase);
        }

        public BackgroundJobContextDictionary(object values)
        {
            this._dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            this.AddValues(values);
        }

        public void Add(string key, object value)
        {
            this._dictionary.Add(key, value);
        }

        private void AddValues(object values)
        {
            if(values != null)
            {
                foreach(PropertyDescriptor descriptor in TypeDescriptor.GetProperties(values))
                {
                    object obj2 = descriptor.GetValue(values);
                    this.Add(descriptor.Name, obj2);
                }
            }
        }

        public void Clear()
        {
            this._dictionary.Clear();
        }

        public bool ContainsKey(string key)
        {
            return this._dictionary.ContainsKey(key);
        }

        public bool ContainsValue(object value)
        {
            return this._dictionary.ContainsValue(value);
        }

        public Dictionary<string, object>.Enumerator GetEnumerator()
        {
            return this._dictionary.GetEnumerator();
        }

        public bool Remove(string key)
        {
            return this._dictionary.Remove(key);
        }

        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
        {
            this._dictionary.Add(item.Key, item.Value); //this._dictionary.Add(item);
        }

        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
        {
            return this._dictionary.Contains(item);
        }

        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            (this._dictionary as ICollection<KeyValuePair<string,object>>).CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
        {
            return this._dictionary.Remove(item.Key); //this._dictionary.Remove(item);
        }

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public bool TryGetValue(string key, out object value)
        {
            return this._dictionary.TryGetValue(key, out value);
        }

        // Properties
        public int Count
        {
            get
            {
                return this._dictionary.Count;
            }
        }

        public object this[string key]
        {
            get
            {
                object obj2;
                this.TryGetValue(key, out obj2);
                return obj2;
            }
            set
            {
                this._dictionary[key] = value;
            }
        }

        public Dictionary<string, object>.KeyCollection Keys
        {
            get
            {
                return this._dictionary.Keys;
            }
        }

        bool ICollection<KeyValuePair<string, object>>.IsReadOnly
        {
            get
            {
                return false; // this._dictionary.IsReadOnly;
            }
        }

        ICollection<string> IDictionary<string, object>.Keys
        {
            get
            {
                return this._dictionary.Keys;
            }
        }

        ICollection<object> IDictionary<string, object>.Values
        {
            get
            {
                return this._dictionary.Values;
            }
        }

        public Dictionary<string, object>.ValueCollection Values
        {
            get
            {
                return this._dictionary.Values;
            }
        }
    }
}
