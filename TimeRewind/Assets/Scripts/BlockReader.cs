using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockReader : MonoBehaviour
{
    public BlockSpace[] spaces;
    public int numberOfSpaces;
    public Client theClient;
    bool hasFoundJumpBlock = false;
    bool hasFoundJumpTo = false;

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
                    if (hasFoundJumpBlock)
                    {
                        
                    } else{
                        hasFoundJumpTo = true;
                    }
                    
                }
                if (spaces[i].blockHeld.direction == Direction.jump)
                {
                    if (hasFoundJumpTo)
                    {
                        
                    } else{
                        hasFoundJumpBlock = true;
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
