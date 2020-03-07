using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fertools.Achievement;
using NaughtyAttributes;
using UnityEngine;

namespace Fertools.Achievement
{


    [CreateAssetMenu(menuName = "Achievements/Container")]
    public class AchievementsContainer : ScriptableObject
    {
        [ReorderableList]
        public List<Achievement> achievements;

        
        public void UnlockAchievement(string id)
        {
            var achievement =  GetAchievement(id);
            if(achievement != null)
                achievement.UnlockAchievement();
        }

        public void LockAchievement(string id)
        {
            var ach = GetAchievement(id);
            if(ach != null)
                ach.LockAchievement();
        }

        
        public void UnlockAllAchievements()
        {
            foreach (var achievement in achievements)
            {
                achievement.unlockStatus = true;
            }
        }

        public void LockAllAchievements()
        {
            
            foreach (var achievement in achievements)
            {
                achievement.unlockStatus = false;
            }
        }

        [Button()]
        public void ResetAllAchievements()
        {
            
            foreach (var achievement in achievements)
            {
                achievement.ResetAchievement();
            }
        }
        
        public void AddProgress(string id, int val)
        {
            var ach = GetAchievement(id);
            if(ach != null)
                ach.AddProgress(val);;
        }

      
      
        public void SetProgress(string id, int val)
        {
            var ach = GetAchievement(id);
            if(ach != null)
                ach.SetProgress(val);;
        }

     
        private Achievement GetAchievement(string id)
        {
         
            if (achievements.Count == 0) return null;
         
            foreach (var achievement in achievements.Where(achievement => string.Equals(achievement.id, id)))
            {
                return achievement;
            }

            Debug.LogError($"Achievement id: {id} not found.");
            return null;
        }
    }
}
