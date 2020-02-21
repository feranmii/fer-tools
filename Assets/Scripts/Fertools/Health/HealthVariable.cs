using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Health/Health Variable")]
public class HealthVariable : ScriptableObject
{

    private int _health = 100;
    
    public int Value => _health;

    public int min = 0;
    public int max = 100;

    public void Reduce(int value)
    {
        _health -= value;

        if (_health <= min)
            _health = min;
        
    }

    public void Add(int value)
    {
        _health += value;

        if (_health >= max)
            _health = max;
        
    }

    public void Set(int value)
    {
        _health = value;
    }

    public void Reset()
    {
        _health = max;
    }
    
}
