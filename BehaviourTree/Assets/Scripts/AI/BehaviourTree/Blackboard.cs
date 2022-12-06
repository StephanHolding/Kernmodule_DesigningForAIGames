using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard
{
    private readonly Dictionary<string, object> blackboardData = new Dictionary<string, object>();

    public void SetVariable(string _key, object _var)
    {
        if (!blackboardData.ContainsKey(_key))
        {
            blackboardData.Add(_key, _var);
        }
        else
        {
            blackboardData[_key] = _var;
        }
    }

    public T GetVariable<T>(string _key)
    {
        return (T)blackboardData[_key];
    }

}
