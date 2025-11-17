using System.Collections.Generic;
using UnityEngine;

public class SkirmishContext
{
    public SkirmishOverseer Manager { get; private set; }

    public SkirmishContext(SkirmishOverseer manager)
    {
        this.Manager = manager;
    }
    
    public bool IsBattleOver = false;
    private Dictionary<string, object> _flags = new Dictionary<string, object>();

    public void SetFlag(string key, object value)
    {
        _flags[key] = value;
        Debug.Log($"Context: {key} = {value}");
    }

    public bool GetFlag(string key)
    {
        if (_flags.TryGetValue(key, out object value))
        {
            if (value is bool)
            {
                return (bool)value;
            }
        }
        return false;
    }
    
    public T GetData<T>(string key)
    {
        if (_flags.TryGetValue(key, out object value))
        {
            if (value is T)
            {
                return (T)value;
            }
        }
        return default(T);
    }
    
    public void RemoveFlag(string key)
    {
        _flags.Remove(key);
    }
}