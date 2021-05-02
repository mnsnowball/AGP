using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Animator animator;
    public TutorialManager tm;
    private Queue<string> sentences;
    // Start is called before the first frame update
    void Awake()
    {
        sentences = new Queue<string>();
    }

    public void SkipDialogue() {
        EndDialogue();
    }

    

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("isOpen", true);
        
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
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();

        StopAllCoroutines();
        
        StartCoroutine(TypeSentence(sentence));
        
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        //AudioManager.instance.PlayRandomDialogueAudio();
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            
            yield return null;
        }

        
    }

    void EndDialogue()
    {
        CameraManager.ins.SetChangeEnabled(true);
        tm.StopTalking();
        animator.SetBool("isOpen", false);
        
    }
}
