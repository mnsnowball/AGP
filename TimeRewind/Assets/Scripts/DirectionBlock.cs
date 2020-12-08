using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum Direction{up, down, left, right, jump, wait}
public class DirectionBlock : MonoBehaviour
{

    public int numberOfIterations = 1;
    public int currentIteration = 0;
    public TextMeshProUGUI iterationText;
    public Direction direction;
    public bool isJumpTo = false;

    [Header("Movement Settings")]
    public float moveSpeed;

    private bool isMoving;

    public int xPosition; // the x position on the blocks grid
    public int yPosition; // the y position on the blocks grid

    public IEnumerator Move(Vector3 targetPos){
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
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
}
