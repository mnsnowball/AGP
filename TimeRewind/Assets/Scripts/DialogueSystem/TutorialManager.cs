using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    int actionIndex = 0;

    public Dialogue[] tutorials;
    Dialogue activeDialogue;
    DialogueManager dm;
    public EnvironmentManager em;
    bool canTalk = false;
    public bool isTalking = false;
    public bool canProceed = false;

    private void Start() {
        dm = FindObjectOfType<DialogueManager>();
        activeDialogue = tutorials[0];
        TriggerDialogue();
    }

    public void TriggerDialogue()
    {
        GameManager.instance.StopPlayerMoving();
        isTalking = true;

        dm.StartDialogue(activeDialogue);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Interact") && isTalking)
        {
            canProceed = true;
        }

        if (isTalking && Input.GetButton("Interact") && canProceed)
        {
            canProceed = false;
            FindObjectOfType<DialogueManager>().DisplayNextSentence();
        }
        if (!isTalking)
        {
            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case(0):
                break;
                case(1):
                RunChecksLevelOne();
                break;
                default:
                break;
            }
        }
    }

    public void StopTalking(){
        GameManager.instance.StartPlayerMoving();
        isTalking = false;
    }

    IEnumerator IncrementAfterTime(float toWait){
        actionIndex++;
        activeDialogue = tutorials[actionIndex];
        yield return new WaitForSeconds(toWait);
        TriggerDialogue();
    }

    public void RunChecksLevelOne(){
        switch (actionIndex)
        {
            case(0):
                if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                {
                    StartCoroutine(IncrementAfterTime(1.2f));
                    
                }
            break;
            case(1):
                if (em.hasMovedBlock)
                {
                    actionIndex++;
                    activeDialogue = tutorials[actionIndex];
                    TriggerDialogue();
                    
                }
            break;
            default:
            break;
        }

    }
}
