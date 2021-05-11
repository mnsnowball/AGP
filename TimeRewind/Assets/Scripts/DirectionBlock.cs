using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public enum Direction{up, down, left, right, jump, wait}
public class DirectionBlock : MonoBehaviour
{

    public int numberOfIterations = 1;
    public int currentIteration = 0;
    public int maximumIterations = 5;
    public TextMeshProUGUI iterationText;
    public Direction direction;
    public bool isJumpTo = false;
    public bool hasBeenHandled = false;

    // these variables should only be used if this block is a jump block
    public DirectionBlock jumpTo;
    public bool hasJumpTo;
    public GameObject dust;


    [Header("Movement Settings")]
    public float moveSpeed;

    private bool isMoving;

    public int xPosition; // the x position on the blocks grid
    public int yPosition; // the y position on the blocks grid


    public IEnumerator Move(Vector3 targetPos){
        float moveTime = 0.0f;
        float maxMoveTime = 0.5f;
        //Debug.Log("Moving block");
        isMoving = true;
        while (EnvironmentManager.instance.canStartMove == false) { // dont move until the environment manager says you can move
            yield return null;
        }
        Instantiate(dust, transform.position, Quaternion.identity);
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            moveTime += Time.deltaTime;
            if (moveTime < maxMoveTime)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                //Debug.Log("Moving...");
                yield return null;
            }
            else
            {
                break;
            }
            
        }

        transform.position = targetPos;
        isMoving = false;
        //Debug.Log("Done moving");
        EnvironmentManager.instance.StopMove(); // reset the movement bool now that I'm done moving
                                                // movement is locked to a trigger in the player's animator, in the pushing/pulling animations
    }

    public void MoveToken(Vector3 targetPos){
        StartCoroutine(Move(targetPos));
    }

    public void IncrementX(){
        xPosition++;
    }

    public void IncrementY(){
        yPosition++;
    }

    public void DecrementX(){
        xPosition--;
    }

    public void DecrementY(){
        yPosition--;
    }

    public void AddIterations(){
        numberOfIterations++;
        if (numberOfIterations > maximumIterations)
        {
            numberOfIterations = 0;
        }
        UpdateText();
    }

    public void ResetIterations(){
        numberOfIterations = 0;
        UpdateText();
    }

    public void DecrementIterations(){
        numberOfIterations--;
        UpdateText();
    }

    void UpdateText(){
        iterationText.text = "x" + numberOfIterations.ToString();
    }

    public void SetJumpTo(DirectionBlock theBlock){
        jumpTo = theBlock;
        hasJumpTo = true;
        theBlock.isJumpTo = true;
        if (direction != Direction.jump)
        {
            Debug.LogWarning("I have a jumpto block when I shouldn't. My direction is: " + direction);
        }
    }

    public void RemoveJumpTo(){
        if(jumpTo != null){
            Debug.Log("Removing current jump to");
            jumpTo.isJumpTo = false;
            jumpTo = null;
            hasJumpTo = false;
        }
        if (direction != Direction.jump)
        {
            Debug.LogWarning("I have a jumpto block when I shouldn't. My direction is: " + direction);
        }
    }

}
