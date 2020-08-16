using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Fertools.Dialogue
{
    [CreateAssetMenu(menuName = "Dialogue/Dialogue Object")]
    public class Dialogue : ScriptableObject
    {
        
        [Dropdown("StringValues")]
            
        public string actor;
        
        [NonSerialized] public bool isFinished;

        
        [HorizontalLine()]
        [ReorderableList]
        public List<Sentence> sentences = new List<Sentence>();
        public Dialogue nextDialogue;

        private List<string> StringValues => DialogueActors();

        //TODO: Make it dynamic to change
        List<string> DialogueActors()
        {
            return new List<string>
            {
                "Allen", "Bucky", "Clair", "Dean", "Elaine"
            }; 
        }

        
    }

    [System.Serializable]
    public class Sentence
    {
        [ResizableTextArea] public string sentence;
    }
}
