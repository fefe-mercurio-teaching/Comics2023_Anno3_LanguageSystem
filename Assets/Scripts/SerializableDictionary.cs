using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, 
    ISerializationCallbackReceiver
{
    public List<TKey> keys = new();
    public List<TValue> values = new();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        foreach (TKey key in this.Keys)
        {
            keys.Add(key);
            values.Add(this[key]);
        }
    }

    public void OnAfterDeserialize()
    {
        Clear();

        for (int i = 0; i < keys.Count; i++)
        {
            Add(keys[i], values[i]);
        }
    }
}

[Serializable]
public class StringSerializableDictionary : SerializableDictionary<string, string>
{
    
}
