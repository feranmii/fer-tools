using System;
using System.Collections;
using System.Collections.Generic;
using EventCallbacks;
using Fertools.Dialogue;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class DialogueUIManager : MonoBehaviour
{

    [BoxGroup("Text")]
    public TextMeshProUGUI nameText;
    [BoxGroup("Text")]
    public TextMeshProUGUI dialogueText;

    [ReadOnly]
    public Dialogue currentDialogue;
    private Queue<string> _sentences;

    private void Start()
    {
        _sentences = new Queue<string>();
    }

    private void OnEnable()
    {
        OnNextDialoguePressed.RegisterListener(NextSentence);
        OnDialogueEnded.RegisterListener(ProcessEndDialogue);
    }

    private void OnDisable()
    {
        OnNextDialoguePressed.UnregisterListener(NextSentence);
        OnDialogueEnded.UnregisterListener(ProcessEndDialogue);

    }

    private void ProcessEndDialogue(OnDialogueEnded ended)
    {
        nameText.gameObject.SetActive(false);
        dialogueText.gameObject.SetActive(false);
    }
    private void NextSentence(OnNextDialoguePressed nextDialoguePressed)
    {
        NextSentence();
    }
    
    
    public void StartDialogue(Dialogue dialogue)
    {
        nameText.gameObject.SetActive(true);
        dialogueText.gameObject.SetActive(true);

        
        currentDialogue = dialogue;
        nameText.text = dialogue.actor;
        _sentences.Clear();

        //Add Dialogue Events
        
        foreach (var s in dialogue.sentences)
        {
            _sentences.Enqueue(s.sentence);
        }
        
        NextSentence();
    }

    public void NextSentence()
    {
      
        if (_sentences.Count == 0)
        {
            currentDialogue.isFinished = true;
            EndDialogue();
            return;
        }

        string sentence = _sentences.Dequeue();
        //dialogueText.text = sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    public void EndDialogue()
    {
        if (currentDialogue.isFinished && currentDialogue.nextDialogue != null)
            StartDialogue(currentDialogue.nextDialogue);
        else
        {
            OnDialogueEnded de = new OnDialogueEnded();
            de.FireEvent();
            
            print("End of Conversation");
        }
            
        
    }
    
    private IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
        
    }

}

public class OnNextDialoguePressed : Event<OnNextDialoguePressed>
{
    
}

public class OnDialogueEnded : Event<OnDialogueEnded>
{
    
}