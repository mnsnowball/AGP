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
        Debug.Log("Playing");
        for (int i = 0; i < spaces.Length; i++)
        {
            if (spaces[i].hasBlock)
            {
                if (spaces[i].blockHeld.isJumpTo)
                {
                    hasFoundJumpTo = true;
                }
                if (spaces[i].blockHeld.direction == Direction.jump)
                {
                    hasFoundJumpBlock = true;
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
