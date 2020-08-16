using System.Collections;
using System.Collections.Generic;
using EventCallbacks;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
   public Battle currentBattle;

   public void NextBattle()
   {
      
   }

   public void EndBattle()
   {
      
   }
}

public class OnBattleStarted : Event<OnBattleStarted>
{
    
}

public class OnBattleEnded : Event<OnBattleEnded>
{
    
}