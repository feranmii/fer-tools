using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public abstract class BaseVariable : ScriptableObject
{
}

public class BaseVariable<T> : BaseVariable
{
    public bool resetOnStart = true;

    public T _value;

    public Action<T> onValueChanged;

    public T Value
    {
        get => _value;
        set
        {
            SetValue(value);
            _value = value;
        }
    }

    void SetValue(T value)
    {
        T oldValue = _value;
        T newValue = value;

        if (!newValue.Equals(oldValue))
        {
            onValueChanged?.Invoke(value);
        }
    }

    public override string ToString()
    {
        return _value == null ? "null" : _value.ToString();
    }


    private void OnEnable()
    {
        if (resetOnStart)
        {
            _value = default(T);
        }
    }
}