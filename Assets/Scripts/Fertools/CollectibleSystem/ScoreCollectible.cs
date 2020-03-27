using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCollectible : Collectible
{
    
    public override void Collect()
    {
        var ps = new OnPointScored();
        ps.FireEvent();
    }
}
