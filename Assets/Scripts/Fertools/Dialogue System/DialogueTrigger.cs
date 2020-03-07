using System;
using System.Collections;
using System.Collections.Generic;
using Fertools.Dialogue;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
   public Dialogue dialogue;

   private DialogueUIManager _dialogueUiManager;
   
   private void Awake()
   {
      _dialogueUiManager = FindObjectOfType<DialogueUIManager>();
      
   }

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.D))
      {
         TriggerDialogue();
      }
      
      if (Input.GetKeyDown(KeyCode.N))
      {
         OnNextDialoguePressed ndp = new OnNextDialoguePressed();
         ndp.FireEvent();
      }
      
      
   }

   public void TriggerDialogue()
   {
      if (_dialogueUiManager == null || dialogue == null)
      {
         Debug.LogError("Missing Dialogue UI Manager or Dialogue Object");
         return;
      }
      
      _dialogueUiManager.StartDialogue(dialogue);
   }
}
