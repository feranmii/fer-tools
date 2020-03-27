using System;
using System.Collections;
using System.Collections.Generic;
using EventCallbacks;
using NaughtyAttributes;
using UnityEngine;

namespace Fertools.Achievement
{


    public class AchievementListener : MonoBehaviour
    {
        
        [Required("No achievements container found")]
        public AchievementsContainer achievements;

        public void OnEnable()
        {
            TestEnemyDeathEvent.RegisterListener(EnemyDeathAchievement);
        }

        private void OnDisable()
        {
            
            TestEnemyDeathEvent.UnregisterListener(EnemyDeathAchievement);
        }


        private void EnemyDeathAchievement(TestEnemyDeathEvent test)
        {
            achievements.AddProgress("deaths", 1);
        }
        
    }
}

public class TestEnemyDeathEvent : Event<TestEnemyDeathEvent>
{
    
}
