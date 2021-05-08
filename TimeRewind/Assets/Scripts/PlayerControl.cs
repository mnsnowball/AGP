using System.Collections;
using UnityEngine;

public enum MoveState{Idle, MovingLeft, MovingRight, MovingUp, MovingDown}
public enum RotationState{Right, Left, Up, Down}
public class PlayerControl : MonoBehaviour
{
    public Animator anim;
    float idleTime = 0f;
    public float idleAnimThreshold = 3f;

    [Header("Movement Settings")]
    public float moveSpeed;
    public float rotationSpeed = 5f;
    public RotationState rotationState;
    public MoveState moveState;
    public float moveIncrement = 1f;
    bool canMove = true;
    
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
    public bool castingEnabled;
    bool canCast = true;
    bool isCastingJump = false;
    DirectionBlock jumpHolder;

    ////////////////////////////////////////////////////

    // Start is called before the first frame update
    void Start()
    {
       xPosition = startingXPos;
       yPosition = startingYPos;
       moveState = MoveState.Idle; 
       isRotating = false;
    }

    ////////////////////////////////////////////////////

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleCasting();
    }

    ////////////////////////////////////////////////////

    void HandleCasting()
    {
        
        if (Input.GetKeyUp(castKey))
        {
            canCast = true;
        }
        if (Input.GetKey(castKey) && canCast && castingEnabled)
        {
            DirectionBlock theBlock = null;
            canCast = false;
            switch (rotationState)
            {
                case (RotationState.Up): 
                { 
                    // check the block above me
                    // if there is a block above me, increase its number of iterations
                    if (environmentManager.HasBlock(yPosition - 1, xPosition))
                    {
                        theBlock = environmentManager.GetBlock(yPosition - 1, xPosition);
                    }
                    break;
                }

                case (RotationState.Down):
                { 
                    // if there is a block below me, increase its number of iterations
                    if (environmentManager.HasBlock(yPosition + 1, xPosition))
                    {
                        theBlock = environmentManager.GetBlock(yPosition + 1, xPosition);
                    }
                    break;
                }

                case(RotationState.Left):
                { 
                    // if there is a block above me, increase its number of iterations
                    if (environmentManager.HasBlock(yPosition, xPosition - 1))
                    {
                        theBlock = environmentManager.GetBlock(yPosition, xPosition - 1);
                    }
                break;
                }

                case(RotationState.Right):
                { 
                    if (environmentManager.HasBlock(yPosition, xPosition + 1))
                    {
                        theBlock = environmentManager.GetBlock(yPosition, xPosition + 1);
                    }
                break;
                }

                default:
                { 
                    break;
                }
            }
            if (theBlock != null)
            {
                // casting on a jump block
                if (theBlock.direction == Direction.jump) 
                {
                    if (!isCastingJump) 
                    {
                        // initiate the cast
                        Debug.Log("Initiating jump cast");
                        isCastingJump = true;
                        jumpHolder = theBlock;

                        if (theBlock.hasJumpTo)
                        {
                            theBlock.RemoveJumpTo();
                        }
                        // enable line between staff and block
                    } 
                    else 
                    {
                        // cancel the cast
                        Debug.Log("Cancelling jump cast");
                        jumpHolder.RemoveJumpTo();
                        isCastingJump = false;
                        jumpHolder = null;
                        // disable line between staff and block
                    }
                } 
                else if (isCastingJump) 
                {
                    // if it's not a jump block but I'm casting, make it a jump to
                    Debug.Log("Concluding/Setting jump cast");
                    theBlock.isJumpTo = true;
                    jumpHolder.SetJumpTo(theBlock);
                    isCastingJump = false;
                    // give the jump block a reference to the direction block
                    // set the endPosition of the jumpTrailRenderer to theBlock
                } 
                else
                {
                    // it's not a jump block and I'm not casting, so just add iterations
                    theBlock.AddIterations();
                    anim.SetTrigger("Cast");
                }
                            
            }
        }
    }

    ////////////////////////////////////////////////////
    
    void HandleMovement()
    {
        if (moveState == MoveState.Idle && !isRotating && canMove)
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
                idleTime = 0f;
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
                    
                } 
                else if (input.x < 0) // going to the left
                {
                    moveState = MoveState.MovingLeft;
                }

                if (input.z > 0) // going up
                {
                    moveState = MoveState.MovingUp;
                    
                } 
                else if (input.z < 0) // going down
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
                        //rotationState = RotationState.Up;
                        StartCoroutine(Rotate(rotationState, RotationState.Up, new Vector3(transform.position.x, transform.position.y, transform.position.z - moveIncrement))); // rotate up function
                        moveState = MoveState.Idle;
                        return;
                    } 
                    else if (rotationState == RotationState.Down)
                    {
                        //if I'm facing down, check if I'm pressing interact button
                        if (interactButton)
                        {
                            // if so, handle it like I'm pulling
                            // check the spot under me
                            if (environmentManager.IsOccupied(yPosition + 1, xPosition))
                            {
                                // if it's an obstacle, rotate myself and return
                                //Debug.Log("I'm facing down and need to rotate up since I'm trying to pull an obstacle");
                                //rotationState = RotationState.Up;
                                StartCoroutine(Rotate(rotationState, RotationState.Up, new Vector3(transform.position.x, transform.position.y, transform.position.z - moveIncrement)));
                                moveState = MoveState.Idle;
                                return; // rotate up function
                            } 
                            else if (environmentManager.HasBlock(yPosition + 1, xPosition))
                            {
                                // if it's a block, does the spot under me have a block or obstruction?
                                if (environmentManager.HasBlock(yPosition - 1, xPosition) || environmentManager.IsOccupied(yPosition - 1, xPosition))
                                {
                                    // if so, indicate to the player that I can't pull in this direction with sound and animation
                                    //Debug.Log("Trying to pull up but there's a block or obstruction in the way!");
                                    moveState = MoveState.Idle;
                                    return;
                                } 
                                else 
                                { 
                                    // if not, move myself up and move the block to my current position
                                    //Debug.Log("Pulling up");
                                    anim.SetTrigger("Pulling");
                                    environmentManager.MoveBlock(yPosition + 1, xPosition, yPosition, xPosition);
                                }   
                                    
                            } 
                            else 
                            { // if there's not a block or obstacle there, just rotate and return
                                //rotationState = RotationState.Up;
                                StartCoroutine(Rotate(rotationState, RotationState.Up, new Vector3(transform.position.x, transform.position.y, transform.position.z - moveIncrement))); // rotate up function
                                moveState = MoveState.Idle;
                                return;
                            }
                        } 
                        else 
                        {
                            //rotationState = RotationState.Up;
                            StartCoroutine(Rotate(rotationState, RotationState.Up, new Vector3(transform.position.x, transform.position.y, transform.position.z - moveIncrement))); // rotate up function
                            moveState = MoveState.Idle;
                            return;
                        }
                            
                        //if I'm not, then I need to rotate up and return
                    }
                    else 
                    {
                        // I'm facing up
                        if (interactButton)
                        {
                            // if I am, we're pushing
                            //Debug.Log("Pushing up");
                            // check environmentmanager the way it is now
                            //if there's a block there, check the next one up
                            if (environmentManager.HasBlock(targetY, targetX))
                            {
                                //if that one has a block or an obstacle, don't do anything
                                if (environmentManager.HasBlock(targetY - 1, targetX) || environmentManager.IsOccupied(targetY - 1, targetX)) 
                                {
                                    //Debug.Log("Block or obstacle found up, not moving and returning instead...");
                                    moveState = MoveState.Idle;
                                    return;
                                } 
                                else 
                                {
                                    // if there's not a block at the next one, we can move but also move the block up
                                    // move block up
                                    //Debug.Log("Block found going up, should move it up");
                                    anim.SetTrigger("Pushing");
                                    environmentManager.MoveBlock(targetY, targetX, targetY - 1, targetX);
                                    // set the block's current position to false
                                    // set the block's next position to true
                                    
                                }
                                
                            }
                        } 
                        else if (environmentManager.HasBlock(targetY, targetX) || environmentManager.IsOccupied(targetY, targetX)) 
                        {
                            // if it isn't, we only need to check if there's a block or obstacle where we want to go
                            //Debug.Log("Not Moving up because there's a block or obstacle in the way");
                            moveState = MoveState.Idle;
                            return;
                        }
                        // if there's a block or obstruction where we wanna go, we indicate to the player that they can't go there and return     
                    } 
                break;

                case MoveState.MovingDown:
                {
                    // targety = current y index + 1
                    targetY = yPosition + 1;

                    // check if I'm facing left or right
                    if (rotationState == RotationState.Left || rotationState == RotationState.Right)
                    {
                        // if so, rotate and return
                        //rotationState = RotationState.Down;
                        StartCoroutine(Rotate(rotationState, RotationState.Down, new Vector3(transform.position.x, transform.position.y, transform.position.z + moveIncrement))); // rotate down function or rotate function
                        moveState = MoveState.Idle;
                        return;
                    }
                    else if (rotationState == RotationState.Up)
                    {
                        //if I'm facing up, check if I'm pressing interact button
                        if (interactButton)
                        {
                            // if so, handle it like I'm pulling
                            // check the spot on top of me
                            if (environmentManager.IsOccupied(yPosition - 1, xPosition))
                            {
                                // if it's an obstacle, rotate myself and return
                                //Debug.Log("I'm facing up and need to rotate down since I'm trying to pull an obstacle");
                                //rotationState = RotationState.Down;
                                StartCoroutine(Rotate(rotationState, RotationState.Down, new Vector3(transform.position.x, transform.position.y, transform.position.z + moveIncrement)));//rotate func
                                moveState = MoveState.Idle;
                                return;
                            }
                            else if (environmentManager.HasBlock(yPosition - 1, xPosition))
                            {
                                // if it's a block, does the spot under me have a block or obstruction?
                                if (environmentManager.HasBlock(yPosition + 1, xPosition) || environmentManager.IsOccupied(yPosition + 1, xPosition)) // is block occupied function
                                {
                                    // if so, indicate to the player that I can't pull in this direction with sound and animation
                                    //Debug.Log("Trying to pull but there's a block or obstruction in the way!");
                                    moveState = MoveState.Idle;
                                    return;
                                }
                                else
                                {
                                    // if not, move myself to the right and move the block to my current position
                                    //Debug.Log("Pulling down");
                                    anim.SetTrigger("Pulling");
                                    environmentManager.MoveBlock(yPosition - 1, xPosition, yPosition, xPosition);
                                }

                            }
                            else
                            { // if there's not a block or obstacle there, just rotate and return
                                //rotationState = RotationState.Down;
                                StartCoroutine(Rotate(rotationState, RotationState.Down, new Vector3(transform.position.x, transform.position.y, transform.position.z + moveIncrement)));
                                moveState = MoveState.Idle;
                                return;
                            }
                        }
                        else
                        {
                            //rotationState = RotationState.Down;
                            StartCoroutine(Rotate(rotationState, RotationState.Down, new Vector3(transform.position.x, transform.position.y, transform.position.z + moveIncrement)));
                            moveState = MoveState.Idle;
                            return;
                        }

                        //if I'm not, then I need to rotate down and return
                    }
                    else // I'm facing down
                    {
                        if (interactButton)
                        {
                            // if I am, we're pushing
                            //Debug.Log("Pushing down");
                            // check environmentmanager the way it is now
                            //if there's a block there, check the next one down
                            if (environmentManager.HasBlock(targetY, targetX))
                            {
                                //if that one has a block or an obstacle, don't do anything
                                if (environmentManager.HasBlock(targetY + 1, targetX) || environmentManager.IsOccupied(targetY + 1, targetX))
                                {
                                    //Debug.Log("Block or obstacle found down, not moving and returning instead...");
                                    moveState = MoveState.Idle;
                                    return;
                                }
                                else
                                {// if there's not a block at the next one, we can move but also move the block down
                                    // move block down
                                    //Debug.Log("Block found going down, should move it down");
                                    anim.SetTrigger("Pushing");
                                    environmentManager.MoveBlock(targetY, targetX, targetY + 1, targetX);
                                    // set the block's current position to false
                                    // set the block's next position to true

                                }

                            }
                        }
                        else if (environmentManager.HasBlock(targetY, targetX) || environmentManager.IsOccupied(targetY, targetX))
                        {// if it isn't, we only need to check if there's a block or obstacle where we want to go
                            //Debug.Log("Not Moving down because there's a block or obstacle in the way");
                            moveState = MoveState.Idle;
                            return;
                        }
                        // if there's a block or obstruction where we wanna go, we indicate to the player that they can't go there and return     
                    }
                    break;
                }
                case MoveState.MovingLeft:
                    // targetx = current x index - 1
                    targetX = xPosition - 1;

                    // check if I'm facing up or down
                    if (rotationState == RotationState.Up || rotationState == RotationState.Down)
                    {
                        // if I'm facing up or down, rotate, and return
                        //rotationState = RotationState.Left;
                        StartCoroutine(Rotate(rotationState, RotationState.Left, new Vector3(transform.position.x + moveIncrement, transform.position.y, transform.position.z)));
                        moveState = MoveState.Idle;
                        return;
                    } 
                    else if (rotationState == RotationState.Right)
                    {
                        //if I'm facing right, check if I'm pressing interact button
                        if (interactButton)
                        {
                            // if so, handle it like I'm pulling
                            // check the spot to the right of me
                            if (environmentManager.IsOccupied(yPosition, xPosition + 1))
                            {
                                // if it's an obstacle, rotate myself and return
                                //Debug.Log("I'm facing right and need to rotate to the left since I'm trying to pull an obstacle");
                                //rotationState = RotationState.Left;
                                StartCoroutine(Rotate(rotationState, RotationState.Left, new Vector3(transform.position.x + moveIncrement, transform.position.y, transform.position.z )));
                                moveState = MoveState.Idle;
                                return;
                            } 
                            else if (environmentManager.HasBlock(yPosition, xPosition + 1))
                            {
                                // if it's a block, does the spot to my left have a block or obstruction?
                                // checking to see if there's room for me to move
                                if (environmentManager.HasBlock(yPosition, xPosition - 1) || environmentManager.IsOccupied(yPosition, xPosition - 1))
                                {
                                    // if so, indicate to the player that I can't pull in this direction with sound and animation
                                    //Debug.Log("Trying to pull but there's a block or obstruction in the way!");
                                    moveState = MoveState.Idle;
                                    return;
                                } 
                                else 
                                { 
                                    // if not, move myself to the right and move the block to my current position
                                    //Debug.Log("Pulling to the left!");
                                    anim.SetTrigger("Pulling");
                                    environmentManager.MoveBlock(yPosition, xPosition + 1, yPosition, xPosition);
                                }   
                                    
                            } 
                            else 
                            { // if there's not a block or obstacle there, just rotate and return
                                //rotationState = RotationState.Left;
                                StartCoroutine(Rotate(rotationState, RotationState.Left, new Vector3(transform.position.x + moveIncrement, transform.position.y, transform.position.z)));
                                moveState = MoveState.Idle;
                                return;
                            }
                        } 
                        else 
                        {
                            //rotationState = RotationState.Left;
                            StartCoroutine(Rotate(rotationState, RotationState.Left, new Vector3(transform.position.x + moveIncrement, transform.position.y, transform.position.z)));
                            moveState = MoveState.Idle;
                            return;
                        }
                            
                        //if I'm not, then I need to rotate to the right and return
                    } 
                    else // I'm facing left
                    {
                        if (interactButton)
                        {
                            // if I am, we're pushing
                            //Debug.Log("Pushing to the left");
                            // check environmentmanager the way it is now
                            //if there's a block there, check the next one to the left
                            if (environmentManager.HasBlock(targetY, targetX))
                            {
                                //if that one has a block or an obstacle, don't do anything
                                if (environmentManager.HasBlock(targetY, targetX - 1) || environmentManager.IsOccupied(targetY, targetX - 1))
                                {
                                    //Debug.Log("Block or obstacle found going right, not moving and returning instead...");
                                    moveState = MoveState.Idle;
                                    return;
                                } 
                                else 
                                {
                                    // if there's not a block at the next one, we can move but also move the block to the right
                                    // move block to the right
                                    //Debug.Log("Block found going right, should move it right");
                                    anim.SetTrigger("Pushing");
                                    environmentManager.MoveBlock(targetY, targetX, targetY, targetX - 1);
                                    // set the block's current position to false
                                    // set the block's next position to true
                                    
                                } 
                            }
                        } 
                        else if (environmentManager.HasBlock(targetY, targetX) || environmentManager.IsOccupied(targetY, targetX)) 
                        {
                            // if it isn't, we only need to check if there's a block or obstacle where we want to go
                            //Debug.Log("Not Moving left because there's a block or obstacle in the way");
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
                        //rotationState = RotationState.Right;
                        StartCoroutine(Rotate(rotationState, RotationState.Right, new Vector3(transform.position.x - moveIncrement, transform.position.y, transform.position.z)));
                        moveState = MoveState.Idle;
                        return;
                    } 
                    else if (rotationState == RotationState.Left)
                    {
                        //if I'm facing left, check if I'm pressing interact button
                        if (interactButton)
                        {
                            // if so, handle it like I'm pulling
                            // check the spot to the left of me
                            if (environmentManager.IsOccupied(yPosition, xPosition - 1))
                            {
                                // if it's an obstacle, rotate myself and return
                                //Debug.Log("I'm facing left and need to rotate to the right since I'm trying to pull an obstacle");
                                //rotationState = RotationState.Right;
                                StartCoroutine(Rotate(rotationState, RotationState.Right, new Vector3(transform.position.x - moveIncrement, transform.position.y, transform.position.z )));
                                moveState = MoveState.Idle;
                                return;
                            } 
                            else if (environmentManager.HasBlock(yPosition, xPosition - 1))
                            {
                                // if it's a block, does the spot to my right have a block or obstruction?
                                if (environmentManager.HasBlock(yPosition, xPosition + 1) || environmentManager.IsOccupied(yPosition, xPosition + 1))
                                {
                                    // if so, indicate to the player that I can't pull in this direction with sound and animation
                                    //Debug.Log("Trying to pull but there's a block or obstruction in the way!");
                                    moveState = MoveState.Idle;
                                    return;
                                } 
                                else 
                                { 
                                    // if not, move myself to the right and move the block to my current position
                                    //Debug.Log("Pulling to the right!");
                                    anim.SetTrigger("Pulling");
                                    environmentManager.MoveBlock(yPosition, xPosition - 1, yPosition, xPosition);
                                }   
                                    
                            } 
                            else 
                            { // if there's not a block or obstacle there, just rotate and return
                                //rotationState = RotationState.Right;
                                StartCoroutine(Rotate(rotationState, RotationState.Right, new Vector3(transform.position.x - moveIncrement, transform.position.y, transform.position.z)));
                                moveState = MoveState.Idle;
                                return;
                            }
                        } 
                        else 
                        {
                            //rotationState = RotationState.Right;
                            StartCoroutine(Rotate(rotationState, RotationState.Right, new Vector3(transform.position.x - moveIncrement, transform.position.y, transform.position.z)));
                            moveState = MoveState.Idle;
                            return;
                        }
                            
                        //if I'm not, then I need to rotate to the right and return
                    } 
                    else // I'm facing right
                    {
                        if (interactButton)
                        {
                            // if I am, we're pushing
                            //Debug.Log("Pushing to the right new");
                            // check environmentmanager the way it is now
                            //if there's a block there, check the next one to the right
                            if (environmentManager.HasBlock(targetY, targetX))
                            {
                                //if that one has a block or an obstacle, don't do anything
                                if (environmentManager.HasBlock(targetY, targetX + 1) || environmentManager.IsOccupied(targetY, targetX + 1))
                                {
                                    //Debug.Log("Block or obstacle found going right, not moving and returning instead...");
                                    moveState = MoveState.Idle;
                                    return;
                                } 
                                else 
                                {
                                    // if there's not a block at the next one, we can move but also move the block to the right
                                    // move block to the right
                                    //Debug.Log("Block found going right, should move it right");
                                    environmentManager.MoveBlock(targetY, targetX, targetY, targetX + 1);
                                    anim.SetTrigger("Pushing");
                                    
                                } 
                            }
                        } 
                        else if (environmentManager.HasBlock(targetY, targetX) || environmentManager.IsOccupied(targetY, targetX)) 
                        {
                            // if it isn't, we only need to check if there's a block or obstacle where we want to go
                            //Debug.Log("Not Moving because there's a block or obstacle in the way");
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
                    anim.SetTrigger("Walking");
                    StartCoroutine(Move(targetPos));

                } 
                else 
                {
                    moveState = MoveState.Idle;
                }// if it's occupied,  dont do anything
                                
            }
            else
            {
                idleTime += Time.deltaTime;
                if (idleTime >= idleAnimThreshold)
                {
                    idleTime = 0f;
                    int randomNum = Random.Range(0, 2);
                    switch (randomNum)
                    {
                        case 0:
                        {
                            anim.SetTrigger("IdleBeardStroke");
                            //Debug.Log("Idle beard");
                            break;
                        }
                        case 1:
                        {
                            anim.SetTrigger("IdleYawn");
                            //Debug.Log("Idle yawn");
                            break;
                        }
                        default:
                        {
                            break;
                        }
                    }
                }
            }
        }
    }

    ////////////////////////////////////////////////////

    IEnumerator Move(Vector3 targetPos)
    {
        yield return new WaitForSeconds(0.4f);
        while ((targetPos - transform.position).sqrMagnitude > 0.000001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        moveState = MoveState.Idle;
        anim.SetTrigger("Idle");
        FixRotation();
    }

    ////////////////////////////////////////////////////

    IEnumerator Rotate(RotationState oldState, RotationState newState, Vector3 target)
    {
        isRotating = true;
        Vector3 targetDirection = new Vector3(0,0,0);
        targetDirection = transform.position  - target;

        //play turn animation
        switch (oldState)
        {
            case RotationState.Right:
                if (newState == RotationState.Down)
                {
                    anim.SetTrigger("TurnRight");
                }
                else if (newState == RotationState.Left)
                {
                    anim.SetTrigger("TurnLeft");
                }
                else if (newState == RotationState.Up)
                {
                    anim.SetTrigger("TurnRight");
                }
                break;
            case RotationState.Left:
                if (newState == RotationState.Down)
                {
                    anim.SetTrigger("TurnLeft");
                }
                else if (newState == RotationState.Right)
                {
                    anim.SetTrigger("TurnLeft");
                }
                else if (newState == RotationState.Up)
                {
                    anim.SetTrigger("TurnRight");
                }
                break;
            case RotationState.Up:
                if (newState == RotationState.Down)
                {
                    anim.SetTrigger("TurnLeft");
                }
                else if (newState == RotationState.Right)
                {
                    anim.SetTrigger("TurnRight");
                }
                else if (newState == RotationState.Left)
                {
                    anim.SetTrigger("TurnLeft");
                }
                break;
            case RotationState.Down:
                if (newState == RotationState.Up)
                {
                    anim.SetTrigger("TurnLeft");
                }
                else if (newState == RotationState.Right)
                {
                    anim.SetTrigger("TurnLeft");
                }
                else if (newState == RotationState.Left)
                {
                    anim.SetTrigger("TurnRight");
                }
                break;
            default:
                break;
        }
        rotationState = newState;
        // rotate in the target direction
        while(Vector3.Angle(transform.forward, targetDirection) > 3){
            Vector3 newDirection  = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime * rotationSpeed, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
            yield return null;
        }
        FixRotation();
        
        isRotating = false;
        yield return null;
    }

    ////////////////////////////////////////////////////

    void FixRotation() 
    {
        switch (rotationState)
        {
            case RotationState.Left:
                transform.eulerAngles = new Vector3(0f, -90f, 0f);
                break;

            case RotationState.Right:
                transform.eulerAngles = new Vector3(0f, 90f, 0f);
                break;

            case RotationState.Up:
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                break;

            case RotationState.Down:
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
                break;

            default:
                break;
        }
    }

    ////////////////////////////////////////////////////

    public void StopMoving()
    {
        canMove = false;
    }

    ////////////////////////////////////////////////////

    public void StartMoving()
    {
        canMove = true;
    }

    ////////////////////////////////////////////////////

    public void Defeated() 
    {
        anim.SetTrigger("Defeat");
    }

    ////////////////////////////////////////////////////

    public void Victory() 
    {
        anim.SetTrigger("Victory");
    }

    ////////////////////////////////////////////////////

    public void PlaySceneAnim() 
    {
        anim.SetTrigger("PlayScene");
    }

    ////////////////////////////////////////////////////
}
