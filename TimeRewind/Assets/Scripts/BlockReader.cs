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
                        if(spaces[j].hasBlock && spaces[j].blockHeld.direction == Direction.jump && spaces[j].blockHeld.hasJumpTo && spaces[j].blockHeld.jumpTo == spaces[i].blockHeld){
                            // then break out of the list and add jumpDirections to the client for the number of times that
                            // the jump block loops
                            jump = spaces[j].blockHeld;
                            Debug.Log("Found jump block. Adding direction set.");
                            jumpFound = true;
                            for (int k = 0; k < jump.numberOfIterations + 1; i++)
                            {
                                theClient.AddDirectionSet(jumpDirections);
                            }
                            
                            spaces[j].blockHeld.hasBeenHandled = true;
                            i = j;
                            break;
                        } else{
                            if (spaces[j].hasBlock)
                            {
                                for (int k = 0; k < spaces[j].blockHeld.numberOfIterations; k++)
                                {
                                    Debug.Log("Adding " + spaces[j].blockHeld.direction);
                                    jumpDirections.Add(spaces[i].blockHeld.direction);
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
