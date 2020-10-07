using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionBlock : MonoBehaviour
{

    public enum Direction{up, down, left, right}

    public Direction theDirection;

    [Header("Movement Settings")]
    public float moveSpeed;

    private bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
}
