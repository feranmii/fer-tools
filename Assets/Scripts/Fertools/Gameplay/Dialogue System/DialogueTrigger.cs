using System;
using System.Collections;
using System.Collections.Generic;
using Fertools.Dialogue;
using UnityEngine;
using UnityEngine.InputSystem;

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
      if (Keyboard.current.dKey.wasPressedThisFrame)
      {
         TriggerDialogue();

      }
      if (Keyboard.current.nKey.wasPressedThisFrame)
      {
         
         var ndp = new OnNextDialoguePressed();
         ndp.FireEvent();
      }
      
      /*
      if (Input.GetKeyDown(KeyCode.D))
      {
      }
      
      if (Input.GetKeyDown(KeyCode.N))
      {
         OnNextDialoguePressed ndp = new OnNextDialoguePressed();
         ndp.FireEvent();
      }
      */
      
      
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
