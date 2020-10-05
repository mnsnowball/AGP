using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed;

    private bool isMoving;
    private Vector3 input;

    public EnvironmentManager environmentManager; // assumes the player starts at [1,1] in the array
    int xPosition;
    int yPosition;

    // Start is called before the first frame update
    void Start()
    {
       xPosition = 1;
       yPosition = 1; 
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.z = Input.GetAxisRaw("Vertical");
        

            // makes it so you can't move diagonally
            if (input.x != 0)
            {
                input.z = 0;
            }

            if (input != Vector3.zero)
            {
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.z += input.z;

                int targetX = xPosition;
                int targetY = yPosition;
                // check the target position
                // to get x-coordinate, check input
                if (input.x > 0) // going to the right
                {
                    // targetx = current x index + 1
                    targetX = xPosition + 1;
                    
                } else if (input.x < 0) // going to the left
                {
                    // targetx = current x index - 1
                    targetX = xPosition - 1;
                }

                if (input.z > 0) // going up
                {
                    // targety = current y index + 1
                    targetY = yPosition - 1;
                    
                } else if (input.z < 0) // going down
                {
                    // targety = current y index - 1
                    targetY = yPosition + 1;
                }

                // if it's not occupied, set the current to not occupied and the next to occupied, and then move
                if (!environmentManager.IsOccupied(targetY, targetX))
                {
                    environmentManager.SetOccupied(targetY, targetX, true);
                    environmentManager.SetOccupied(yPosition, xPosition, false);
                    xPosition = targetX;
                    yPosition = targetY;
                    StartCoroutine(Move(targetPos));
                } else {// if it's occupied,  debug.log
                    //Debug.Log("You are trying to move to a space that is out of bounds");
                    //Debug.Log("Current x: " + xPosition + "   Current y: " + yPosition + "   TargetPosX: " + targetX + "   TargetPosY: " + targetY);
                
                }
                                
            }
        }
    }

    IEnumerator Move(Vector3 targetPos){
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
