using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveState{Idle, MovingLeft, MovingRight, MovingUp, MovingDown}
public enum RotationState{Right, Left, Up, Down}
public class PlayerControl : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed;
    public float rotationSpeed = 5f;
    public RotationState rotationState;
    public MoveState moveState;
    public float moveIncrement = 1f;
    
    private bool isMoving;
    private bool isRotating;
    private Vector3 input;
    private bool interactButton;

    [Header("Environment Settings")]
    public EnvironmentManager environmentManager; // assumes the player starts at [1,1] in the array
    public int startingXPos = 1;
    public int startingYPos = 1;
    int xPosition;
    int yPosition;

    

    [Header("Interactivity Settings")]
    public KeyCode interactKey;
    public KeyCode castKey = KeyCode.F;
    bool canCast = true;
    bool isCastingJump = false;
    DirectionBlock jumpHolder;
    

    // Start is called before the first frame update
    void Start()
    {
       xPosition = startingXPos;
       yPosition = startingYPos;
       moveState = MoveState.Idle; 
       isRotating = false;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleCasting();
    }

    void HandleCasting(){
        
        if (Input.GetKeyUp(castKey))
        {
            canCast = true;
        }
        if (Input.GetKey(castKey) && canCast)
        {
            DirectionBlock theBlock = null;
            //Debug.Log("Casting...");
            canCast = false;
            switch (rotationState)
            {
                /*
                Yeah! So there would be four cases: one where the jumpTo comes first, 
                one where the jumpFrom comes first, one where a jumpTo is found but no 
                jump from, and one where a jumpFrom is found but no jumpTo. In the last 
                two I'd just want it to skip it and carry on. So it can come before or after. 

                If the jumpFrom comes first, it'd look through the rest of the blocks for a 
                jumpTo and skip it if it doesn't find one.

                It the jumpTo comes first, it'll make a list of directions and iterate through 
                the rest until it finds a jumpFrom. If it doesn't find a jumpFrom then it 
                throws out the list and gets skipped, if it does find one then it adds the 
                list of directions to the client's queue for n times, with n being the number 
                of iterations on the jumpFrom
                */
                case(RotationState.Up): // check the block above me
                    // if there is a block above me, increase its number of iterations
                    if (environmentManager.HasBlock(yPosition - 1, xPosition))
                    {
                        theBlock = environmentManager.GetBlock(yPosition - 1, xPosition);
                    }
                break;

                case(RotationState.Down):
                    // if there is a block below me, increase its number of iterations
                    if (environmentManager.HasBlock(yPosition + 1, xPosition))
                    {
                        theBlock = environmentManager.GetBlock(yPosition + 1, xPosition);
                    }
                break;

                case(RotationState.Left):
                    // if there is a block above me, increase its number of iterations
                    if (environmentManager.HasBlock(yPosition, xPosition - 1))
                    {
                        theBlock = environmentManager.GetBlock(yPosition, xPosition - 1);
                    }
                break;

                case(RotationState.Right):
                    if (environmentManager.HasBlock(yPosition, xPosition + 1))
                    {
                        theBlock = environmentManager.GetBlock(yPosition, xPosition + 1);
                    }
                break;

                default:
                break;
            }
            if (theBlock != null)
            {
                if (theBlock.direction == Direction.jump) // casting on a jump block
                {
                    if (!isCastingJump) // initiate the cast
                    {
                        Debug.Log("Initiating jump cast");
                        isCastingJump = true;
                        jumpHolder = theBlock;

                        if (theBlock.hasJumpTo)
                        {
                            theBlock.RemoveJumpTo();
                        }
                        // enable line between staff and block
                    } else { // cancel the cast
                        Debug.Log("Cancelling jump cast");
                        jumpHolder.RemoveJumpTo();
                        isCastingJump = false;
                        jumpHolder = null;
                        // disable line between staff and block
                    }
                } else if (isCastingJump) // if it's not a jump block but I'm casting, make it a jump to
                {
                    Debug.Log("Concluding/Setting jump cast");
                    theBlock.isJumpTo = true;
                    jumpHolder.SetJumpTo(theBlock);
                    isCastingJump = false;
                    // give the jump block a reference to the direction block
                    // set the endPosition of the jumpTrailRenderer to theBlock
                } else{ // it's not a jump block and I'm not casting, so just add iterations
                    theBlock.AddIterations();
                }
                            
            }
        }
    }

    void HandleMovement(){
        if (moveState == MoveState.Idle && !isRotating)
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
                    moveState = MoveState.MovingRight;
                    
                } else if (input.x < 0) // going to the left
                {
                    moveState = MoveState.MovingLeft;
                }

                if (input.z > 0) // going up
                {
                    moveState = MoveState.MovingUp;
                    
                } else if (input.z < 0) // going down
                {
                    moveState = MoveState.MovingDown;
                }

                switch (moveState)
                {
                case MoveState.MovingUp:
                    // targety = current y index - 1
                    // inverted because y-indices increase as we move down
                    targetY = yPosition - 1;

                    // check if I'm facing up or down
                    if (rotationState == RotationState.Right || rotationState == RotationState.Left)
                    {
                        // if I'm facing left or right, rotate, and return
                        rotationState = RotationState.Up;
                        StartCoroutine(Rotate(rotationState, new Vector3(transform.position.x, transform.position.y, transform.position.z + moveIncrement)));
                        moveState = MoveState.Idle;
                        return;
                    } else if (rotationState == RotationState.Down)
                    {
                        //if I'm facing up, check if I'm pressing interact button
                        if (interactButton)
                        {
                            // if so, handle it like I'm pulling
                            // check the spot under me
                            if (environmentManager.IsOccupied(yPosition + 1, xPosition))
                            {
                                // if it's an obstacle, rotate myself and return
                                Debug.Log("I'm facing down and need to rotate up since I'm trying to pull an obstacle");
                                rotationState = RotationState.Up;
                                StartCoroutine(Rotate(rotationState, new Vector3(transform.position.x, transform.position.y, transform.position.z + moveIncrement)));
                                moveState = MoveState.Idle;
                                return;
                            } else if (environmentManager.HasBlock(yPosition + 1, xPosition))
                            {
                                // if it's a block, does the spot under me have a block or obstruction?
                                if (environmentManager.HasBlock(yPosition - 1, xPosition) || environmentManager.IsOccupied(yPosition - 1, xPosition))
                                {
                                    // if so, indicate to the player that I can't pull in this direction with sound and animation
                                    Debug.Log("Trying to pull up but there's a block or obstruction in the way!");
                                    moveState = MoveState.Idle;
                                    return;
                                } else { 
                                    // if not, move myself up and move the block to my current position
                                    Debug.Log("Pulling up");
                                    environmentManager.MoveBlock(yPosition + 1, xPosition, yPosition, xPosition);
                                }   
                                    
                            } else { // if there's not a block or obstacle there, just rotate and return
                                rotationState = RotationState.Up;
                                StartCoroutine(Rotate(rotationState, new Vector3(transform.position.x, transform.position.y, transform.position.z + moveIncrement)));
                                moveState = MoveState.Idle;
                                return;
                            }
                        } else {
                            rotationState = RotationState.Up;
                            StartCoroutine(Rotate(rotationState, new Vector3(transform.position.x, transform.position.y, transform.position.z + moveIncrement)));
                            moveState = MoveState.Idle;
                            return;
                        }
                            
                        //if I'm not, then I need to rotate up and return
                    } else // I'm facing up
                    {
                        if (interactButton)
                        {
                            // if I am, we're pushing
                            Debug.Log("Pushing up");
                            // check environmentmanager the way it is now
                            //if there's a block there, check the next one up
                            if (environmentManager.HasBlock(targetY, targetX))
                            {
                                //if that one has a block or an obstacle, don't do anything
                                if (environmentManager.HasBlock(targetY - 1, targetX ) || environmentManager.IsOccupied(targetY - 1, targetX))
                                {
                                    Debug.Log("Block or obstacle found up, not moving and returning instead...");
                                    moveState = MoveState.Idle;
                                    return;
                                } else {// if there's not a block at the next one, we can move but also move the block up
                                    // move block up
                                    Debug.Log("Block found going up, should move it up");
                                    environmentManager.MoveBlock(targetY, targetX, targetY - 1, targetX);
                                    // set the block's current position to false
                                    // set the block's next position to true
                                    
                                }
                                
                            }
                        } else if (environmentManager.HasBlock(targetY, targetX) || environmentManager.IsOccupied(targetY, targetX)) {// if it isn't, we only need to check if there's a block or obstacle where we want to go
                            Debug.Log("Not Moving up because there's a block or obstacle in the way");
                            moveState = MoveState.Idle;
                            return;
                        }
                        // if there's a block or obstruction where we wanna go, we indicate to the player that they can't go there and return     
                    } 
                break;

                case MoveState.MovingDown:
                    // targety = current y index + 1
                    targetY = yPosition + 1;

                    // check if I'm facing left or right
                    if (rotationState == RotationState.Left || rotationState == RotationState.Right)
                    {
                        // if so, rotate and return
                        rotationState = RotationState.Down;
                        StartCoroutine(Rotate(rotationState, new Vector3(transform.position.x, transform.position.y, transform.position.z - moveIncrement)));
                        moveState = MoveState.Idle;
                        return;
                    } else if (rotationState == RotationState.Up)
                    {
                        //if I'm facing up, check if I'm pressing interact button
                        if (interactButton)
                        {
                            // if so, handle it like I'm pulling
                            // check the spot on top of me
                            if (environmentManager.IsOccupied(yPosition - 1, xPosition))
                            {
                                // if it's an obstacle, rotate myself and return
                                Debug.Log("I'm facing up and need to rotate down since I'm trying to pull an obstacle");
                                rotationState = RotationState.Down;
                                StartCoroutine(Rotate(rotationState, new Vector3(transform.position.x, transform.position.y, transform.position.z - moveIncrement)));
                                moveState = MoveState.Idle;
                                return;
                            } else if (environmentManager.HasBlock(yPosition - 1, xPosition))
                            {
                                // if it's a block, does the spot under me have a block or obstruction?
                                if (environmentManager.HasBlock(yPosition + 1, xPosition) || environmentManager.IsOccupied(yPosition + 1, xPosition))
                                {
                                    // if so, indicate to the player that I can't pull in this direction with sound and animation
                                    Debug.Log("Trying to pull but there's a block or obstruction in the way!");
                                } else { 
                                    // if not, move myself to the right and move the block to my current position
                                    Debug.Log("Pulling down");
                                    environmentManager.MoveBlock(yPosition - 1, xPosition, yPosition, xPosition);
                                }   
                                    
                            } else { // if there's not a block or obstacle there, just rotate and return
                                rotationState = RotationState.Down;
                                StartCoroutine(Rotate(rotationState, new Vector3(transform.position.x, transform.position.y, transform.position.z - moveIncrement)));
                                moveState = MoveState.Idle;
                                return;
                            }
                        } else {
                            rotationState = RotationState.Down;
                            StartCoroutine(Rotate(rotationState, new Vector3(transform.position.x, transform.position.y, transform.position.z - moveIncrement)));
                            moveState = MoveState.Idle;
                            return;
                        }
                            
                        //if I'm not, then I need to rotate down and return
                    } else // I'm facing down
                    {
                        if (interactButton)
                        {
                            // if I am, we're pushing
                            Debug.Log("Pushing down");
                            // check environmentmanager the way it is now
                            //if there's a block there, check the next one down
                            if (environmentManager.HasBlock(targetY, targetX))
                            {
                                //if that one has a block or an obstacle, don't do anything
                                if (environmentManager.HasBlock(targetY + 1, targetX ) || environmentManager.IsOccupied(targetY + 1, targetX))
                                {
                                    Debug.Log("Block or obstacle found down, not moving and returning instead...");
                                    moveState = MoveState.Idle;
                                    return;
                                } else {// if there's not a block at the next one, we can move but also move the block down
                                    // move block down
                                    Debug.Log("Block found going down, should move it down");
                                    environmentManager.MoveBlock(targetY, targetX, targetY + 1, targetX);
                                    // set the block's current position to false
                                    // set the block's next position to true
                                    
                                }
                                
                            }
                        } else if (environmentManager.HasBlock(targetY, targetX) || environmentManager.IsOccupied(targetY, targetX)) {// if it isn't, we only need to check if there's a block or obstacle where we want to go
                            Debug.Log("Not Moving down because there's a block or obstacle in the way");
                            moveState = MoveState.Idle;
                            return;
                        }
                        // if there's a block or obstruction where we wanna go, we indicate to the player that they can't go there and return     
                    } 
                break;

                case MoveState.MovingLeft:
                    // targetx = current x index - 1
                    targetX = xPosition - 1;

                    

                    // check if I'm facing up or down
                    if (rotationState == RotationState.Up || rotationState == RotationState.Down)
                    {
                        // if I'm facing up or down, rotate, and return
                        rotationState = RotationState.Left;
                        StartCoroutine(Rotate(rotationState, new Vector3(transform.position.x - moveIncrement, transform.position.y, transform.position.z)));
                        moveState = MoveState.Idle;
                        return;
                    } else if (rotationState == RotationState.Right)
                    {
                        //if I'm facing right, check if I'm pressing interact button
                        if (interactButton)
                        {
                            // if so, handle it like I'm pulling
                            // check the spot to the right of me
                            if (environmentManager.IsOccupied(yPosition, xPosition + 1))
                            {
                                // if it's an obstacle, rotate myself and return
                                Debug.Log("I'm facing right and need to rotate to the left since I'm trying to pull an obstacle");
                                rotationState = RotationState.Left;
                                StartCoroutine(Rotate(rotationState, new Vector3(transform.position.x - moveIncrement, transform.position.y, transform.position.z )));
                                moveState = MoveState.Idle;
                                return;
                            } else if (environmentManager.HasBlock(yPosition, xPosition + 1))
                            {
                                // if it's a block, does the spot to my left have a block or obstruction?
                                // checking to see if there's room for me to move
                                if (environmentManager.HasBlock(yPosition, xPosition - 1) || environmentManager.IsOccupied(yPosition, xPosition - 1))
                                {
                                    // if so, indicate to the player that I can't pull in this direction with sound and animation
                                    Debug.Log("Trying to pull but there's a block or obstruction in the way!");
                                } else { 
                                    // if not, move myself to the right and move the block to my current position
                                    Debug.Log("Pulling to the left!");
                                    environmentManager.MoveBlock(yPosition, xPosition + 1, yPosition, xPosition);
                                }   
                                    
                            } else { // if there's not a block or obstacle there, just rotate and return
                                rotationState = RotationState.Left;
                                StartCoroutine(Rotate(rotationState, new Vector3(transform.position.x - moveIncrement, transform.position.y, transform.position.z)));
                                moveState = MoveState.Idle;
                                return;
                            }
                        } else {
                            rotationState = RotationState.Left;
                            StartCoroutine(Rotate(rotationState, new Vector3(transform.position.x - moveIncrement, transform.position.y, transform.position.z)));
                            moveState = MoveState.Idle;
                            return;
                        }
                            
                        //if I'm not, then I need to rotate to the right and return
                    } else // I'm facing left
                    {
                        if (interactButton)
                        {
                            // if I am, we're pushing
                            Debug.Log("Pushing to the left");
                            // check environmentmanager the way it is now
                            //if there's a block there, check the next one to the left
                            if (environmentManager.HasBlock(targetY, targetX))
                            {
                                //if that one has a block or an obstacle, don't do anything
                                if (environmentManager.HasBlock(targetY, targetX - 1) || environmentManager.IsOccupied(targetY, targetX - 1))
                                {
                                    Debug.Log("Block or obstacle found going right, not moving and returning instead...");
                                    moveState = MoveState.Idle;
                                    return;
                                } else {// if there's not a block at the next one, we can move but also move the block to the right
                                    // move block to the right
                                    Debug.Log("Block found going right, should move it right");
                                    environmentManager.MoveBlock(targetY, targetX, targetY, targetX - 1);
                                    // set the block's current position to false
                                    // set the block's next position to true
                                    
                                }
                                
                            }
                        } else if (environmentManager.HasBlock(targetY, targetX) || environmentManager.IsOccupied(targetY, targetX)) {// if it isn't, we only need to check if there's a block or obstacle where we want to go
                            Debug.Log("Not Moving left because there's a block or obstacle in the way");
                            moveState = MoveState.Idle;
                            return;
                        }
                        // if there's a block or obstruction where we wanna go, we indicate to the player that they can't go there and return     
                    } 
                break;

                case MoveState.MovingRight:
                    // targetx = current x index + 1
                    targetX = xPosition + 1;

                    // check if I'm facing up or down
                    if (rotationState == RotationState.Up || rotationState == RotationState.Down)
                    {
                        // if I'm facing up or down, rotate, and return
                        rotationState = RotationState.Right;
                        StartCoroutine(Rotate(rotationState, new Vector3(transform.position.x + moveIncrement, transform.position.y, transform.position.z)));
                        moveState = MoveState.Idle;
                        return;
                    } else if (rotationState == RotationState.Left)
                    {
                        //if I'm facing left, check if I'm pressing interact button
                        if (interactButton)
                        {
                            // if so, handle it like I'm pulling
                            // check the spot to the left of me
                            if (environmentManager.IsOccupied(yPosition, xPosition - 1))
                            {
                                // if it's an obstacle, rotate myself and return
                                Debug.Log("I'm facing left and need to rotate to the right since I'm trying to pull an obstacle");
                                rotationState = RotationState.Right;
                                StartCoroutine(Rotate(rotationState, new Vector3(transform.position.x + moveIncrement, transform.position.y, transform.position.z )));
                                moveState = MoveState.Idle;
                                return;
                            } else if (environmentManager.HasBlock(yPosition, xPosition - 1))
                            {
                                // if it's a block, does the spot to my right have a block or obstruction?
                                if (environmentManager.HasBlock(yPosition, xPosition + 1) || environmentManager.IsOccupied(yPosition, xPosition + 1))
                                {
                                    // if so, indicate to the player that I can't pull in this direction with sound and animation
                                    Debug.Log("Trying to pull but there's a block or obstruction in the way!");
                                } else { 
                                    // if not, move myself to the right and move the block to my current position
                                    Debug.Log("Pulling to the right!");
                                    environmentManager.MoveBlock(yPosition, xPosition - 1, yPosition, xPosition);
                                }   
                                    
                            } else { // if there's not a block or obstacle there, just rotate and return
                                rotationState = RotationState.Right;
                                StartCoroutine(Rotate(rotationState, new Vector3(transform.position.x + moveIncrement, transform.position.y, transform.position.z)));
                                moveState = MoveState.Idle;
                                return;
                            }
                        } else {
                            rotationState = RotationState.Right;
                            StartCoroutine(Rotate(rotationState, new Vector3(transform.position.x + moveIncrement, transform.position.y, transform.position.z)));
                            moveState = MoveState.Idle;
                            return;
                        }
                            
                        //if I'm not, then I need to rotate to the right and return
                    } else // I'm facing right
                    {
                        if (interactButton)
                        {
                            // if I am, we're pushing
                            Debug.Log("Pushing to the right new");
                            // check environmentmanager the way it is now
                            //if there's a block there, check the next one to the right
                            if (environmentManager.HasBlock(targetY, targetX))
                            {
                                //if that one has a block or an obstacle, don't do anything
                                if (environmentManager.HasBlock(targetY, targetX + 1) || environmentManager.IsOccupied(targetY, targetX + 1))
                                {
                                    Debug.Log("Block or obstacle found going right, not moving and returning instead...");
                                    moveState = MoveState.Idle;
                                    return;
                                } else {// if there's not a block at the next one, we can move but also move the block to the right
                                    // move block to the right
                                    Debug.Log("Block found going right, should move it right");
                                    environmentManager.MoveBlock(targetY, targetX, targetY, targetX + 1);
                                    // set the block's current position to false
                                    // set the block's next position to true
                                    
                                }
                                
                            }
                        } else if (environmentManager.HasBlock(targetY, targetX) || environmentManager.IsOccupied(targetY, targetX)) {// if it isn't, we only need to check if there's a block or obstacle where we want to go
                            Debug.Log("Not Moving because there's a block or obstacle in the way");
                            moveState = MoveState.Idle;
                            return;
                        }
                        // if there's a block or obstruction where we wanna go, we indicate to the player that they can't go there and return     
                    }                    
                break;

                default:
                break;
            }


                // if it's not occupied, set the current to not occupied and the next to occupied, and then move
                if (!environmentManager.IsOccupied(targetY, targetX))
                {
                    xPosition = targetX;
                    yPosition = targetY;
                    StartCoroutine(Move(targetPos));

                }else{
                    moveState = MoveState.Idle;
                }// if it's occupied,  dont do anything
                                
            }            
        }
    }

    IEnumerator Move(Vector3 targetPos){

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        moveState = MoveState.Idle;
    }

    IEnumerator Rotate(RotationState state, Vector3 target){
        isRotating = true;
        Vector3 targetDirection = new Vector3(0,0,0);
        targetDirection = transform.position  - target;
        // rotate in the target direction
        while(Vector3.Angle(transform.forward, targetDirection) > 3){
            Vector3 newDirection  = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime * rotationSpeed, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
            yield return null;
        }
        switch (state)
        {
            case RotationState.Left:
                transform.eulerAngles = new Vector3(0f, 90f, 0f);
            break;

            case RotationState.Right:
                transform.eulerAngles = new Vector3(0f, -90f, 0f);
            break;

            case RotationState.Up:
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
            break;

            case RotationState.Down:
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
            break;

            default:
            break;
        }
        
        isRotating = false;
    }
}
