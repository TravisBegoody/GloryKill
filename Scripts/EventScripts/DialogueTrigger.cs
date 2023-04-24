using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    private bool canDialogue;//You can talk to the npc
    private void Start()
    {
        TriggerDialogue();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && canDialogue)
        {
            FindObjectOfType<DialogueManager>().DisplayNextSentence();
        }
    }
    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canDialogue = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canDialogue = false;
        }
    }
}
