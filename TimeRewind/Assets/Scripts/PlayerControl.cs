using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed;

    private bool isMoving;
    private Vector3 input;
    private bool interactButton;

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
            interactButton = Input.GetKey(KeyCode.Space); // is the player pressing space as well?

            // makes it so you can't move diagonally
            if (input.x != 0)
            {
                input.z = 0;
            }

            if (input != Vector3.zero)
            {
                Debug.Log("Input detected");
                Vector3 targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.z += input.z;

                int targetX = xPosition;
                int targetY = yPosition;

                int blockTargetX = xPosition;
                int blockTargetY = yPosition;
                // check the target position
                // to get x-coordinate, check input
                if (input.x > 0) // going to the right
                {
                    // targetx = current x index + 1
                    targetX = xPosition + 1;

                    //if there's a block there, check the next one to the right
                    if (environmentManager.HasBlock(targetY, targetX))
                    {
                        //if that one has a block or an obstacle, don't do anything
                        if (environmentManager.HasBlock(targetY, targetX + 1) || environmentManager.IsOccupied(targetY, targetX + 1))
                        {
                            Debug.Log("Block or obstacle found going right, not moving and returning instead...");
                            return;
                        } else {// if there's not a block at the next one, we can move but also move the block to the right
                            // move block to the right
                            Debug.Log("Block found going right, should move it right");
                            environmentManager.MoveBlock(targetY, targetX, targetY, targetX + 1);
                            // set the block's current position to false
                            // set the block's next position to true
                            
                        }
                        
                    }
                    
                } else if (input.x < 0) // going to the left
                {
                    // targetx = current x index - 1
                    targetX = xPosition - 1;

                    //if there's a block there, check the next one to the left
                    if (environmentManager.HasBlock(targetY, targetX))
                    {
                        //if that one has a block or an obstacle, don't do anything
                        if (environmentManager.HasBlock(targetY, targetX - 1) || environmentManager.IsOccupied(targetY, targetX - 1))
                        {
                            Debug.Log("Block or obstacle found going left, not moving and returning instead...");
                            return;
                        } else {// if there's not a block at the next one, we can move but also move the block to the left
                            // move block to the left
                            Debug.Log("Block found going left, should move it left");
                            environmentManager.MoveBlock(targetY, targetX, targetY, targetX - 1);
                            // set the block's current position to false
                            // set the block's next position to true
                            
                        }
                        
                    }
                }

                if (input.z > 0) // going up
                {
                    // targety = current y index - 1
                    // inverted because y-indices increase as we move down
                    targetY = yPosition - 1;

                    //if there's a block there, check the next one up
                    if (environmentManager.HasBlock(targetY, targetX))
                    {
                        //if that one has a block or an obstacle, don't do anything
                        if (environmentManager.HasBlock(targetY - 1, targetX) || environmentManager.IsOccupied(targetY - 1, targetX))
                        {
                            Debug.Log("Block or obstacle found going up, not moving and returning instead...");
                            return;
                        } else {// if there's not a block at the next one, we can move but also move the block up one
                            // move block one up
                            Debug.Log("Block found going up, should move it up");
                            environmentManager.MoveBlock(targetY, targetX, targetY - 1, targetX);
                            // set the block's current position to false
                            // set the block's next position to true
                            
                        }
                        
                    }
                    
                } else if (input.z < 0) // going down
                {
                    // targety = current y index + 1
                    targetY = yPosition + 1;

                    //if there's a block there, check the next one down
                    if (environmentManager.HasBlock(targetY, targetX))
                    {
                        //if that one has a block or an obstacle, don't do anything
                        if (environmentManager.HasBlock(targetY + 1, targetX) || environmentManager.IsOccupied(targetY + 1, targetX))
                        {
                            Debug.Log("Block or obstacle found going down, not moving and returning instead...");
                            return;
                        } else {// if there's not a block at the next one, we can move but also move the block one down
                            // move block one down
                            Debug.Log("Block found going down, should move it down");
                            environmentManager.MoveBlock(targetY, targetX, targetY + 1, targetX);
                            // set the block's current position to false
                            // set the block's next position to true
                        }
                        
                    }
                }

                // if it's not occupied, set the current to not occupied and the next to occupied, and then move
                if (!environmentManager.IsOccupied(targetY, targetX))
                {
                    //environmentManager.SetOccupied(targetY, targetX, true);
                    //environmentManager.SetOccupied(yPosition, xPosition, false);
                    xPosition = targetX;
                    yPosition = targetY;
                    StartCoroutine(Move(targetPos));

                }// if it's occupied,  dont do anything
                                
            }
        }
        //Debug.Log("Frames per second: " + 1/Time.deltaTime);
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
