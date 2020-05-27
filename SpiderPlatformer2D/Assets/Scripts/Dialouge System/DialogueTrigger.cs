using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public Dialogue dialogueIdle;
    public Dialogue completeDialogue;

    public void TriggerDialogue(bool missionCompleted,int playersCame)
    {

        if (missionCompleted)
        {
            DialogueManager.Instance.StartDialogue(completeDialogue);
            return;
        }
        if (playersCame == 1)
        {
            DialogueManager.Instance.StartDialogue(dialogue);
        }
        else if(playersCame>1)
        {
            DialogueManager.Instance.StartDialogue(dialogueIdle); ;
        }
        
    }
}
