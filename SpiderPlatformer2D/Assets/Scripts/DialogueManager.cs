using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialougeText;
    private Queue<string> sentences;
    
  
    public float typingSpeed;

    public GameObject continueButton;
    public GameObject backgroundImage;
    public GameObject canvasDialog;
    public GameObject[] DialogableCharacter;




    void Start()
    {
        sentences = new Queue<string>();
    }
  
    public void StartDialogue(Dialogue dialogue)
    {
        
        Debug.Log("Starting with conversation" + dialogue.name);

        nameText.text = dialogue.name;
      
        
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    
    }
    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialougeText.text = sentence;
    }
    void EndDialogue()
    {
        Debug.Log("end of con");

    }
}
