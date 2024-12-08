using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SerializableDictionary<T, U>
{
    [SerializeField]
    private DictionaryItem<T, U>[] _dictItems;

    public Dictionary<T, U> ToDictionary()
    {
        Dictionary<T, U> newDict = new Dictionary<T, U>();

        foreach (var item in _dictItems)
        {
            newDict.Add(item.Key, item.Value);
        }

        return newDict;
    }
}

[System.Serializable]
public struct DictionaryItem<T, U>
{
    [SerializeField]
    public T Key;
    [SerializeField]
    public U Value;
}