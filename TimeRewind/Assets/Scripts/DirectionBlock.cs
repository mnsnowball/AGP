using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Direction{up, down, left, right}
public class DirectionBlock : MonoBehaviour
{

    int numberOfIterations = 1;
    int currentIteration = 0;

    public Direction direction;

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
}
