using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Score Table")]
public class ScoreTable : ScriptableObject
{
    public int scorePoint = 1;
    public int streakScore = 2;
}
