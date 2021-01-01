using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockReader : MonoBehaviour
{
    public BlockSpace[] spaces;
    public int numberOfSpaces;
    public Client theClient;
    public List<Direction> jumpDirections;
    private void Start() {
        
    }

    public void Play(){
        Debug.Log("Playing");
        for (int i = 0; i < spaces.Length; i++)
        {
            if (spaces[i].hasBlock)
            {
                if (spaces[i].blockHeld.isJumpTo)
                {
                    jumpDirections = new List<Direction>();
                    bool jumpFound = false;
                    DirectionBlock jump = null;
                    Debug.Log("Looking for jump block");
                    // iterate through the blocks from here and add each one to jumpDirections
                    for (int j = i; j < spaces.Length; j++)
                    {
                        BlockSpace currentSpace = spaces[j];
                        if(currentSpace.hasBlock && currentSpace.blockHeld.direction == Direction.jump && currentSpace.blockHeld.hasJumpTo && currentSpace.blockHeld.jumpTo == spaces[i].blockHeld){
                            // then break out of the list and add jumpDirections to the client for the number of times that
                            // the jump block loops
                            jump = currentSpace.blockHeld;
                            Debug.Log("Found jump block. Adding direction set.");
                            jumpFound = true;
                            for (int k = 0; k < jump.numberOfIterations + 1; k++)
                            {
                                theClient.AddDirectionSet(jumpDirections);
                            }
                            
                            currentSpace.blockHeld.hasBeenHandled = true;
                            i = j; // skips the jump block
                            break;
                        } else{
                            if (currentSpace.hasBlock)
                            {
                                for (int counter = 0; counter < currentSpace.blockHeld.numberOfIterations; counter++)
                                {
                                    Debug.Log("Adding " + currentSpace.blockHeld.direction);
                                    jumpDirections.Add(currentSpace.blockHeld.direction);
                                }
                            }
                            
                        }
                    }
                    
                    if(!jumpFound){
                        jumpDirections.Clear();
                        Debug.LogError("Jump block not found");
                    } else{
                        continue;
                    }
                    // then set i to be the jump block's index
                    // if you didn't find the jump block then throw an error and 
                    // complete this one as normal
                }
                if (spaces[i].hasBlock && spaces[i].blockHeld.direction == Direction.jump && !spaces[i].blockHeld.hasBeenHandled)
                {
                    bool hasFoundJumpTo = false;
                    // iterate through the list looking for my jumpTo
                    // if I find it, set i to be the index of it,
                    // and proceed as normal
                    for (int j = i; j < spaces.Length; j++)
                    {
                        if(spaces[j].hasBlock && spaces[j].blockHeld == spaces[i].blockHeld.jumpTo){
                            i = j;
                            hasFoundJumpTo = true;
                            break;
                        }
                    }
                    if (!hasFoundJumpTo)
                    {
                        Debug.LogError("Jump found but not jump to. Continuing");
                        continue;
                    }
                    
                }
                for (int k = 0; k < spaces[i].blockHeld.numberOfIterations; k++)
                {
                    theClient.AddDirection(spaces[i].blockHeld.direction);
                }
            }
        }
        theClient.hasFinished = true;

    }
}
